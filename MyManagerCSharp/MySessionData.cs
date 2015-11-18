using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MyManagerCSharp
{
    public class MySessionData
    {
        public string Login { get; set; }

        private System.Net.Mail.MailAddress _email;
        public string Email
        {
            set
            {
                _email = new System.Net.Mail.MailAddress(value);
            }

            get
            {
                if (_email == null)
                {
                    return "";
                }

                return _email.ToString();
            }
        }

        public string Roles { get; set; }

        private List<Models.MyGroupSmall> _groups;
        public List<Models.MyGroupSmall> Groups
        {
            get
            {
                if (_groups == null)
                {
                    _groups = new List<Models.MyGroupSmall>();
                }
                return _groups;
            }

            set
            {
                _groups = value;
            }
        }

        //public string Profilo { get; set; }
        public string Profili { get; set; }

        public long UserId
        {
            set
            {
                _userId = value;
            }
            get
            {
                return _userId;
            }
        }

        public System.Security.Principal.SecurityIdentifier SID { get { return _sid; } }

        private long _userId = -1;
        private System.Security.Principal.SecurityIdentifier _sid;

        public MySessionData()
        {

        }

        public MySessionData(long userId)
        {
            _userId = userId;
        }

        public MySessionData(System.Security.Principal.SecurityIdentifier sid)
        {
            _sid = sid;
        }

        public bool IsAuthenticated
        {
            get
            {
                return (_userId != -1) || (_sid != null);
            }
        }

        public bool IsInRole(string role)
        {
            if (String.IsNullOrEmpty(Roles))
            {
                return false;
            }

            bool esito;
            esito = Roles.IndexOf(role.ToUpper() + ";") != -1;

            //Debug.WriteLine("MySessionData IsInRole: " + role + ": " + esito);
            return esito;
        }


        public bool IsInProfile(string role)
        {
            if (String.IsNullOrEmpty(Profili))
            {
                return false;
            }

            bool esito;
            esito = Profili.IndexOf(role.ToUpper() + ";") != -1;

            Debug.WriteLine("MySessionData IsInProfile: " + role + ": " + esito);
            return esito;
        }


        public bool IsInProfile(string[] roles)
        {
            if (String.IsNullOrEmpty(Profili))
            {
                return false;
            }

            bool esito;

            foreach (string r in roles)
            {
                esito = IsInProfile(r);
                if (esito == true)
                {
                    return true;
                }
            }

            return false;
        }



        public bool IsInGroupAdministrators()
        {
            return IsInGroup("Administrators");
        }

        public bool IsInGroup(string group)
        {
            if (Groups == null || Groups.Count == 0)
            {
                return false;
            }

            Models.MyGroupSmall item;

            item = Groups.Find(x => x.nome == group);


            return item != null;
        }

        public List<Models.MyGroupSmall> getGroupByTipo(string tipo)
        {
            if (Groups == null || Groups.Count == 0)
            {
                return null;
            }

            List<Models.MyGroupSmall> items;

            items = Groups.FindAll(x => x.tipo == tipo);

            return items;
        }



        public virtual void LogOff()
        {
            _userId = -1;
            _sid = null;
            Roles = "";
            Login = "";
            // Profilo = "";
            Profili = "";
            Groups.Clear();
        }

    }
}
