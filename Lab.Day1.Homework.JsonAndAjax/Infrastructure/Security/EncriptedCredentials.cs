using System;
using System.Security.Cryptography;

namespace Lab.Day1.Homework.JsonAndAjax.Infrastructure.Security
{
    /// <summary>
    /// This object created when user is registered
    /// It is also used for finding user in database at logon:
    /// for entered login\password creates EncriptedCredential and then it will be send to database as request to find
    /// </summary>
    public class EncriptedCredentials
    {
        private const int RfcIterations = 4096;
        private const int SaltLength = 16;
        private const int HashLength = 32;

        public string Salt { get; private set; }

        public string Hash { get; private set; }

        public EncriptedCredentials(string salt, string hash)
        {
            Salt = salt;
            Hash = hash;
        }

        public EncriptedCredentials(string password)
        {
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException();
            var salt = new byte[SaltLength];
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(salt);
            }
            Salt = Convert.ToBase64String(salt);
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, salt, RfcIterations);
            byte [] hashArray = key.GetBytes(HashLength);
            Hash = Convert.ToBase64String(hashArray);
        }

        public bool CheckCredentials(string password)
        {
            //if (password == null) throw new ArgumentNullException();
            //if (password.Length < 6) throw new ArgumentException("Password length must be greater than 5");
            if (string.IsNullOrEmpty(password)) return false;
            try
            {
                var salt = Convert.FromBase64String(Salt);
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, salt, RfcIterations);
                var actualHash = Convert.ToBase64String(key.GetBytes(HashLength));
                if (actualHash == Hash) return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
