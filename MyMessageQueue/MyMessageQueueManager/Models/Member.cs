using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.MessageQueue.Models
{
    public class Member
    {
        public long id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public DateTime dateAdded { get; set; }

        public Member(string nome, string mail)
        {
            name = nome.Trim();
            email = mail.Trim(); 
        }


        public Member(System.Data.DataRow row)
        {
            id = long.Parse(row["id"].ToString());
            dateAdded = (row["date_added"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["date_added"].ToString());

            name = row["name"].ToString();
            email = row["email"].ToString();
        }

    }
}
