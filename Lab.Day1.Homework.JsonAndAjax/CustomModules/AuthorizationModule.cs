using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.EnterpriseServices;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using Lab.Day1.Homework.JsonAndAjax.Data;
using Lab.Day1.Homework.JsonAndAjax.Infrastructure.HttpHelpers;
using Lab.Day1.Homework.JsonAndAjax.Infrastructure.HttpHelpers.HttpErrors;
using Lab.Day1.Homework.JsonAndAjax.Model;
using Lab.Day1.Homework.JsonAndAjax.Model.ViewModels;

namespace Lab.Day1.Homework.JsonAndAjax.CustomModules
{
    /// <summary>
    /// This module used for custom authentication in ASP.NET (for IIS)
    /// Used custom HttpHandlingExeption system for simpler error handling
    /// Implemented:
    /// -simple unauthenticated request filtering system
    /// -manage login/register/logout ajax requests
    /// -custom authentication using cookies
    /// </summary>
    public class AuthorizationModule : IHttpModule
    {
        #region Private Fields

        private const string AuthenticationCookieTokenName = "authentication-token";
        private const string LoginPage = "";
        private static readonly List<string> PagesAllowedWithoutLogin = new List<string>();

        #endregion

        #region Constructors
        /// <summary>
        /// Static constants initialization (needed for unauthentication access for main page (bypass routes)
        /// </summary>
        static AuthorizationModule()
        {
            //Enabled without autorization:
            PagesAllowedWithoutLogin.Add("~/");
            PagesAllowedWithoutLogin.Add("~/index.html");
            PagesAllowedWithoutLogin.Add("~/Scripts/LoginHelper.js");
            PagesAllowedWithoutLogin.Add("~/Scripts/tabmenu_js.js");
            PagesAllowedWithoutLogin.Add("~/Content/tabmenu_page_style.css");
            PagesAllowedWithoutLogin.Add("~/Content/tabmenu_js.css");
            PagesAllowedWithoutLogin.Add("~/image.jpg");
            PagesAllowedWithoutLogin.Add("~/Scripts/ContactLoader.js");
        }
        #endregion

        #region Inner Authentication Stages

        /// <summary>
        /// If correct user credentials are presented in InputString in Request
        /// then check user for existance => check for correct password => set authenctication cookie
        /// </summary>
        /// <param name="context">HttpContext instance</param>
        /// <param name="credentialsViewModel">User credentials</param>
        private void CheckCredentialsAndAuthenticate(HttpContext context, CredentialsViewModel credentialsViewModel)
        {
            if (credentialsViewModel == null) throw new HttpNotAuthenticatedHandlingException("Error. Empty or incorrect login data");
            credentialsViewModel.CheckCredentials();
            using (var repository = new Repository<User>())
            {
                var user = repository.GetAll(x => x.Name == credentialsViewModel.Username).FirstOrDefault();
                if (user == null)
                {
                    throw new HttpNotAuthenticatedHandlingException("No such user");
                }
                if (!user.IsCorrectPassword(credentialsViewModel.Password)) throw new HttpNotAuthenticatedHandlingException("Incorrect password");
                //Add authorization cookies to response:
                var token = Guid.NewGuid();
                HttpCookie cookie = HttpContextHelper.CreateNewCookie(AuthenticationCookieTokenName, token.ToString());
                context.Response.Cookies.Add(cookie);
                user.SetAuthenticationToken(token, cookie.Expires);
                repository.Update(user);
                //Associate user with the request:
                context.User = user;
            }
        }


        /// <summary>
        /// Create and authenticate user (password must be provided in InputStream by XmlHttpRequest
        /// Correct JSON format: { "Username" : "...", "Password" : "...", "RequestType": "..."}
        /// </summary>
        /// <param name="context">HttpContext instance</param>
        /// <param name="credentialsViewModel">User credentials</param>
        private void CreateAndAuthenticateUser(HttpContext context, CredentialsViewModel credentialsViewModel)
        {
            if (credentialsViewModel == null) throw new HttpNotAuthenticatedHandlingException("Error. Empty or incorrect login data");
            
            using (var repository = new Repository<User>())
            {
                var user = repository.GetAll(x => x.Name == credentialsViewModel.Username).FirstOrDefault();
                if (user != null)
                {
                    throw new HttpNotAuthenticatedHandlingException("User with such name already exist.");
                }
                user = new User(credentialsViewModel.Username, credentialsViewModel.Password);
                repository.Add(user);
                //Add authorization cookies to response:
                var token = Guid.NewGuid();
                HttpCookie cookie = HttpContextHelper.CreateNewCookie(AuthenticationCookieTokenName, token.ToString());
                cookie.Value = token.ToString();
                context.Response.Cookies.Add(cookie);
                user.SetAuthenticationToken(token, cookie.Expires);
                repository.Update(user);
                //Associate user with the request:
                context.User = user;
            }
            
        }

