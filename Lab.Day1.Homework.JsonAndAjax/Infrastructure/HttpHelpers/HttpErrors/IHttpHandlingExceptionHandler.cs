using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Lab.Day1.Homework.JsonAndAjax.Infrastructure.HttpHelpers.HttpErrors
{
    public interface IHttpHandlingExceptionHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpException"></param>
        /// <returns>If true - than calling method must exit (HttpContext exits)</returns>
        bool Handle<T>(HttpContext context, T httpException) where T : IHttpResult, IHttpHandler;
    }
}
