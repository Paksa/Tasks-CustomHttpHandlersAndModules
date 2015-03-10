using System;
using System.IO;
using System.Web;

namespace Lab.Day1.Homework.JsonAndAjax.CustomHandlers
{
    public class ContactsClassHandler : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.CurrentExecutionFilePathExtension == null) return;
            if (context.Request.CurrentExecutionFilePathExtension == ".json")
            {
                try
                {
                    var jsonPath = context.Request.PhysicalPath;
                    if (File.Exists(jsonPath))
                    {
                        context.Response.ContentType = "application/json";
                        context.Response.WriteFile(jsonPath);
                    }
                }
                catch (FileNotFoundException ex)
                {
                    context.Response.StatusCode = 404;
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = 500;
                }
            }
            
        }

        #endregion
    }
}
