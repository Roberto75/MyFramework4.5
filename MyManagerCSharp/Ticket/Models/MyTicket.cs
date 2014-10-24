using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp.Ticket.Models
{
    public class MyTicket
    {
        public long id { get; set; }
        public DateTime dateAdded { get; set; }
        public DateTime dateLastModified { get; set; }
        public TicketManager.TicketStatus status { get; set; }
        public long ownerId { get; set; }
        public string titolo { get; set; }
        public string targetId { get; set; }

        public string referenceTypeId { get; set; }
        public long? referenceId { get; set; }

        public string referenceSource { get; set; }
        public long? referenceSourceId { get; set; }

        public List<MyTicketPost> Posts { get; set; }
        public List<MyTicketAttachment> Attachments { get; set; }

        public virtual MyManagerCSharp.Models.MyUserSmall Owner { get; set; }

        public MyTicket()
        {
        }

        public MyTicket(System.Data.DataRow row)
        {
            id = long.Parse(row["Id"].ToString());
            dateAdded = (row["date_added"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["date_added"].ToString());
            dateLastModified = (row["DATE_LAST_MODIFIED"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["DATE_LAST_MODIFIED"].ToString());
            ownerId = long.Parse(row["owner_id"].ToString());
            titolo = (row["titolo"] is DBNull) ? "" : row["titolo"].ToString();
            targetId = (row["target_id"] is DBNull) ? "" : row["target_id"].ToString();

            referenceTypeId = (row["reference_type_id"] is DBNull) ? "" : row["reference_type_id"].ToString();
            if (row["reference_id"] is DBNull)
            {
                referenceId = null;
            }
            else
            {
                referenceId = long.Parse(row["reference_id"].ToString());
            }

            if (row["reference_source_id"] is DBNull)
            {
                referenceSourceId = null;
            }
            else
            {
                referenceSourceId = long.Parse(row["reference_source_id"].ToString());
            }


            referenceSource = (row["reference_source"] is DBNull) ? "" : row["reference_source"].ToString();


            status = (TicketManager.TicketStatus)Enum.Parse(typeof(TicketManager.TicketStatus), row["TICKET_STATUS_ID"].ToString());
            



        }
    }
}
