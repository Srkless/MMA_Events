using System;
using System.Security.Cryptography;
using System.Text;

namespace MMA_Events.Services
{
    public class ValidationService
    {
        public bool ValidatePassword(string password, string confirmPassword)
        {
            return password == confirmPassword && password.Length >= 8; ;
        }

        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public bool validateEmail(string email)
        {
            return email.Contains("@");
        }
    }
}