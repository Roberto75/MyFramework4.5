using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;
using System.Diagnostics;

namespace MyUsers
{
    public class LdapManager
    {
        DirectoryEntry _searchRoot;

        string _server;
        string _login;
        string _password;
        string _container;

        //string _server = "RSDCCTM201.corp.sanpaoloimi.com";
        //string _login = "CORP\\Administrator";
        //string _password = "Passw0rd1";

        public LdapManager(string server, string login, string password, string container)
        {
            _server = server;
            _login = login;
            _password = password;
            _container = container;

            //container = "DC=corp,DC=sanpaoloimi,DC=com";
            //container = "DC=techub,DC=lan";

            string domainPath;
            domainPath = String.Format("LDAP://{0}/{1}", _server, container);

            _searchRoot = new DirectoryEntry(domainPath, _login, _password);
        }




        public string getDistinguishedName()
        {
            foreach (string prop in _searchRoot.Properties.PropertyNames)
            {
                Debug.WriteLine(String.Format("{0} - {1}", prop, _searchRoot.Properties[prop][0]));

            }

            return _searchRoot.Properties["distinguishedName"][0].ToString();

        }



        public void getGroups()
        {

            DirectorySearcher search = new DirectorySearcher(_searchRoot);
            search.Filter = "(&(objectCategory=group))";

            SearchResultCollection resultCol = search.FindAll();
            int conta;
            conta = 0;

            foreach (SearchResult result in resultCol)
            {
                Debug.WriteLine(String.Format("Group [{0}]: {1}", conta, result.Properties["name"][0]));
                conta++;
            }


            //PrincipalContext ctx = new PrincipalContext(ContextType.Domain);

            // define a "query-by-example" principal - here, we search for a GroupPrincipal 
            //GroupPrincipal qbeGroup = new GroupPrincipal(ctx);

            // create your principal searcher passing in the QBE principal    
            //PrincipalSearcher srch = new PrincipalSearcher(qbeGroup);

            // find all matches
            //foreach (Principal found in srch.FindAll())
            //{

            // do whatever here - "found" is of type "Principal" - it could be user, group, computer.....          
            //Debug.WriteLine(found.Name); 
            //}
        }


        public void getGroup(string groupName)
        {
            Debug.WriteLine("AD - GetGroup ");


            DirectorySearcher search = new DirectorySearcher(_searchRoot);
            search.Filter = "(&(objectCategory=group)(CN=" + groupName + "))";

            SearchResultCollection resultCol = search.FindAll();
            int conta;
            conta = 0;

            foreach (SearchResult result in resultCol)
            {
                Debug.WriteLine(String.Format("Group [{0}]: {1}", conta, result.Properties["name"][0]));

                //printSearchResult(result);
                conta++;
            }

        }



        public void printSearchResult(SearchResult result)
        {
            foreach (string prop in result.Properties.PropertyNames)
            {
                Debug.WriteLine(String.Format("{0} - {1}", prop, result.Properties[prop][0]));

            }

        }


        public List<Models.MyUser> getUsers()
        {
            return getUsers(String.Empty);
        }

        public List<Models.MyUser> getUsers(string login)
        {
            DirectorySearcher search = new DirectorySearcher(_searchRoot);
            search.Filter = "(&(objectClass=user)(objectCategory=person)";
            //search.PropertiesToLoad.Add("samaccountname");
            //search.PropertiesToLoad.Add("mail");
            //search.PropertiesToLoad.Add("usergroup");
            //search.PropertiesToLoad.Add("displayname");//first name

            if (!String.IsNullOrEmpty(login))
            {
                //search.Filter += String.Format("(name={0})", login.Trim());

                //search.Filter += String.Format("(samaccountname={0} )", login.Trim());

                search.Filter += String.Format("(|(samaccountname={0})(name={0}))", login.Trim());

            }


            search.Filter += ")";

            List<Models.MyUser> risultato = new List<Models.MyUser>();
            Models.MyUser utente;

            int conta = 0;

            SearchResultCollection resultCol = search.FindAll();
            foreach (SearchResult result in resultCol)
            {
                Debug.WriteLine(String.Format("User [{0}]: {1}", conta, result.Properties["name"][0]));
                utente = new Models.MyUser();

                //printSearchResult(result);

                setMyUser(ref utente, result);

                risultato.Add(utente);

                conta++;
            }


            return risultato;
        }




        private void setMyUser(ref Models.MyUser utente, SearchResult result)
        {

            //http://www.manageengine.com/products/ad-manager/help/csv-import-management/active-directory-ldap-attributes.html

            //utente.SID = new System.Security.Principal.SecurityIdentifier(result.Properties["objectguid"][0] as System.Byte[]);


            var sidInBytes = (byte[])result.Properties["objectsid"][0];
            var sid = new System.Security.Principal.SecurityIdentifier(sidInBytes, 0);

            utente.SID = sid;


            Debug.WriteLine("[SID] " + sid.ToString());

            //memberof - CN=bu01,OU=Service-Groups,DC=techub,DC=lan
            //samaccountname - roberto.rutigliano
            //mail - roberto.rutigliano@techub.it

            if (result.Properties["samaccountname"].Count > 0 && !String.IsNullOrEmpty(result.Properties["samaccountname"][0].ToString()))
            {
                utente.login = result.Properties["samaccountname"][0].ToString();
            }

            if (result.Properties["givenName"].Count > 0 && !String.IsNullOrEmpty(result.Properties["givenName"][0].ToString()))
            {
                utente.nome = result.Properties["givenName"][0].ToString();
            }

            if (result.Properties["sn"].Count > 0 && !String.IsNullOrEmpty(result.Properties["sn"][0].ToString()))
            {
                utente.cognome = result.Properties["sn"][0].ToString();
            }


            if (result.Properties["mail"].Count > 0 && !String.IsNullOrEmpty(result.Properties["mail"][0].ToString()))
            {
                utente.email = result.Properties["mail"][0].ToString();
            }

        }



