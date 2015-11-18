using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Security.Cryptography;
using System.IO;
using System.Security;
using System.Runtime.InteropServices;

namespace MyManagerCSharp
{
    public class SecurityManager
    {


        public static SecureString getSecureString(string Source)
        {
            if (String.IsNullOrWhiteSpace(Source))
            {
                return null;
            }

            SecureString result = new SecureString();

            foreach (char c in Source.ToCharArray())
            {
                result.AppendChar(c);
            }

            return result;
        }



        public static string getStringFromSecureString(SecureString ss)
        {
            string password;

            IntPtr bstr = Marshal.SecureStringToBSTR(ss);

            try
            {
                password = Marshal.PtrToStringUni(bstr);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(bstr);
            }

            return password;
        }



        public static string getMD5Hash(FileInfo fi)
        {

            byte[] hash;

            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(fi.FullName))
                {
                    hash = md5.ComputeHash(stream);
                }
            }

            string temp;
            temp = BitConverter.ToString(hash).Replace("-", "").ToUpper();

            return temp;
        }




        public static string getMD5Hash(string input)
        {
            // Create a new instance of the MD5 object.
            System.Security.Cryptography.MD5 md5Hasher = System.Security.Cryptography.MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            Byte[] mdata = md5Hasher.ComputeHash(System.Text.Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes and create a string.
            System.Text.StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data and format each one as a hexadecimal string.
            //int i;

            for (int i = 0; i < mdata.Length; i++)
            {
                sBuilder.Append(mdata[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString().ToUpper();

        }


        //stringa di lunghezza 16 = > 128
        //stringa di lunghezza 32 = > 256

        //Legal min key size = 128 
        //Legal max key size = 256 
        //Legal min block size = 128 
        //Legal max block size = 256 

        //Essentially, if you want to use RijndaelManaged as AES you need to make sure that:
        //The block size is set to 128 bits
        //You are not using CFB mode, or if you are the feedback size is also 128 bits


        #region ""AES Decrypt "


        public static string AESDecryptFromBytes(byte[] cipherText, string Key, string IV)
        {
            return SecurityManager.AESDecryptFromBytes(cipherText, System.Text.UTF8Encoding.UTF8.GetBytes(Key), System.Text.UTF8Encoding.UTF8.GetBytes(IV));
        }

        public static string AESDecryptFromBase64String(string cipherTextBase64, string Key, string IV)
        {
            return SecurityManager.AESDecryptFromBytes(Convert.FromBase64String(cipherTextBase64), System.Text.UTF8Encoding.UTF8.GetBytes(Key), System.Text.UTF8Encoding.UTF8.GetBytes(IV));
        }

        public static string AESDecryptSFromBase64String(string cipherTextBase64, byte[] Key, byte[] IV)
        {
            return SecurityManager.AESDecryptFromBytes(Convert.FromBase64String(cipherTextBase64), Key, IV);
        }

        public static string AESDecryptFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create an AesManaged object 
            // with the specified key and IV. 
            using (RijndaelManaged aesAlg = new RijndaelManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption. 
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream 
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }

        #endregion


        #region ""AES Encrypt "

        public static byte[] AESEncryptFromString(string plainText, string Key, string IV)
        {
            return SecurityManager.AESEncryptFromString(plainText, System.Text.UTF8Encoding.UTF8.GetBytes(Key), System.Text.UTF8Encoding.UTF8.GetBytes(IV));
        }

        public static byte[] AESEncryptFromString(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;
            // Create an AesManaged object 
            // with the specified key and IV. 
            using (RijndaelManaged aesAlg = new RijndaelManaged())
            {

                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption. 
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream. 
            return encrypted;

        }


        #endregion

    }
}
