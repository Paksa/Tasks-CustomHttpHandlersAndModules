using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Lab.Day1.Homework.JsonAndAjax.Model;

namespace Lab.Day1.Homework.JsonAndAjax.Data
{
    public class AuthenticationContext : DbContext
    {
        public AuthenticationContext()
            : base("AuthenticationContext")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<AuthenticationContext>());
        }
        public DbSet<User> Users { get; set; }
    }
}