        public Models.MyUser getUser(string login)
        {
            List<Models.MyUser> risultato;

            risultato = getUsers(login);

            if (risultato.Count == 0)
            {
                return null;
            }


            if (risultato.Count  > 1)
            {
                throw new MyManagerCSharp.MyException("id > 1");
            }

            return risultato[0];


        }

        public Models.MyUser getUser(System.Security.Principal.SecurityIdentifier sid)
        {
            return getUser(sid, null);
        }

        public Models.MyUser getUser(Models.MyUser user)
        {
            // aggiorno i dati dell'utente con quelli dell'Active Directory

            if (user.SID == null)
            {
                return null;
            }

            return getUser(user.SID, user );
        }

        public Models.MyUser getUser(System.Security.Principal.SecurityIdentifier sid, Models.MyUser user)
        {

            //DA CORREGGERE in SEARCH BY SID
            DirectoryEntry searchByUID = new DirectoryEntry("LDAP://" + _server + "/<GUID=" + sid.ToString() + ">", _login, _password);

            DirectorySearcher search = new DirectorySearcher(searchByUID);
            //search.Filter = "(&(objectClass=user)(objectCategory=person)(GUID=" + uid.ToString()+ "))";

            SearchResultCollection resultCol = search.FindAll();

            if (resultCol.Count == 0)
            {
                return null;
            }

            if (resultCol.Count > 1)
            {
                throw new MyManagerCSharp.MyException(MyManagerCSharp.MyException.ErrorNumber.Record_duplicato);
            }

            Debug.WriteLine(String.Format("User;  {0}", resultCol[0].Properties["name"][0]));

            printSearchResult(resultCol[0]);


            if (user == null)
            {
                user = new Models.MyUser();
            }


            setMyUser(ref user, resultCol[0]);
            return user;
        }
        
        public void getUsersFromGroup(string groupName)
        {

            //cerco il gruppo
            DirectorySearcher search = new DirectorySearcher(_searchRoot);
            search.Filter = "(&(objectCategory=group)(CN=" + groupName + "))";

            SearchResultCollection resultCol = search.FindAll();
            int conta;
            conta = 0;

            if (resultCol.Count == 1)
            {
                printSearchResult(resultCol[0]);

                search.Filter = "(&(objectcategory=user)(memberof=" + resultCol[0].Properties["distinguishedname"][0] + "))";

                resultCol = search.FindAll();

                foreach (SearchResult result in resultCol)
                {
                    Debug.WriteLine(String.Format("User [{0}]: {1}", conta, result.Properties["name"][0]));

                    conta++;
                }

            }
        }
        
        public System.Security.Principal.SecurityIdentifier isAuthenticated(string myLogin, string myPassword)
        {

            string domainPath;
            domainPath = String.Format("LDAP://{0}/{1}", _server, _container);

            //domainPath = String.Format("LDAP://{0}", _server);

            DirectoryEntry entry = new DirectoryEntry(domainPath, myLogin, myPassword);

            //Guid? uid = null;

            System.Security.Principal.SecurityIdentifier sid = null;

            try
            {
                //Bind to the native AdsObject to force authentication.
                object obj = entry.NativeObject;

                DirectorySearcher search = new DirectorySearcher(entry);

                search.Filter = "(SAMAccountName=" + myLogin + ")";
                search.PropertiesToLoad.Add("cn");
                //search.PropertiesToLoad.Add("objectguid");
                search.PropertiesToLoad.Add("objectsid");
                

                SearchResult result = search.FindOne();

                if (null == result)
                {
                    return null;
                }

                //Update the new path to the user in the directory.
                //_path = result.Path;

                //_filterAttribute = (string)result.Properties["cn"][0];

                Debug.WriteLine(String.Format("Path: {0}", result.Path));
                Debug.WriteLine(String.Format("CN: {0}", (string)result.Properties["cn"][0]));

                //uid = new Guid(result.Properties["objectguid"][0] as System.Byte[]);

                var sidInBytes = (byte[])result.Properties["objectsid"][0];
                sid = new System.Security.Principal.SecurityIdentifier(sidInBytes, 0);

            }
            catch (System.DirectoryServices.DirectoryServicesCOMException ex)
            {
                if (ex.ErrorCode == -2147023570)
                {
                    //Logon failure: unknown user name or bad password
                    return null;
                }
                Debug.WriteLine("Exception: " + ex.Message);
                throw new Exception("Error authenticating user. " + ex.Message);
            }

            return sid;

        }

    }
}
