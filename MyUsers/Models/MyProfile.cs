using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MyUsers.Models
{
    public class MyProfile
    {
        [Key]
        public string profiloId { get; set; }
        public string nome { get; set; }
        public DateTime dateAdded { get; set; }
        //public virtual ICollection<MyUser> Utenti { get; set; }

        public MyProfile()
        {

        }

        public MyProfile(System.Data.DataRow row)
        {
            profiloId = row["profilo_id"].ToString();
            nome = row["nome"].ToString();
            dateAdded = (row["date_added"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["date_added"].ToString()); 
        }
    }
}
