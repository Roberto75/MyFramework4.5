using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyUsers.Models
{
    public class MyTest
    {
        public int  userId { get; set; }

        public string nome { get; set; }
        public string cognome { get; set; }
        public string login { get; set; }
        public string password { get; set; }


        public string profiloId { get; set; }
        public virtual MyProfile Profilo { get; set; }

        public int? customerId { get; set; }
        public virtual MyCustomer Customer { get; set; }

       // public virtual ICollection<MyGroup> Gruppi { get; set; }

        public string codiceFiscale { get; set; }
        public string email { get; set; }
        public string telefono { get; set; }
        public string mobile { get; set; }
        public string indirizzo { get; set; }
        public string numero_civico { get; set; }
        public string cap { get; set; }
        public string http { get; set; }
        public string fax { get; set; }
        public string sesso { get; set; }
        public DateTime? dataDiNascita { get; set; }

        public bool isEnabled { get; set; }
        public int? loginSuccess { get; set; }
        public int? loginFailed { get; set; }

        public DateTime dateAdded { get; set; }
        public DateTime? dateModified { get; set; }
        public DateTime? dateLastLogin { get; set; }
        public DateTime? dateExpire { get; set; }
        public DateTime? dateExpirePassword { get; set; }
        public DateTime? dateModifiedPassword { get; set; }
        public DateTime? dateDeleted { get; set; }
        public DateTime? dateActivationAccount { get; set; }

        public bool? havePhoto { get; set; }
        public string tipoImpresa { get; set; }
        public string descrizioneImpresa { get; set; }

        public string regione { get; set; }
        public string provincia { get; set; }
        public string comune { get; set; }
        public int? regioneId { get; set; }
        public string provinciaId { get; set; }
        public string comuneId { get; set; }


        public string emailChanging { get; set; }
        public string emailActivationCode { get; set; }

        public int? dayExpirePassword { get; set; }

    }
}
