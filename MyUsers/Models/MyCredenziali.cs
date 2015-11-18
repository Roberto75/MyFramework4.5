using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyUsers.Models
{
    public class MyCredenziali
    {
        private string _login;
        private string _password;

        public string Login
        {

            get { return _login; }
        }

        public string Password
        {

            get { return _password; }
        }

        public MyCredenziali(string login, string password)
        {
            _login = login;
            _password = password;
        }



    }
}
