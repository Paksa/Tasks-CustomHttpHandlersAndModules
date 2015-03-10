using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Lab.Day1.Homework.JsonAndAjax.Infrastructure.HttpHelpers.HttpErrors
{
    public abstract class HttpHandlingException : Exception, IHttpHandler, IHttpResult
    {
        protected HttpHandlingException()
        {
        }

        protected HttpHandlingException(string message) : base(message)
        {
        }

        protected HttpHandlingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected HttpHandlingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public abstract int Status { get; }

        public abstract bool IsCancellingRequest { get; }

        public abstract void ProcessRequest(HttpContext context);

        public virtual bool IsReusable
        {
            get { return true; }
        }
    }
}