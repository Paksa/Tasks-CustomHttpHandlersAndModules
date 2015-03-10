using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab.Day1.Homework.JsonAndAjax.Infrastructure.HttpHelpers.HttpErrors
{
    public static class HttpHandlingExceptionHandler
    {
        public static bool Handle<T>(HttpContext context, T httpException) where T : IHttpResult, IHttpHandler
        {
            httpException.ProcessRequest(context);
            if (httpException.IsCancellingRequest)
            {
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                return true;
            }
            return false;
        }
    }
}