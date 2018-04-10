using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace DropBoxWebhookAPI
{
    /// <summary>
    /// Class for Containing all Methods for Crypto functions
    /// </summary>
    public class Crypt
    {
        /// <summary>
        /// **** This Method is used to compute the hash value of an input string.
        /// </summary>
        /// <param name="sha256Hash"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetSha256Hash(HMACSHA256 sha256Hash, string input)
        {
            //*** Convert the input string to a byte array and compute the hash. 
            byte[] data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            //*** Create a new Stringbuilder to collect the bytes and create a string.
            var stringBuilder = new StringBuilder();

            //*** Loop through each byte of the hashed data
            foreach (byte t in data)
            {
                stringBuilder.Append(t.ToString("x2")); //*** format each one as a hexadecimal string. 
            }

            //*** Return the hexadecimal string. 
            return stringBuilder.ToString();
        }

        /// <summary>
        /// **** This Method is used to computes its hash and then compares that hash with the hashed signature I received from Dropbox to ensure that they match up
        /// </summary>
        /// <param name="sha256Hash"></param>
        /// <param name="input"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static bool VerifySha256Hash(HMACSHA256 sha256Hash, string input, string hash)
        {
            // Hash the input. 
            string hashOfInput = GetSha256Hash(sha256Hash, input);

            if (String.Compare(hashOfInput, hash, StringComparison.OrdinalIgnoreCase) == 0)
                return true;

            return false;
        }
    }
}