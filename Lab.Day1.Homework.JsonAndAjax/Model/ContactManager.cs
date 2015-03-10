using System;
using System.Collections.Generic;

namespace Lab.Day1.Homework.JsonAndAjax.Model
{
    public static class ContactManager
    {
        private static readonly List<Contact> Contacts;
        static ContactManager()
        {
            Contacts = new List<Contact>();
            const int contactCount = 5;
            var rnd = new Random();
            for (int i = 0; i <= contactCount; ++i)
            {
                var contact = new Contact()
                {
                    Name = string.Format("Contact{0}", i),
                    Age = rnd.Next(0, 118)
                };
                Contacts.Add(contact);
            }
        }
        public static List<Contact> GetContacts()
        {
            return Contacts;
        }
    }
}