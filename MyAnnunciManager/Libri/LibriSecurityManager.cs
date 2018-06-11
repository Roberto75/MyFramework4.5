using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annunci.Libri
{
   public class LibriSecurityManager
    {
        const string _key = "R0b3rt055wkijhttopokrpty89661xYz";
        const string _IV = "173hdteiP3i48705";

        public static string Key
        {
            get
            {
                return _key;
            }
        }

        public static string IV
        {
            get
            {
                return _IV;
            }
        }



        public static string Encrypt(string plainText)
        {
            byte[] encrypted = MyManagerCSharp.SecurityManager.AESEncryptFromString(plainText, System.Text.UTF8Encoding.UTF8.GetBytes(_key), System.Text.UTF8Encoding.UTF8.GetBytes(_IV));

            string encryptedBase64;
            encryptedBase64 = Convert.ToBase64String(encrypted);

            return encryptedBase64;
        }

        public static string Decrypt(string cipherTextBase64)
        {
            return MyManagerCSharp.SecurityManager.AESDecryptSFromBase64String(cipherTextBase64, System.Text.UTF8Encoding.UTF8.GetBytes(_key), System.Text.UTF8Encoding.UTF8.GetBytes(_IV));
        }
    }
}

