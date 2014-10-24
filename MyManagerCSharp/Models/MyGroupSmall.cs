using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp.Models
{
    public class MyGroupSmall
    {
        public long gruppoId { get; set; }
        public string nome { get; set; }
        public string tipo { get; set; }


        public MyGroupSmall(System.Data.DataRow row)
        {
            gruppoId = long.Parse(row["gruppo_id"].ToString());
            nome = row["nome"].ToString();
            tipo = (row["tipo_id"] is DBNull) ? "" : row["tipo_id"].ToString();
        }
    }
}
