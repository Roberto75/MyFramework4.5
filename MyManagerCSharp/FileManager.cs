using System;
using System.Collections.Generic;
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
    }
}
