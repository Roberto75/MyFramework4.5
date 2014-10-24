using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp.Ticket.Models
{
    public class MyTicketPost
    {
        public long id { get; set; }
        public long ticketId { get; set; }

        public DateTime dateAdded { get; set; }
        public bool isFirstPost { get; set; }
        public long userId { get; set; }
        public string note { get; set; }


        public virtual MyManagerCSharp.Models.MyUserSmall Owner { get; set; }

        public MyTicketPost()
        {
        }

        public MyTicketPost(System.Data.DataRow row)
        {
            id = long.Parse(row["Id"].ToString());
            dateAdded = (row["date_added"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["date_added"].ToString());
            isFirstPost = bool.Parse(row["isFirstPost"].ToString());
            //referenceId = (row["reference_id"] is DBNull) ? "" : row["reference_id"].ToString();
            //referenceType = (row["reference_type"] is DBNull) ? "" : row["reference_type"].ToString();
            //level = (row["my_level"] is DBNull) ? "" : row["my_level"].ToString();
            ticketId = long.Parse(row["ticket_id"].ToString());
            userId = long.Parse(row["user_id"].ToString());
            note = (row["note"] is DBNull) ? "" : row["note"].ToString();
            //type = (row["my_type"] is DBNull) ? "" : row["my_type"].ToString();
        }
    }
}
