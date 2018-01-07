using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MyUsers.Models
{
    public class MyRole
    {
        [Key]
        public string ruoloId { get; set; }
        public string nome { get; set; }

        public long gruppoId { get; set; }
        public string gruppo { get; set; }

        public MyRole(string id)
        {
            ruoloId = id;
        }


        public MyRole(System.Data.DataRow row)
        {
            ruoloId = row["ruolo_id"].ToString();
            nome = row["nome"].ToString();

            if (row.Table.Columns.Contains("gruppo"))
            {
                if (!(row["gruppo_id"] is DBNull))
                {
                    gruppoId = long.Parse(row["gruppo_id"].ToString());
                }

                gruppo = row["gruppo"].ToString();
            }

        }
    }

}
