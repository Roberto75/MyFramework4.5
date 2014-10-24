using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MyUsers
{
    public class MyCustomPrincipalWindows : System.Security.Principal.WindowsPrincipal
    {

        //List<string> _roles;
        string _roles;

        public MyCustomPrincipalWindows(System.Security.Principal.WindowsIdentity identity)
            : base(identity)
        {

           // _roles = roles;


            MyUsers.UserManager manager = new MyUsers.UserManager("DefaultConnection");
            manager.openConnection();
            try
            {
                System.Security.Principal.SecurityIdentifier sid = new System.Security.Principal.SecurityIdentifier( identity.User.Value);
             

           //   _roles = manager.getRoles(sid);

            }
            finally
            {
                manager.closeConnection();
            }

        }


        public override bool IsInRole(string role)
        {
            // if (base.IsInRole(role) || _roles.Contains(role))
            bool esito;
            esito = _roles.IndexOf(role.ToUpper() + ";") != -1;

            if (base.IsInRole(role) || esito)
            {

                Debug.WriteLine("MyCustomPrincipalWindows IsInRole: " + role + ": " + esito);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
