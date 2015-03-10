using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab.Day1.Homework.JsonAndAjax.Infrastructure.HttpHelpers.HttpErrors
{
    public class HttpStatusResponseHandlingException: HttpHandlingException
    {
        private readonly int _status;
        public HttpStatusResponseHandlingException(int status) : base()
        {
            _status = status;
        }

        public HttpStatusResponseHandlingException(int status, string message) : base(message)
        {
            _status = status;
        }
        
        public override int Status { get {return _status;} }

        public override bool IsCancellingRequest { get { return true; } }

        public override void ProcessRequest(HttpContext context)
        {
            context.Response.StatusCode = Status;
            if (string.IsNullOrEmpty(base.Message)) return;
            context.Response.Write(base.Message);
        }
    }
}