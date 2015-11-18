using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.MessageQueue.Models
{
    public class Email : MessageBase
    {
        public string Subject { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Body { get; set; }

        public List<Attachment> Attachments { get; set; }

        public Email()
        {
            Attachments = new List<Attachment>();
        }


        public Email(System.Data.DataRow row)
        {
            id = long.Parse(row["id"].ToString());

            Subject = row["Subject"].ToString();
            From = row["From"].ToString();
            To = row["To"].ToString();
            Cc = row["Cc"].ToString();
            Bcc = row["Bcc"].ToString();
            Body = row["Body"].ToString();

            if (row["distribution_list_id"] is DBNull)
            {
                distributionListId = null;
            }
            else
            {
                distributionListId = long.Parse(row["distribution_list_id"].ToString());
            }

            Attachments = new List<Attachment>();
        }
    }
}
