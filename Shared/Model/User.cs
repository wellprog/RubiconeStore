using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace Shared.Model
{
    public class User : IValidate
    {
        public int ID { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Login { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Password { get; set; } = "";
        public DateTime CreateDate { get; set; } = DateTime.Now;

        /************************************************/
        // Relations
        [JsonIgnore]
        public List<UserSession> UserSessions { get; set; }
        [JsonIgnore]
        public List<Check> Checks { get; set; }

        public bool IsModelRight()
        {
            if (string.IsNullOrEmpty(Login)) return false;
            if (string.IsNullOrEmpty(Password)) return false;
            if (string.IsNullOrEmpty(Email)) return false;

            return true;
        }

        public bool IsPasswordRight(string password)
        {
            return Password == EncriptPassword(password);
        }

        public void PreparePassword()
        {
            Password = EncriptPassword(Password);
        }

        private string EncriptPassword(string password)
        {
            using (var hasher = MD5.Create())
            {
                var hash = hasher.ComputeHash(Encoding.Unicode.GetBytes(password));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < hash.Length; i++)
                {
                    sBuilder.Append(hash[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            } 
        }
    }
}
