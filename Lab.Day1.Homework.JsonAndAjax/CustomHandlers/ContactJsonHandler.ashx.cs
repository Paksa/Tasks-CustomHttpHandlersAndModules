using System.Web;
using Lab.Day1.Homework.JsonAndAjax.Model;

namespace Lab.Day1.Homework.JsonAndAjax.CustomHandlers
{
    /// <summary>
    /// Summary description for ContactJsonHandler
    /// </summary>
    public class ContactJsonHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var contacts = ContactManager.GetContacts();
            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var jsonContactsString = javaScriptSerializer.Serialize(contacts);
            context.Response.ContentType = "application/json";
            context.Response.Write(jsonContactsString);
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}