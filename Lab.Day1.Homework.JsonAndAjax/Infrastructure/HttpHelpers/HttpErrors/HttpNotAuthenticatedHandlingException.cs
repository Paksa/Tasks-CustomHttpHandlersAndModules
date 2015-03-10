using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Lab.Day1.Homework.JsonAndAjax.Infrastructure.HttpHelpers.HttpErrors
{
    public class HttpNotAuthenticatedHandlingException : HttpHandlingException
    {
        public HttpNotAuthenticatedHandlingException()
        { }

        public HttpNotAuthenticatedHandlingException(string message) : base(message)
        {
            
        }

        public override int Status
        {
            get { return 401; }
        }

        public override bool IsCancellingRequest
        {
            get { return true; }
        }

        public override void ProcessRequest(HttpContext context)
        {
            context.Response.StatusCode = Status;
            if (string.IsNullOrEmpty(base.Message)) context.Response.Write("Access denied");
            else context.Response.Write(base.Message);
            context.Response.End();
        }
    }
}
