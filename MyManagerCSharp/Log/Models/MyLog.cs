using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp.Log.Models
{
    public class MyLog
    {
        public long id { get; set; }
        public DateTime dateAdded { get; set; }
        public string referenceId { get; set; }
        public string referenceType { get; set; }
        public Log.LogManager.Level level { get; set; }
        public string note { get; set; }
        public string type { get; set; }

        public MyLog()
        {
        }

        public MyLog(System.Data.DataRow row)
        {
            id = long.Parse(row["Id"].ToString());
            dateAdded = (row["date_added"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["date_added"].ToString());
            referenceId = (row["reference_id"] is DBNull) ? "" : row["reference_id"].ToString();
            referenceType = (row["reference_type"] is DBNull) ? "" : row["reference_type"].ToString();


            if (row["my_level"] is DBNull)
            {
                level = LogManager.Level.Undefined;
            }
            else
            {
                level = (LogManager.Level)Enum.Parse(typeof(LogManager.Level), row["my_level"].ToString());
            }

            note = (row["my_note"] is DBNull) ? "" : row["my_note"].ToString();
            type = (row["my_type"] is DBNull) ? "" : row["my_type"].ToString();
        }

    }



}
