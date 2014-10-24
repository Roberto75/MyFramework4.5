using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp.Alert.Models
{
    public class MyAlert
    {
        public long id { get; set; }
        public DateTime dateAdded { get; set; }
        public string nome { get; set; }
        public string descrizione { get; set; }
        public bool isEnabled { get; set; }

        public MyAlert(long valore)
        {
            id = valore;
        }

        public MyAlert(System.Data.DataRow row)
        {
            id = long.Parse(row["Id"].ToString());
            dateAdded = (row["date_added"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["date_added"].ToString());
            nome = (row["nome"] is DBNull) ? "" : row["nome"].ToString();
            descrizione = (row["descrizione"] is DBNull) ? "" : row["descrizione"].ToString();
            isEnabled = bool.Parse(row["is_enabled"].ToString());
        }
    }
}
