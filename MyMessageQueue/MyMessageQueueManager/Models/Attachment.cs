using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.MessageQueue.Models
{
    public class Attachment
    {
        public long id { get; set; }
        public string name { get; set; }
        public DateTime dateAdded { get; set; }
        public byte[] fileStream { get; set; }


        public Attachment() { }


        public Attachment(string filePath)
        {
            FileInfo info = new FileInfo(filePath);

            name = info.Name;
            fileStream = MyManagerCSharp.FileManager.getBytes(filePath);
        }


        public Attachment(System.Data.DataRow row)
        {
            id = long.Parse(row["id"].ToString());
            name = row["name"].ToString();
            dateAdded = (row["date_added"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["date_added"].ToString());

            if (row["stream"] is DBNull)
            {
                fileStream = null;
            }
            else
            {
                fileStream = (row["stream"] as byte[]);
            }


        }

    }
}
