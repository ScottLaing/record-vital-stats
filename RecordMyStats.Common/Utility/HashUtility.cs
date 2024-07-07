using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RecordMyStats.Common.Utility
{
    public class HashUtility
    {
        public static string HashString(string text, string salt = "")
        {
            if (String.IsNullOrEmpty(text))
            {
                return String.Empty;
            }

#pragma warning disable SYSLIB0021

            // Uses SHA256 to create the hash
            using (var sha = new System.Security.Cryptography.SHA256Managed())

#pragma warning restore SYSLIB0021

            {
                // Convert the string to a byte array first, to be processed
                byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(text + salt);
                byte[] hashBytes = sha.ComputeHash(textBytes);

                // Convert back to a string, removing the '-' that BitConverter adds
                string hash = BitConverter
                    .ToString(hashBytes)
                    .Replace("-", String.Empty);

                return hash;
            }
        }

        public string CreateSHA512(string strData)
        {
            var message = Encoding.UTF8.GetBytes(strData);
            using (var alg = SHA512.Create())
            {
                string hex = "";

                var hashValue = alg.ComputeHash(message);
                foreach (byte x in hashValue)
                {
                    hex += String.Format("{0:x2}", x);
                }
                return hex;
            }
        }

        public static string GetUniqueSessionKey()
        {
            var guid = Guid.NewGuid();
            var sessionKey = guid.ToString().Replace("-", "").Replace("{", "").Replace("}", "").ToUpper();
            sessionKey = sessionKey.Substring(0, 20);
            return sessionKey;
        }
    }
}
