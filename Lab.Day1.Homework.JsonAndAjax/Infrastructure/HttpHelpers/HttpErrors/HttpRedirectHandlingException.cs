using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab.Day1.Homework.JsonAndAjax.Infrastructure.HttpHelpers.HttpErrors
{
    public class HttpRedirectHandlingException: HttpHandlingException
    {
        private readonly string _redirectUrl;
        public HttpRedirectHandlingException(string url)
        {
            _redirectUrl = url;
        }

        public override int Status
        {
            get { return 301; }
        }

        public override bool IsCancellingRequest
        {
            get { return true; }
        }

        public override void ProcessRequest(HttpContext context)
        {
            context.Response.StatusCode = 301;
            context.Response.RedirectLocation = _redirectUrl;
            context.Response.StatusDescription = "Redirecting to login page";
        }
    }
}