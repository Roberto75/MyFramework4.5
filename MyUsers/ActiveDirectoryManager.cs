using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices.AccountManagement;
using System.Diagnostics;

namespace MyUsers
{
    public class ActiveDirectoryManager
    {

        PrincipalContext ctx;

        public ActiveDirectoryManager()
        {
            // create your domain context
            ctx = new PrincipalContext(ContextType.Domain);
            //ctx = new PrincipalContext(ContextType.Domain, " 123.123.123.123","CORP\\Administrator","PASSWORD");

            //ctx = new PrincipalContext(ContextType.Domain, "xxxxx.com", "USER", "PASSWORD");


            //bool esito;
            //esito = ctx.ValidateCredentials("CORP\user", "PASSWORD");
        }


        public void getGroups()
        {

            //PrincipalContext ctx = new PrincipalContext(ContextType.Domain);

            // define a "query-by-example" principal - here, we search for a GroupPrincipal 
            GroupPrincipal qbeGroup = new GroupPrincipal(ctx);

            // create your principal searcher passing in the QBE principal    
            PrincipalSearcher srch = new PrincipalSearcher(qbeGroup);

            // find all matches
            foreach (Principal found in srch.FindAll())
            {

                // do whatever here - "found" is of type "Principal" - it could be user, group, computer.....          
                Debug.WriteLine(found.Name);
            }
        }


        public void getGroup(string groupName)
        {
            Debug.WriteLine("AD - GetGroups ");
            GroupPrincipal grp = GroupPrincipal.FindByIdentity(ctx, IdentityType.Name, groupName);

            if (grp != null)
            {
                foreach (Principal p in grp.GetMembers(true))
                {
                    Debug.WriteLine(p.Name); //You can add more attributes, samaccountname, UPN, DN, object type, etc... 
                }


                grp.Dispose();
                ctx.Dispose();

            }
            else
            {
                Console.WriteLine("\nWe did not find that group in that domain, perhaps the group resides in a different domain?");
            }
        }





        public void getUsers()
        {
        }


        public string getEmail(System.Security.Principal.SecurityIdentifier sid)
        {
            UserPrincipal user = UserPrincipal.FindByIdentity(ctx, IdentityType.Sid, sid.Value);
            if (user == null)
            {
                return "";
            }

            Debug.WriteLine("DisplayName: " + user.DisplayName);

            return user.EmailAddress;

        }


        public Models.MyUser getUser(System.Security.Principal.SecurityIdentifier sid)
        {

            UserPrincipal user = UserPrincipal.FindByIdentity(ctx, IdentityType.Sid, sid.Value);
            if (user == null)
            {
                return null;
            }

            Debug.WriteLine("DisplayName: " + user.DisplayName);

            Models.MyUser u = new Models.MyUser();
            u.nome = user.GivenName;
            u.cognome = user.Surname;
            u.SID = user.Sid;
            u.email = user.EmailAddress;
            u.login = user.SamAccountName;

            return u;
        }


        public void getUser(string userName)
        {


            UserPrincipal user = UserPrincipal.FindByIdentity(ctx, IdentityType.DistinguishedName, "CN=" + userName + ",CN=Users,DC=corp,DC=sanpaoloimi,DC=com");
            if (user != null)
            {
                Debug.WriteLine("DisplayName: " + user.DisplayName);
            }
        }

    }
}
