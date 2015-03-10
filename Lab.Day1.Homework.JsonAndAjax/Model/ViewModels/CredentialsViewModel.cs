using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Lab.Day1.Homework.JsonAndAjax.Infrastructure.HttpHelpers.HttpErrors;

namespace Lab.Day1.Homework.JsonAndAjax.Model.ViewModels
{
    public class CredentialsViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string RequestType { get; set; }

        /// <summary>
        /// Generate HttpHandlingException if credentials are not correct
        /// </summary>
        public void CheckCredentials()
        {
            if (!this.IsCorrectUsername(Username)) throw new HttpNotAuthenticatedHandlingException("Incorrect username");
            if (!this.IsCorrectPassword(Password)) throw new HttpNotAuthenticatedHandlingException("Incorrect password");
        }

        private bool IsCorrectUsername(string username)
        {
            var regex = new Regex(@"^[a-zA-Z0-9\.-]{4,}$");
            var matchers = regex.Matches(username);
            if (matchers.Count != 1) return false;
            return true;
        }

        private bool IsCorrectPassword(string password)
        {
            var regex = new Regex(@"^[a-zA-Z0-9\.-]{4,}$");
            var matchers = regex.Matches(password);
            if (matchers.Count != 1) return false;
            return true;
        }
    }
}