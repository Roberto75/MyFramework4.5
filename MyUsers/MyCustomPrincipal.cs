using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

namespace MyUsers
{
    public class MyCustomPrincipal : System.Security.Principal.IPrincipal 
    {
        public System.Security.Principal.IIdentity Identity { get; private set; }

        public MyCustomPrincipal(MyCustomIdentity identity)
        {
            this.Identity = identity;
        }


        public bool IsInRole(string role)
        {
            bool esito;
            //esito = (((MyCustomIndentity)this.Identity).Roles.IndexOf(role.ToUpper() + "#") != -1);

            esito = false;


            Debug.WriteLine("MyCustomPrincipal IsInRole: " + role + ": " + esito);
            return esito;
        }

        // public bool IsInRole( System.Security.Principal.SecurityIdentifier sid){

        //     Debug.WriteLine("IsInRole");
        //     return true;
        // }

    }
}
