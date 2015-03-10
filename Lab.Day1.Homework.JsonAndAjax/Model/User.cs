using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Lab.Day1.Homework.JsonAndAjax.Infrastructure.Security;

namespace Lab.Day1.Homework.JsonAndAjax.Model
{
    public class User : IPrincipal, IIdentity
    {
        /// <summary>
        /// Needed for entity framework
        /// </summary>
        private User() { }

        public User(string name, string password)
        {
            Name = name;
            var encriptedCredentials = new EncriptedCredentials(password);
            Salt = encriptedCredentials.Salt;
            Hash = encriptedCredentials.Hash;
            IsAuthenticated = false;
            AuthenticationTokenExpiration = null;
            AuthenticationToken = null;
        }
        public User(string name, string password, Guid token, DateTime cookieExpiration) :this(name, password)
        {
            
            AuthenticationTokenExpiration = cookieExpiration;
            AuthenticationToken = token;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; private set; }
        public virtual string Name { get; private set; }
        public virtual string Salt { get; private set; }
        public virtual string Hash { get; private set; }
        public virtual Guid? AuthenticationToken { get; private set; }
        public virtual DateTime? AuthenticationTokenExpiration { get; private set; }
        public virtual bool IsAuthenticated { get; private set; }
        public virtual bool IsInRole(string role)
        {
            return false;
        }

        public virtual IIdentity Identity
        {
            get { return this; }
        }

        public virtual string AuthenticationType
        {
            get { return "CustomAuthentication"; }
        }

        public bool IsCorrectPassword(string password)
        {
            var encriptedCredentials = new EncriptedCredentials(this.Salt, this.Hash);
            return encriptedCredentials.CheckCredentials(password);
        }

        public void SetAuthenticationToken(Guid token, DateTime expirationTime)
        {
            this.AuthenticationToken = token;
            this.AuthenticationTokenExpiration = expirationTime;
        }

        public void ChangeAuthenticationFlag(bool value)
        {
            IsAuthenticated = value;
        }
    }
}