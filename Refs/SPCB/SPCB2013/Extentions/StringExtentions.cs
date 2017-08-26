using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;

namespace SPBrowser.Extentions
{
    public static class StringExtentions
    {
        /// <summary>
        /// Returns the <see cref="SecureString"/> for use of passwords.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static SecureString GetSecureString(this string password)
        {
            SecureString securePassWord = new SecureString();
            foreach (char c in password.ToCharArray()) securePassWord.AppendChar(c);

            return securePassWord;
        }

        /// <summary>
        /// Strips the quotes at the begin and end of the string.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Returns stripped string, without quotes at start and end.</returns>
        public static string StripQuotes(this string s)
        {
            if (s.EndsWith("\"") && s.StartsWith("\""))
            {
                return s.Substring(1, s.Length - 2);
            }
            else
            {
                return s;
            }
        }
    }
}
