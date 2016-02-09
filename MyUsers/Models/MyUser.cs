using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MyUsers.Models
{
    public class MyUser : MyUserSmall
    {

        public enum SelectFileds
        {
            Lista,
            Full
        }
        public string password { get; set; }

        // Foreign key
        public long? customerId { get; set; }
        public virtual MyCustomer Customer { get; set; }

        // Foreign key
        //public string profiloId { get; set; }
        //public virtual MyProfile Profilo { get; set; }

        public virtual List<MyProfile> Profili { get; set; }
        public virtual List<MyGroup> Gruppi { get; set; }
        public virtual List<MyRole> Ruoli { get; set; }


        public string codiceFiscale { get; set; }

        public string telefono { get; set; }
        public string mobile { get; set; }
        public string indirizzo { get; set; }
        public string numero_civico { get; set; }
        public string cap { get; set; }
        public string http { get; set; }
        public string fax { get; set; }
        public string sesso { get; set; }
        public DateTime? dataDiNascita { get; set; }

        public bool? isEnabled { get; set; }
        public int? loginSuccess { get; set; }
        public int? loginFailed { get; set; }

        public DateTime? dateAdded { get; set; }
        public DateTime? dateModified { get; set; }
        public DateTime? dateLastLogin { get; set; }
        public DateTime? datePreviousLogin { get; set; }
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

        //*** Active Directory
        // public Guid? uidAD { get; set; }
        public System.Security.Principal.SecurityIdentifier SID { get; set; }

        public MyUser()
        {
            //this.Gruppi = new HashSet<MyGroup>();
            dateAdded = DateTime.Now;
        }

        public MyUser(System.Data.DataRow row, SelectFileds mode)
        {
            userId = long.Parse(row["user_id"].ToString());
            login = row["my_login"].ToString();
            nome = row["nome"].ToString();
            cognome = row["cognome"].ToString();
            email = row["email"].ToString();
            dateAdded = (row["date_added"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["date_added"].ToString());

            if (row["date_last_login"] is DBNull)
            {
                dateLastLogin = null;
            }
            else
            {
                dateLastLogin = DateTime.Parse(row["date_last_login"].ToString());
            }

            if (row["date_previous_login"] is DBNull)
            {
                datePreviousLogin = null;
            }
            else
            {
                datePreviousLogin = DateTime.Parse(row["date_previous_login"].ToString());
            }


            if (row["customer_id"] is DBNull)
            {
                customerId = null;
            }
            else
            {
                customerId = long.Parse(row["customer_id"].ToString());
            }
                                  

            if (row["is_enabled"] is DBNull)
            {
                isEnabled = null;
            }
            else
            {
                isEnabled = bool.Parse(row["is_enabled"].ToString());
            }


            if (row["sid"] is DBNull)
            {
                SID = null;
            }
            else
            {
                SID = new System.Security.Principal.SecurityIdentifier(row["sid"].ToString());
            }


            if (mode == SelectFileds.Full)
            {
                // profiloId = (row["profilo_Id"] is DBNull) ? "" : row["profilo_Id"].ToString();
                codiceFiscale = (row["codice_fiscale"] is DBNull) ? "" : row["codice_fiscale"].ToString();
                telefono = (row["telefono"] is DBNull) ? "" : row["telefono"].ToString();
                mobile = (row["mobile"] is DBNull) ? "" : row["mobile"].ToString();
                indirizzo = (row["indirizzo"] is DBNull) ? "" : row["indirizzo"].ToString();
                numero_civico = (row["numero_civico"] is DBNull) ? "" : row["numero_civico"].ToString();
                cap = (row["cap"] is DBNull) ? "" : row["cap"].ToString();
                http = (row["http"] is DBNull) ? "" : row["http"].ToString();
                fax = (row["fax"] is DBNull) ? "" : row["fax"].ToString();
                sesso = (row["sesso"] is DBNull) ? "" : row["sesso"].ToString();
                telefono = (row["telefono"] is DBNull) ? "" : row["telefono"].ToString();
                loginSuccess = (row["login_success"] is DBNull) ? 0 : int.Parse(row["login_success"].ToString());
                loginFailed = (row["login_failure"] is DBNull) ? 0 : int.Parse(row["login_failure"].ToString());
                //havePhoto = (row["photo"] is DBNull) ? null : bool.Parse(row["photo"].ToString());
                tipoImpresa = (row["tipologia"] is DBNull) ? "" : row["tipologia"].ToString();
                descrizioneImpresa = (row["descrizione"] is DBNull) ? "" : row["descrizione"].ToString();

                regione = (row["regione"] is DBNull) ? "" : row["regione"].ToString();
                regioneId = (row["regione_id"] is DBNull) ? -1 : int.Parse(row["regione_id"].ToString());
                provincia = (row["provincia"] is DBNull) ? "" : row["provincia"].ToString();
                provinciaId = (row["provincia_id"] is DBNull) ? "" : row["provincia_id"].ToString();
                comune = (row["comune"] is DBNull) ? "" : row["comune"].ToString();
                comuneId = (row["comune_id"] is DBNull) ? "" : row["comune_id"].ToString();

                dateModified = (row["date_modified"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["date_modified"].ToString());
                dateModifiedPassword = (row["date_modified_password"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["date_modified_password"].ToString());
                dateExpire = (row["date_expire"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["date_expire"].ToString());
                dateExpirePassword = (row["date_expire_password"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["date_expire_password"].ToString());
                dateDeleted = (row["date_deleted"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["date_deleted"].ToString());
                dateActivationAccount = (row["date_activation_account"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["date_activation_account"].ToString());

                dataDiNascita = (row["date_of_birth"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["date_of_birth"].ToString());

            }

        }


        //public MyUser(System.Data.DataRow row, SelectFileds from)
        //{

        //    //switch (from)
        //    //{

        //    //    case CreateFrom.Lista:
        //    //        userId = long.Parse(row["userId"].ToString());
        //    //        login = row["my_login"].ToString();
        //    //        nome = row["nome"].ToString();
        //    //        cognome = row["cognome"].ToString();
        //    //        email = row["email"].ToString();
        //    //        dateAdded = DateTime.Parse(row["date_added"].ToString());
        //    //        dateLastLogin = (row["date_last_login"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["date_last_login"].ToString());
        //    //        isEnabled = bool.Parse(row["is_enabled"].ToString());
        //    //        break;
        //    //    case CreateFrom.Full:
        //    //        profiloId = (row["profilo_Id"] is DBNull) ? "" : row["profilo_Id"].ToString();
        //    //        break;

        //    //}


        //    userId = long.Parse(row["user_id"].ToString());
        //    login = row["my_login"].ToString();
        //    nome = row["nome"].ToString();
        //    cognome = row["cognome"].ToString();
        //    email = row["email"].ToString();
        //    dateAdded = DateTime.Parse(row["date_added"].ToString());
        //    dateLastLogin = (row["date_last_login"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["date_last_login"].ToString());
        //    isEnabled = bool.Parse(row["is_enabled"].ToString());

        //    if (from == SelectFileds.Full)
        //    {
        //        profiloId = (row["profilo_Id"] is DBNull) ? "" : row["profilo_Id"].ToString();
        //        codiceFiscale = (row["codice_fiscale"] is DBNull) ? "" : row["codice_fiscale"].ToString();
        //        telefono = (row["telefono"] is DBNull) ? "" : row["telefono"].ToString();
        //        mobile  = (row["mobile"] is DBNull) ? "" : row["mobile"].ToString();
        //        indirizzo = (row["indirizzo"] is DBNull) ? "" : row["indirizzo"].ToString();
        //        numero_civico = (row["numero_civico"] is DBNull) ? "" : row["numero_civico"].ToString();
        //        cap = (row["cap"] is DBNull) ? "" : row["cap"].ToString();
        //        http = (row["http"] is DBNull) ? "" : row["http"].ToString();
        //        fax = (row["fax"] is DBNull) ? "" : row["fax"].ToString();
        //        sesso = (row["sesso"] is DBNull) ? "" : row["sesso"].ToString();
        //        telefono = (row["telefono"] is DBNull) ? "" : row["telefono"].ToString();
        //        loginSuccess = (row["login_success"] is DBNull) ? 0 : long.Parse (row["login_success"].ToString());
        //        loginFailed = (row["login_failure"] is DBNull) ? 0 : long.Parse(row["login_failure"].ToString());
        //        //havePhoto = (row["photo"] is DBNull) ? null : bool.Parse(row["photo"].ToString());
        //        tipoImpresa = (row["tipologia"] is DBNull) ? "" : row["tipologia"].ToString();
        //        descrizioneImpresa = (row["descrizione"] is DBNull) ? "" : row["descrizione"].ToString();

        //        regione = (row["regione"] is DBNull) ? "" : row["regione"].ToString();
        //        regioneId = (row["regione_id"] is DBNull) ? -1 : long.Parse (row["regione_id"].ToString());
        //        provincia = (row["provincia"] is DBNull) ? "" : row["provincia"].ToString();
        //        provinciaId = (row["provincia_id"] is DBNull) ? "" : row["provincia_id"].ToString();
        //        comune = (row["comune"] is DBNull) ? "" : row["comune"].ToString();
        //        comuneId  = (row["comune_id"] is DBNull) ? "" : row["comune_id"].ToString();

        //        dateModified = (row["date_modified"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["date_modified"].ToString());
        //        dateModifiedPassword = (row["date_modified_password"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["date_modified_password"].ToString());
        //        dateExpire = (row["date_expire"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["date_expire"].ToString());
        //        dateExpirePassword = (row["date_expire_password"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["date_expire_password"].ToString());
        //        dateDeleted = (row["date_deleted"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["date_deleted"].ToString());
        //        dateActivationAccount = (row["date_activation_account"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["date_activation_account"].ToString());

        //        dataDiNascita = (row["date_of_birth"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["date_of_birth"].ToString());
        //        customerId  = (row["customer_id"] is DBNull) ? -1 : int.Parse(row["customer_id"].ToString());
        //    }






        //}



    }
}
