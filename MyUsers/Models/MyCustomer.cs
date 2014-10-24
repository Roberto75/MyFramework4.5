using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MyUsers.Models
{
    public class MyCustomer
    {
        [Key]
        public int customerId { get; set; }
        public string ragioneSociale { get; set; }
        public string codiceFiscale { get; set; }
        public string partitaIva { get; set; }

        public string indirizzo { get; set; }
        public string numero_civico { get; set; }
        public string cap { get; set; }
        public string http { get; set; }
        public string fax { get; set; }

        public string regione { get; set; }
        public string provincia { get; set; }
        public string comune { get; set; }
        public int regioneId { get; set; }
        public string provinciaId { get; set; }
        public string comuneId { get; set; }


        public bool isEnabled { get; set; }
        public DateTime dateAdded { get; set; }
        public DateTime? dateModified { get; set; }

        // public virtual ICollection<MyTest> Utenti { get; set; }

        public MyCustomer()
        {
            dateAdded = DateTime.Now;
        }


        public MyCustomer(System.Data.DataRow row)
        {
            customerId = int.Parse(row["customer_id"].ToString());
            ragioneSociale = row["ragione_sociale"].ToString();
            codiceFiscale = row["codice_fiscale"].ToString();
            dateAdded = DateTime.Parse(row["date_added"].ToString());
            
            if (row["date_modified"] is DBNull)
            {
                dateModified = null;
            }
            else
            {
                dateModified = DateTime.Parse(row["date_modified"].ToString());
            }


           // isEnabled = bool.Parse(row["is_enabled"].ToString());
        }

    }
}
