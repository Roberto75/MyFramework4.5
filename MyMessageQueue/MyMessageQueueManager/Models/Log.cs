using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.MessageQueue.Models
{
    public class Log
    {
        public long id { get; set; }
        public DateTime dateAdded { get; set; }
        public long messageId { get; set; }
        public LogManager.Level level { get; set; }
        public string note { get; set; }

        public Log()
        {
        }

        public Log(System.Data.DataRow row)
        {
            id = long.Parse(row["Id"].ToString());
            dateAdded = (row["date_added"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["date_added"].ToString());
            messageId = (row["message_id"] is DBNull) ? -1 : long.Parse(row["message_id"].ToString());

            if (row["my_level"] is DBNull)
            {
                level = LogManager.Level.Undefined;
            }
            else
            {
                level = (LogManager.Level)Enum.Parse(typeof(LogManager.Level), row["my_level"].ToString());
            }

            note = (row["my_note"] is DBNull) ? "" : row["my_note"].ToString();
        }
    }
}