        /// <summary>
        /// Check cookie (if presented) for correctness
        /// </summary>
        /// <param name="context">HttpContext instance</param>
        void CheckAuthenticationCookie(HttpContext context)
        {
            
            var cookie = context.Request.Cookies[AuthenticationCookieTokenName];
            if (cookie == null) throw new InvalidOperationException("Cookie cannot be empty");
            if (context.Request.InputStream.Length > 0)
            {
                var credentials = this.GetCredentialsViewModel(context);
                if (credentials.RequestType == "Logout")
                {
                    context.RemoveCookie(AuthenticationCookieTokenName);
                    context.SendJsonResponseOkAndEndRequest();
                }
            }
            User user = null;
            using (var repository = new Repository<User>())
            {
                user = repository.GetAll(x => x.AuthenticationToken.ToString() == cookie.Value).FirstOrDefault();
                
            }
            if (user == null || cookie.Expires >= user.AuthenticationTokenExpiration)
            {
                context.RemoveCookie(AuthenticationCookieTokenName);
                return;
            }
            user.ChangeAuthenticationFlag(true);
            context.User = user;
        }

        /// <summary>
        /// Simple routing filter for unathenticated requests
        /// </summary>
        /// <param name="context">HttpContext instance</param>
        private void CheckUnauthenticatedRoute(HttpContext context)
        {
            if (!PagesAllowedWithoutLogin.Any(x => x.Equals(context.Request.AppRelativeCurrentExecutionFilePath)))
            throw new HttpNotAuthenticatedHandlingException();
        }

        /// <summary>
        /// Extract credentials from HttpContext.Request.InputStream
        /// </summary>
        /// <param name="context">HttpContext instance</param>
        /// <returns>Credentials</returns>
        private CredentialsViewModel GetCredentialsViewModel(HttpContext context)
        {
            if (context.Request.InputStream.Length <= 0) throw new HttpRedirectHandlingException(LoginPage);
            CredentialsViewModel credentialsViewModel;
            using (var inputStream = new StreamReader(context.Request.InputStream))
            {
                try
                {
                    var jsonString = inputStream.ReadToEnd();
                    var jsSerializer = new JavaScriptSerializer();
                    credentialsViewModel = jsSerializer.Deserialize<CredentialsViewModel>(jsonString);
                }
                catch (Exception ex)
                {
                    credentialsViewModel = null;
                }
            }
            return credentialsViewModel;
        }
        
        #endregion

        #region IHttpModule Members

        public void Dispose()
        {
            //clean-up code here.
        }

        /// <summary>
        /// Subsribe to HttpApplication events here (called automatically by asp.net)
        /// </summary>
        /// <param name="context">HttpApplication instance</param>
        public void Init(HttpApplication context)
        {
            //Subscribe on BeginRequest to filter requests
            context.BeginRequest += new EventHandler(this.AuthenticateUser);
        }

        #endregion

        #region Main Event Processing Callbacks

        /// <summary>
        /// This method called upon BeginRequestEvent. Performs basic athentication for pages
        /// </summary>
        /// <param name="source">Source of event (must be HttpApplication)</param>
        /// <param name="e">Event argumets</param>
        public void AuthenticateUser(Object source, EventArgs e)
        {
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;
            var authenticationCookie = context.Request.Cookies[AuthenticationCookieTokenName];

            try
            {
                if (authenticationCookie == null)
                {
                    this.CheckUnauthenticatedRoute(context);
                    if (context.Request.ContentLength <= 0) return;
                    //needed for correct request handling (to not respone for other than .html or /):
                    var pathExtension = context.Request.CurrentExecutionFilePathExtension;
                    if (pathExtension == ".html" || pathExtension == "")
                    {
                        var credentials = this.GetCredentialsViewModel(context);
                        string requestType = null;
                        if (credentials != null) requestType = credentials.RequestType;
                        if (requestType != null && requestType == "Register")
                        {
                            this.CreateAndAuthenticateUser(context, credentials);
                        }
                        else
                        {
                            //request type must be "Login"
                            this.CheckCredentialsAndAuthenticate(context, credentials);
                        }
                        context.Response.End();
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
                else
                {
                    this.CheckAuthenticationCookie(context);
                }
            }
            catch (HttpHandlingException ex)
            {
                HttpHandlingExceptionHandler.Handle(context, ex);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion
    }
}
