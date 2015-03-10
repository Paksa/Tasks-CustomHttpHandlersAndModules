using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Lab.Day1.Homework.JsonAndAjax.CustomHandlers
{
    //TODO: Give it more readable name
    /// <summary>
    /// Manage default page (can return different pages on one url, now configured for index.html)
    /// </summary>
    public class DefaultPageRouterHandler : HttpTaskAsyncHandler
    {
        public override Task ProcessRequestAsync(HttpContext context)
        {
            Action asyncHtmlReader = () =>
            {
                try
                {
                    //var absoluteHtmlPath = context.Request.CurrentExecutionFilePath;
                    //using (var htmlPage = File.Open(absoluteHtmlPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    //{
                        if (context.User.Identity.IsAuthenticated)
                        {
                            context.Response.WriteFile("~/WelcomPage.html");
                        }
                        else
                        {
                            context.Response.WriteFile("~/index.html");
                        }
                        context.Response.ContentType = "text/html";
                    //}
                }
                catch (FileNotFoundException ex)
                {
                    context.Response.StatusCode = 404;
                    context.Response.End();
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = 500;
                    context.Response.End();
                }
            };
            var task = new Task(asyncHtmlReader);
            task.Start();
            return task;
        }
    }
}