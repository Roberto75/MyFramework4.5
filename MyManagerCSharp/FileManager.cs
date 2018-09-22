using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace MyManagerCSharp
{
    public class FileManager
    {


        public static byte[] getBytes(string filePath)
        {
            FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(stream);

            byte[] data = reader.ReadBytes((int)stream.Length);

            reader.Close();
            stream.Close();

            return data;
        }

        public static Encoding getEncoding(FileInfo fi)
        {
            // Read the BOM
            var bom = new byte[4];
            using (var file = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read))
            {
                file.Read(bom, 0, 4);
            }

            Debug.WriteLine("BOM:" + bom[0] + " " + bom[1] + " " + bom[2] + " " + bom[3]);

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;
            return Encoding.ASCII;
        }


        /*
        public static bool isEncodingUtf8(FileInfo fi)
        {

            
            Encoding encoding = Encoding.Default;
            String original = String.Empty;

            using (StreamReader sr = new StreamReader(fi.FullName, Encoding.Default))
            {
                original = sr.ReadToEnd();
                encoding = sr.CurrentEncoding;
                sr.Close();
            }

            if (encoding == Encoding.UTF8)
            {
                Debug.WriteLine(fi.Name + " isEncodingUtf8 true");
                return true;
            }

            Debug.WriteLine(fi.Name + " isEncodingUtf8 false " + encoding.BodyName);
            return false;
        }
        */

        public static String readFileAsUtf8(string fileName)
        {
            Encoding encoding = Encoding.Default;
            String original = String.Empty;

            using (StreamReader sr = new StreamReader(fileName, Encoding.Default))
            {
                original = sr.ReadToEnd();
                encoding = sr.CurrentEncoding;
                sr.Close();
            }

            if (encoding == Encoding.UTF8)
                return original;

            byte[] encBytes = encoding.GetBytes(original);
            byte[] utf8Bytes = Encoding.Convert(encoding, Encoding.UTF8, encBytes);
            return Encoding.UTF8.GetString(utf8Bytes);
        }



        public static String readTextFile(FileInfo fileName)
        {
            return System.IO.File.ReadAllText(fileName.FullName, Encoding.UTF8);
        }


        public static void writeTextFile(FileInfo fileName, string content)
        {
            System.IO.File.WriteAllText(fileName.FullName, content, Encoding.UTF8);
        }

    }
}
