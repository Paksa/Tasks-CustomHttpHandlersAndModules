using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab.Day1.Homework.JsonAndAjax.Infrastructure.HttpHelpers
{
    public static class HttpContextHelper
    {

        public static void RemoveCookie(this HttpContext context, string cookieName)
        {
            var cookie = context.Request.Cookies[cookieName];
            if (cookie == null) return;
            cookie.Value = null;
            cookie.Expires = DateTime.Now.AddYears(-1);
            context.Response.Cookies.Remove(cookieName);
            context.Response.SetCookie(cookie);
        }

        public static HttpCookie CreateNewCookie(string cookieName, string cookieValue)
        {
            var cookie = new HttpCookie(cookieName);
            cookie.Value = cookieValue;
            var expirationDate = DateTime.MaxValue;
            cookie.Expires = expirationDate;
            return cookie;
        }

        public static void SendJsonResponseOkAndEndRequest(this HttpContext context)
        {
            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json";
            context.Response.Write(@"{""Result"":""Ok""}");
            context.Response.End();
        }
    }
}