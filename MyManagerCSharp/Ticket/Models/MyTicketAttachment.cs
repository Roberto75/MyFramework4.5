using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp.Ticket.Models
{
    public class MyTicketAttachment
    {
        public long id { get; set; }
        public long ticketId { get; set; }

        public DateTime dateAdded { get; set; }
        public long userId { get; set; }
        public string note { get; set; }

        public string fileName { get; set; }

        public virtual MyManagerCSharp.Models.MyUserSmall Owner { get; set; }

        public string getPath(string absoluteServerPath)
        {
            string path;
            path = String.Format("{0}{1}\\{2}", absoluteServerPath, ticketId, fileName);

            return path;
        }

        public MyTicketAttachment()
        {
        }

        public MyTicketAttachment(System.Data.DataRow row)
        {
            id = long.Parse(row["id"].ToString());
            dateAdded = (row["date_added"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["date_added"].ToString());

            ticketId = long.Parse(row["ticket_id"].ToString());
            userId = long.Parse(row["user_id"].ToString());
            note = (row["note"] is DBNull) ? "" : row["note"].ToString();
            fileName = (row["file_name"] is DBNull) ? "" : row["file_name"].ToString();
        }
    }
}
