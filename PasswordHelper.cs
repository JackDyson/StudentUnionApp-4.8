using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace StudentUnionApp
{
    public class PasswordHelper
    {
        // password hashing function for staff
        public static string HashPassword(string password)
        {
            // generate a random salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            // derive a 256-bit subkey (use HMACSHA1 with 10000 iterations)
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            // create a 36 byte array to store the salt and subkey
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            // copy the salt and subkey into the hashBytes array
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            // return the hashed password
            return Convert.ToBase64String(hashBytes);
        }

        // password verification function for staff
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // extract the bytes
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);
            // get the salt
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            // compute the hash on the password the user entered
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            // compare the results
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }        
    }
}