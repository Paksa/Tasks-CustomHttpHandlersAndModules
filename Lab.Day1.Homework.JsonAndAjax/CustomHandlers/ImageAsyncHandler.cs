using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace Lab.Day1.Homework.JsonAndAjax.CustomHandlers
{
    /// <summary>
    /// Return images in response using async operations
    /// </summary>
    public class ImageAsyncHandler : HttpTaskAsyncHandler
    {
        public override Task ProcessRequestAsync(HttpContext context)
        {
            Action asyncImageReader = () =>
            {
                try
                {
                    var absoluteImagePath = context.Request.PhysicalPath;
                    using (var imageFile = File.Open(absoluteImagePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        imageFile.CopyTo(context.Response.OutputStream);
                        context.Response.ContentType = "image/jpg";
                    }
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
            var task = new Task(asyncImageReader);
            task.Start();
            return task;
        }

        public override bool IsReusable
        {
            get { return true; }
        }
    }
}
