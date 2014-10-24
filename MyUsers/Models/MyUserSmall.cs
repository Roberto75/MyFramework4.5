using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyUsers.Models
{
    public class MyUserSmall
    {
        //ho reso la userId nullable per poterla utilizzare come filtro opzionale nelle ricerche
        public long? userId { get; set; }

        public string nome { get; set; }
        public string cognome { get; set; }

        public string login { get; set; }
        public string email { get; set; }


        public string NomeCognome
        {
            get
            {
                string temp;

                temp = nome;

                if (String.IsNullOrEmpty(temp))
                {
                    temp = cognome;
                }
                else
                {
                    temp += " " + cognome;
                }

                return temp;
            }
        }

        public MyUserSmall()
        {

        }

        public MyUserSmall(System.Data.DataRow row)
        {
            userId = long.Parse(row["user_id"].ToString());
            login = row["my_login"].ToString();
            nome = row["nome"].ToString();
            cognome = row["cognome"].ToString();
            email = row["email"].ToString();
        }

    }
}
