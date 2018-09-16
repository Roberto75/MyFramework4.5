using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;
using MyManagerCSharp;

namespace MyUsers
{
    public class UserManager : ManagerDB
    {

        public const string _sqlElencoUtenti = "SELECT  t1.user_id, t1.my_login, t1.nome, t1.cognome , t1.email,  t1.date_added,  t1.date_previous_login, t1.date_last_login, t1.is_enabled, t1.customer_id , t1.sid ";


        public UserManager(string connectionName)
            : base(connectionName)
        {

        }

        public UserManager(System.Data.Common.DbConnection connection)
            : base(connection)
        {

        }


        public byte[] exportToCSV()
        {
            mStrSQL = "select t2.my_login as Login , t1.date_added as Access, " +
                " CASE tipo  " +
                "   WHEN '" + MyManagerCSharp.Log.LogUserManager.LogType.LoginMobile.ToString() + "' THEN 'Mobile' " +
                "	ELSE '' " +
                " END as Mobile " +
                " from MyLogUser as t1 " +
                " join Utente as t2 on t1.user_id = t2.user_id " +
                " where tipo='" + MyManagerCSharp.Log.LogUserManager.LogType.Login.ToString() + "' or tipo = '" + MyManagerCSharp.Log.LogUserManager.LogType.LoginMobile.ToString() + "' " +
                " order by t2.my_login, t1.date_added";

            mDt = mFillDataTable(mStrSQL);

            string content;
            content = MyManagerCSharp.CSVManager.toCSV(mDt, ";", true);

            byte[] contentByte = new byte[content.Length * sizeof(char)];
            System.Buffer.BlockCopy(content.ToCharArray(), 0, contentByte, 0, contentByte.Length);

            return contentByte;
        }



        public void getList(Models.SearchUsers model)
        {

            List<Models.MyUser> risultato;
            risultato = new List<Models.MyUser>();

            mStrSQL = " FROM UTENTE as t1";

            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();



            string strWHERE = "";
            if (model.filter != null)
            {
                if (!String.IsNullOrEmpty(model.filter.nome))
                {
                    strWHERE += " AND UPPER(nome) like  @NOME";
                    mAddParameter(command, "@NOME", "%" + model.filter.nome.ToUpper().Trim() + "%");
                }

                if (!String.IsNullOrEmpty(model.filter.cognome))
                {
                    strWHERE += " AND UPPER(cognome) like  @COGNOME";
                    mAddParameter(command, "@COGNOME", "%" + model.filter.cognome.ToUpper().Trim() + "%");
                }

                if (!String.IsNullOrEmpty(model.filter.email))
                {
                    strWHERE += " AND UPPER(email) like  @EMAIL";
                    mAddParameter(command, "@EMAIL", "%" + model.filter.email.ToUpper().Trim() + "%");
                }

                if (!String.IsNullOrEmpty(model.filter.login))
                {
                    strWHERE += " AND UPPER(my_login) like  @MY_LOGIN";
                    mAddParameter(command, "@MY_LOGIN", "%" + model.filter.login.ToUpper().Trim() + "%");
                }
            }


            if (!String.IsNullOrEmpty(strWHERE))
            {
                mStrSQL += " WHERE (1=1) " + strWHERE;
            }




            string temp;
            int totalRecords;

            //paginazione
            if (model.PageSize > 0 && (mConnection is System.Data.SqlClient.SqlConnection))
            {
                temp = "SELECT COUNT(*) " + mStrSQL;
                command.CommandText = temp;

                totalRecords = int.Parse(mExecuteScalar(command));
                model.TotalRows = totalRecords;
            }

            string orderBy = "";
            if (!String.IsNullOrEmpty(model.Sort))
            {
                string sortField = getSortField(model.Sort);

                if (model.SortDir.ToUpper().Trim() != "ASC" && model.SortDir.ToUpper().Trim() != "DESC")
                {
                    model.SortDir = "ASC";
                }
                Debug.WriteLine("ORDER BY " + sortField + " " + model.SortDir);

                orderBy = " ORDER BY " + sortField + " " + model.SortDir;
            }


            temp = _sqlElencoUtenti + mStrSQL + orderBy;


            if (model.PageSize > 0 && (mConnection is System.Data.SqlClient.SqlConnection))
            {
                temp += " OFFSET " + ((model.PageNumber - 1) * model.PageSize) + " ROWS FETCH NEXT " + model.PageSize + " ROWS ONLY";
            }


            command.CommandText = temp;
            command.Connection = mConnection;
            mDt = mFillDataTable(command);

            if (model.PageSize > 0 && !(mConnection is System.Data.SqlClient.SqlConnection))
            {
                model.TotalRows = mDt.Rows.Count;

                // apply paging
                IEnumerable<DataRow> rows = mDt.AsEnumerable().Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize);
                foreach (DataRow row in rows)
                {
                    risultato.Add(new Models.MyUser(row, Models.MyUser.SelectFileds.Lista));
                }
            }
            else
            {
                foreach (DataRow row in mDt.Rows)
                {
                    risultato.Add(new Models.MyUser(row, Models.MyUser.SelectFileds.Lista));
                }
            }

            model.Utenti = risultato;
        }


        private string getSortField(string modelSort)
        {
            string sortField = "";

            switch (modelSort)
            {
                case "Login":
                    sortField = "my_login";
                    break;
                case "DateAdded":
                    sortField = "DATE_ADDED";
                    break;
                case "DateLastLogin":
                    sortField = "date_last_login";
                    break;
                default:
                    sortField = modelSort;
                    break;
            }

            Debug.WriteLine("modelSort: " + modelSort + " => " + sortField);
            return sortField;
        }

        public Models.MyUser getUser(long id)
        {
            mStrSQL = "SELECT * FROM UTENTE WHERE user_id = " + id;
            mDt = mFillDataTable(mStrSQL);

            if (mDt.Rows.Count == 0)
            {
                return null;
            }

            if (mDt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException("id > 0");
            }

            Models.MyUser u;
            u = new Models.MyUser(mDt.Rows[0], Models.MyUser.SelectFileds.Full);
            //u = new Models.MyUser(mDt.Rows[0]);

            return u;
        }


        public Models.MyUser getUserFromSID(System.Security.Principal.SecurityIdentifier sid)
        {

            mStrSQL = "SELECT * FROM UTENTE WHERE SID = '" + sid.ToString() + "'";

            mDt = mFillDataTable(mStrSQL);

            if (mDt.Rows.Count == 0)
            {
                return null;
            }

            if (mDt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException("id > 0");
            }

            Models.MyUser u;
            u = new Models.MyUser(mDt.Rows[0], Models.MyUser.SelectFileds.Full);
            //u = new Models.MyUser(mDt.Rows[0]);

            return u;
        }

        public long getUserIdFromSID(System.Security.Principal.SecurityIdentifier sid)
        {
            string temp;
            temp = mExecuteScalar("SELECT user_id FROM UTENTE WHERE SID = '" + sid.ToString() + "'");

            if (String.IsNullOrEmpty(temp))
            {
                return -1;
            }

            return long.Parse(temp);
        }



        public int addLoginSucces(System.Security.Principal.SecurityIdentifier sid, string login, string ip)
        {
            mStrSQL = "update UTENTE set date_last_login= GetDate() , login_success= login_success +1 , ip_address = '" + ip + "' where UPPER(SID) = @SID ";

            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();
            command.CommandText = mStrSQL;

            mAddParameter(command, "@SID", sid.ToString().ToUpper());

            try
            {
                int esito;
                esito = mExecuteNoQuery(command);

                //if (esito == 0)
                //{
                //    insertActiveDirectorySID(sid, login);
                //}

                return esito;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.Message);
            }

            return -1;
        }

        public void addLoginSucces(long userId, string ip)
        {
            mStrSQL = "update UTENTE set date_last_login= GetDate() , login_success= login_success +1 , [date_previous_login] = date_last_login ";

            if (!String.IsNullOrEmpty(ip))
            {
                mStrSQL += ", ip_address = '" + ip + "'";
            }
            mStrSQL += " WHERE user_id = " + userId;

            mExecuteNoQuery(mStrSQL);
        }

        public long isAuthenticated(string myLogin, string myPassword)
        {
            return isAuthenticated(myLogin, myPassword, "");
        }

        public long isAuthenticated(string myLogin, string myPassword, string ip)
        {
            long userId = -1;

            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();

            mStrSQL = "select user_id, is_enabled, my_password, email from utente where UPPER(my_login) = @mylogin ";
            mAddParameter(command, "@mylogin", myLogin.ToUpper());
            command.CommandText = mStrSQL;

            try
            {
                mDt = mFillDataTable(command);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw new MyManagerCSharp.MyException("isAuthenticated", ex);
            }


            // Controllo se l'utente è censito
            if (mDt.Rows.Count == 0)
            {
                //utente non è censito
                throw new MyManagerCSharp.MyException(MyManagerCSharp.MyException.ErrorNumber.LoginPasswordErrati);
            }

            if (mDt.Rows.Count > 1)
            {
                //utente non è censito
                throw new MyManagerCSharp.MyException(MyManagerCSharp.MyException.ErrorNumber.LoginPasswordErrati);
            }

            userId = long.Parse(mDt.Rows[0]["USER_ID"].ToString());

            if (userId == -1)
            {

            }

            if (!bool.Parse(mDt.Rows[0]["is_enabled"].ToString()))
            {
                throw new MyManagerCSharp.MyException(MyManagerCSharp.MyException.ErrorNumber.UtenteDisabilitato);
            }


            string passwordMD5;
            passwordMD5 = MyManagerCSharp.SecurityManager.getMD5Hash(myPassword.Trim());

            //Debug.WriteLine(String.Format("psw:{0} MD5:{1} USER:{2} ", myPassword.Trim(), passwordMD5, mDt.Rows[0]["my_password"]));


            if (!String.Equals(passwordMD5, mDt.Rows[0]["my_password"].ToString(), StringComparison.OrdinalIgnoreCase))
            {
                mStrSQL = "update UTENTE set login_failure=login_failure +1 where user_id=  " + userId;
                mExecuteNoQuery(mStrSQL);
                throw new MyManagerCSharp.MyException(MyManagerCSharp.MyException.ErrorNumber.LoginPasswordErrati);
            }

            // TUTTO OK!!!!
            //metto i dati dell'utente in sessione 
            addLoginSucces(userId, ip);

            return userId;
        }


        public string getRoles(long userId)
        {
            //07/01/2017 Modifica per compatibilità con Access 2017
            if (mConnection.GetType().Name == "OleDbConnection")
            {
                mStrSQL = "select t1.ruolo_id from ((ruolo as t1 " +
                " inner join GruppoRuolo as t2 on t1.ruolo_id = t2.ruolo_id) " +
                " inner join UtenteGruppo as t3 on t2.gruppo_id = t3.gruppo_id )" +
                " where t3.user_id = " + userId;
            }
            else
            {
                mStrSQL = "select t1.ruolo_id from ruolo as t1 " +
             " join GruppoRuolo as t2 on t1.ruolo_id = t2.ruolo_id " +
             " join UtenteGruppo as t3 on t2.gruppo_id = t3.gruppo_id " +
             " where t3.user_id = " + userId;
            }




            Debug.WriteLine(mStrSQL);

            mDt = mFillDataTable(mStrSQL);

            string roles;
            roles = "";
            foreach (DataRow row in mDt.Rows)
            {
                roles += row[0] + ";";
            }

            return roles;
        }


        public List<Models.MyRole> getRoles()
        {
            List<Models.MyRole> risultato = new List<Models.MyRole>();
            mStrSQL = "select t1.ruolo_id , t1.nome from ruolo as t1  order by nome";

            mDt = mFillDataTable(mStrSQL);
            Models.MyRole ruolo;
            foreach (DataRow row in mDt.Rows)
            {
                ruolo = new Models.MyRole(row);
                risultato.Add(ruolo);
            }

            return risultato;
        }


        public bool updateProfili(IEnumerable<Models.MyProfile> profili, long userId)
        {
            mStrSQL = "DELETE FROM  UtenteProfilo WHERE user_id = " + userId;
            mExecuteNoQuery(mStrSQL);

            foreach (Models.MyProfile p in profili)
            {
                mStrSQL = "INSERT INTO UtenteProfilo ( date_added,  profilo_id, user_id ) VALUES ( GetDate() , '" + p.profiloId + "'," + userId + ")";
                mExecuteNoQuery(mStrSQL);
            }

            return true;
        }



        public string getRoles(System.Security.Principal.SecurityIdentifier sid)
        {
            mStrSQL = "select t1.ruolo_id from ruolo as t1 " +
                " join GruppoRuolo as t2 on t1.ruolo_id = t2.ruolo_id " +
                " join UtenteGruppo as t3 on t2.gruppo_id = t3.gruppo_id " +
                " join Utente as t4 on t3.user_id = t4.user_id " +
                " where t4.sid = '" + sid.ToString() + "'";

            mDt = mFillDataTable(mStrSQL);

            string roles;
            roles = "";
            foreach (DataRow row in mDt.Rows)
            {
                roles += row[0] + ";";
            }

            return roles;
        }


        public List<MyManagerCSharp.Models.MyGroupSmall> getGroupSmall(long userId)
        {
            List<MyManagerCSharp.Models.MyGroupSmall> risultato = new List<MyManagerCSharp.Models.MyGroupSmall>();
            MyManagerCSharp.Models.MyGroupSmall gruppo;

            mStrSQL = "select t2.* from UtenteGruppo as t1 left join gruppo  as t2 on (t1.gruppo_id = t2.gruppo_id) WHERE t1.user_id = " + userId;

            mDt = mFillDataTable(mStrSQL);

            foreach (DataRow row in mDt.Rows)
            {
                gruppo = new MyManagerCSharp.Models.MyGroupSmall(row);
                risultato.Add(gruppo);
            }

            return risultato;
        }


        public string getProfili(long userId)
        {
            mStrSQL = "select t1.profilo_id from UtenteProfilo as t1  where t1.user_id = " + userId;

            mDt = mFillDataTable(mStrSQL);

            string profili;
            profili = "";
            foreach (DataRow row in mDt.Rows)
            {
                profili += row[0] + ";";
            }

            return profili;
        }

        public string getEmail(long userId)
        {
            mStrSQL = "SELECT EMAIL FROM UTENTE WHERE USER_ID = " + userId;
            return mExecuteScalar(mStrSQL);
        }


        public long? getCustomerId(long userId)
        {
            mStrSQL = "SELECT CUSTOMER_ID FROM UTENTE WHERE USER_ID = " + userId;
            string temp = mExecuteScalar(mStrSQL);

            if (String.IsNullOrEmpty(temp))
            {
                return null;
            }

            return long.Parse(temp);
        }


        public long insertActiveDirectorySID(System.Security.Principal.SecurityIdentifier sid, string login)
        {
            Models.MyUser user = new Models.MyUser();
            user.SID = sid;
            user.login = login;
            user.isEnabled = true;

            return insert(user);
        }



        public long insert(Models.MyUser u)
        {
            long newId = -1;
            string strSQLParametri = "";

            mStrSQL = "INSERT INTO UTENTE ( date_added  ";
            strSQLParametri = " VALUES ( GetDate()  ";

            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();

            if (!String.IsNullOrEmpty(u.nome))
            {
                mStrSQL += ",NOME ";
                strSQLParametri += ", @NOME ";
                mAddParameter(command, "@NOME", u.nome);
            }

            if (!String.IsNullOrEmpty(u.cognome))
            {
                mStrSQL += ",COGNOME ";
                strSQLParametri += ", @COGNOME ";
                mAddParameter(command, "@COGNOME", u.cognome);
            }

            if (!String.IsNullOrEmpty(u.email))
            {
                mStrSQL += ",EMAIL ";
                strSQLParametri += ", @EMAIL ";
                mAddParameter(command, "@EMAIL", u.email);
            }

            if (!String.IsNullOrEmpty(u.login))
            {
                mStrSQL += ",MY_LOGIN ";
                strSQLParametri += ", @MY_LOGIN ";
                mAddParameter(command, "@MY_LOGIN", u.login);
            }

            if (!String.IsNullOrEmpty(u.password))
            {
                mStrSQL += ",MY_PASSWORD";
                strSQLParametri += ", @MY_PASSWORD ";
                //mAddParameter(command, "@MY_PASSWORD", u.password);
                mAddParameter(command, "@MY_PASSWORD", MyManagerCSharp.SecurityManager.getMD5Hash(u.password));
            }

            if (!String.IsNullOrEmpty(u.indirizzo))
            {
                mStrSQL += ",INDIRIZZO ";
                strSQLParametri += " ,@INDIRIZZO ";
                mAddParameter(command, "@INDIRIZZO", u.indirizzo);
            }

            if (!String.IsNullOrEmpty(u.telefono))
            {
                mStrSQL += ",TELEFONO ";
                strSQLParametri += ", @TELEFONO ";
                mAddParameter(command, "@TELEFONO", u.telefono);
            }

            if (!String.IsNullOrEmpty(u.numero_civico))
            {
                mStrSQL += ",NUMERO_CIVICO ";
                strSQLParametri += ", @NUMERO_CIVICO ";
                mAddParameter(command, "@NUMERO_CIVICO", u.numero_civico);
            }

            //if (!String.IsNullOrEmpty(u.citta)) {
            //    mStrSQL += ",CITTA ";
            //    strSQLParametri += ", @CITTA ";
            //    mAddParameter(command, "@CITTA", u.citta);
            //}

            if (u.isEnabled != null)
            {
                mStrSQL += ",is_enabled ";
                strSQLParametri += ", @IS_ENABLED ";
                mAddParameter(command, "@IS_ENABLED", u.isEnabled);
            }

            if (!String.IsNullOrEmpty(u.cap))
            {
                mStrSQL += ",CAP ";
                strSQLParametri += ", @CAP ";
                mAddParameter(command, "@CAP", u.cap);
            }

            if (u.havePhoto != null)
            {
                mStrSQL += ",PHOTO ";
                strSQLParametri += ", @PHOTO ";
                mAddParameter(command, "@PHOTO", u.havePhoto);
            }

            if (u.dataDiNascita != null && u.dataDiNascita != DateTime.MinValue)
            {
                mStrSQL += ",date_of_birth ";
                strSQLParametri += ", @data_nascita ";
                mAddParameter(command, "@data_nascita", u.dataDiNascita);
            }

            //if (!String.IsNullOrEmpty(u.cittaNascita)) {
            //    mStrSQL += ",CITTA_NASCITA ";
            //    strSQLParametri += ", @CITTA_NASCITA ";
            //    mAddParameter(command, "@CITTA_NASCITA", u.cittaNascita);
            //}

            if (!String.IsNullOrEmpty(u.mobile))
            {
                mStrSQL += ",MOBILE ";
                strSQLParametri += ", @MOBILE ";
                mAddParameter(command, "@MOBILE", u.mobile);
            }

            if (!String.IsNullOrEmpty(u.codiceFiscale))
            {
                mStrSQL += ",CODICE_FISCALE ";
                strSQLParametri += ", @CODICEFISCALE ";
                mAddParameter(command, "@CODICEFISCALE", u.codiceFiscale);
            }

            if (!String.IsNullOrEmpty(u.sesso))
            {
                mStrSQL += ",SESSO ";
                strSQLParametri += ", @SESSO ";
                mAddParameter(command, "@SESSO", u.sesso);
            }

            //'Giugno 2007
            if (!String.IsNullOrEmpty(u.http))
            {
                mStrSQL += ",HTTP ";
                strSQLParametri += ", @HTTP ";
                mAddParameter(command, "@HTTP", u.http);
            }

            if (!String.IsNullOrEmpty(u.fax))
            {
                mStrSQL += ",FAX ";
                strSQLParametri += ", @FAX ";
                mAddParameter(command, "@FAX", u.fax);
            }

            if (!String.IsNullOrEmpty(u.regione))
            {
                mStrSQL += ",REGIONE ";
                strSQLParametri += ", @REGIONE ";
                mAddParameter(command, "@REGIONE", u.regione);
            }

            if (!String.IsNullOrEmpty(u.provincia))
            {
                mStrSQL += ",PROVINCIA ";
                strSQLParametri += ", @PROVINCIA ";
                mAddParameter(command, "@PROVINCIA", u.provincia);
            }

            if (!String.IsNullOrEmpty(u.comune))
            {
                mStrSQL += ", COMUNE ";
                strSQLParametri += ", @COMUNE ";
                mAddParameter(command, "@COMUNE", u.comune);
            }

            if (u.regioneId != null && u.regioneId != -1)
            {
                mStrSQL += ",REGIONE_ID ";
                strSQLParametri += ", @REGIONE_ID ";
                mAddParameter(command, "@REGIONE_ID", u.regioneId);
            }

            //' If _provinciaId <> -1 ){
            if (!String.IsNullOrEmpty(u.provinciaId))
            {
                mStrSQL += ",PROVINCIA_ID ";
                strSQLParametri += ", @PROVINCIA_ID ";
                mAddParameter(command, "@PROVINCIA_ID", u.provinciaId);
            }

            //'If _comuneId <> -1 ){
            if (!String.IsNullOrEmpty(u.comuneId))
            {
                mStrSQL += ", COMUNE_ID ";
                strSQLParametri += ", @COMUNE_ID ";
                mAddParameter(command, "@COMUNE_ID", u.comuneId);
            }

            //if (!String.IsNullOrEmpty(u.profiloId))
            //{
            //    mStrSQL += ", PROFILO_ID ";
            //    strSQLParametri += ", @PROFILO_ID ";
            //    mAddParameter(command, "@PROFILO_ID", u.profiloId);
            //}
            //else
            //{
            //    mStrSQL += ", PROFILO_ID ";
            //    strSQLParametri += ", NULL ";
            //}

            if (u.customerId != null && u.customerId != -1)
            {
                mStrSQL += ", customer_id ";
                strSQLParametri += ", @CUSTOMER_ID ";
                mAddParameter(command, "@CUSTOMER_ID", u.customerId);
            }

            if (u.SID != null)
            {
                mStrSQL += ", SID ";
                strSQLParametri += ", @SID ";
                mAddParameter(command, "@SID", u.SID.ToString());
            }


            command.CommandText = mStrSQL + " ) " + strSQLParametri + " )";

            //If test_mode = True) {
            //    Me.mTransactionBegin()
            //    mExecuteNoQuery(command)
            //    Me.mTransactionRollback()
            //    Return -1
            //}

            mExecuteNoQuery(command);

            newId = mGetIdentity();

            return newId;
        }


        public bool delete(long userId)
        {
            return delete(userId, false);
        }

        public bool delete(long userId, bool test_mode)
        {

            mTransactionBegin();
            int esito = -1;
            try
            {
                mStrSQL = "DELETE FROM UtenteGruppo WHERE user_id = " + userId;
                mExecuteNoQuery(mStrSQL);


                mStrSQL = "DELETE FROM UtenteProfilo WHERE user_id = " + userId;
                mExecuteNoQuery(mStrSQL);

                //Cancello l'integrità referenziale sull DB tra la tabella dei Logs e la tabella Utente in modo da poter conservare i log degli utenti eliminati
                //mStrSQL = "DELETE FROM MyLogUser WHERE user_id = " + userId;
                //mExecuteNoQuery(mStrSQL);

                mStrSQL = "DELETE FROM UTENTE WHERE user_id = " + userId;

                esito = mExecuteNoQuery(mStrSQL);

                if (test_mode)
                {
                    mTransactionRollback();
                }
                else
                {
                    mTransactionCommit();
                }

            }
            catch (Exception ex)
            {
                mTransactionRollback();
                throw ex;
            }

            return esito == 1;
        }



        public List<Models.MyProfile> getProfili()
        {

            List<Models.MyProfile> risultato;
            risultato = new List<Models.MyProfile>();

            mStrSQL = "SELECT * FROM PROFILO ORDER BY NOME";

            mDt = mFillDataTable(mStrSQL);

            foreach (DataRow row in mDt.Rows)
            {
                risultato.Add(new Models.MyProfile(row));
            }

            return risultato;
        }


        public Models.MyProfile getProfilo(string profiloId)
        {
            mStrSQL = "SELECT * FROM PROFILO WHERE profilo_id = '" + profiloId + "'";

            mDt = mFillDataTable(mStrSQL);

            if (mDt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException("id > 0");
            }


            if (mDt.Rows.Count == 0)
            {
                return null;
            }


            Models.MyProfile p;
            p = new Models.MyProfile(mDt.Rows[0]);

            return p;

        }

        public bool setProfili(Models.MyUser u)
        {

            mStrSQL = "select t2.* from UtenteProfilo as t1 left join profilo  as t2 on (t1.profilo_id = t2.profilo_id) WHERE t1.user_id = " + u.userId;

            mDt = mFillDataTable(mStrSQL);


            List<Models.MyProfile> listaProfili = new List<Models.MyProfile>();

            Models.MyProfile p;

            foreach (DataRow row in mDt.Rows)
            {
                p = new Models.MyProfile(row);
                listaProfili.Add(p);
            }


            u.Profili = listaProfili;

            return true;
        }



        //public bool setGroupsWithoutAdministrators(Models.MyUser u)
        //{
        //    GroupManager gManager = new GroupManager(this.mConnection);
        //    return gManager.setGroups(u, true);
        //}

        public bool setCustomer(Models.MyUser u)
        {
            CustomerManager cManager = new CustomerManager(this.mConnection);
            return cManager.set(u);
        }


        public bool setGroups(Models.MyUser u)
        {
            GroupManager gManager = new GroupManager(this.mConnection);
            return gManager.setGroups(u);
        }


        public bool setRoles(Models.MyUser u)
        {
            GroupManager gManager = new GroupManager(this.mConnection);
            return gManager.setRoles(u);
        }



        public bool update(Models.MyUser u)
        {

            mStrSQL = "UPDATE UTENTE SET DATE_MODIFIED = GetDate()  ";

            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();


            if (!String.IsNullOrEmpty(u.email))
            {
                mStrSQL += " ,EMAIL = @EMAIL ";
                mAddParameter(command, "@EMAIL", u.email);
            }

            if (!String.IsNullOrEmpty(u.login))
            {
                mStrSQL += " ,MY_LOGIN = @LOGIN ";
                mAddParameter(command, "@LOGIN", u.login);
            }

            if (!String.IsNullOrEmpty(u.nome))
            {
                mStrSQL += ",NOME = @NOME ";
                mAddParameter(command, "@NOME", u.nome);
            }


            if (!String.IsNullOrEmpty(u.cognome))
            {
                mStrSQL += ",COGNOME = @COGNOME ";
                mAddParameter(command, "@COGNOME", u.cognome);
            }

            if (!String.IsNullOrEmpty(u.indirizzo))
            {
                mStrSQL += ",INDIRIZZO = @INDIRIZZO ";
                mAddParameter(command, "@INDIRIZZO", u.indirizzo);
            }

            if (!String.IsNullOrEmpty(u.telefono))
            {
                mStrSQL += ",TELEFONO = @TELEFONO ";
                mAddParameter(command, "@TELEFONO", u.telefono);
            }

            if (!String.IsNullOrEmpty(u.numero_civico))
            {
                mStrSQL += ",NUMERO_CIVICO = @NUMERO_CIVICO ";
                mAddParameter(command, "@NUMERO_CIVICO", u.numero_civico);
            }


            if (!String.IsNullOrEmpty(u.cap))
            {
                mStrSQL += ",CAP = @CAP ";
                mAddParameter(command, "@CAP", u.cap);
            }


            if (!String.IsNullOrEmpty(u.mobile))
            {
                mStrSQL += ",MOBILE = @MOBILE ";
                mAddParameter(command, "@MOBILE", u.mobile);
            }

            if (!String.IsNullOrEmpty(u.codiceFiscale))
            {
                mStrSQL += ",CODICE_FISCALE = @CODICEFISCALE ";
                mAddParameter(command, "@CODICEFISCALE", u.codiceFiscale);
            }

            if (!String.IsNullOrEmpty(u.sesso))
            {
                mStrSQL += ",SESSO  = @SESSO ";
                mAddParameter(command, "@SESSO", u.sesso);
            }

            if (!String.IsNullOrEmpty(u.http))
            {
                mStrSQL += ",HTTP = @HTTP ";
                mAddParameter(command, "@HTTP", u.http);
            }

            if (!String.IsNullOrEmpty(u.fax))
            {
                mStrSQL += ",FAX = @FAX ";
                mAddParameter(command, "@FAX", u.fax);
            }

            if (!String.IsNullOrEmpty(u.regione))
            {
                mStrSQL += ",REGIONE = @REGIONE ";
                mAddParameter(command, "@REGIONE", u.regione);
            }

            if (!String.IsNullOrEmpty(u.provincia))
            {
                mStrSQL += ",PROVINCIA = @PROVINCIA ";
                mAddParameter(command, "@PROVINCIA", u.provincia);
            }

            if (!String.IsNullOrEmpty(u.comune))
            {
                mStrSQL += ", COMUNE = @COMUNE ";
                mAddParameter(command, "@COMUNE", u.comune);
            }

            if (u.regioneId != null && u.regioneId != -1)
            {
                mStrSQL += ",REGIONE_ID = @REGIONE_ID ";
                mAddParameter(command, "@REGIONE_ID", u.regioneId);
            }

            //' If _provinciaId <> -1 ){
            if (!String.IsNullOrEmpty(u.provinciaId))
            {
                mStrSQL += ",PROVINCIA_ID = @PROVINCIA_ID ";
                mAddParameter(command, "@PROVINCIA_ID", u.provinciaId);
            }

            //'If _comuneId <> -1 ){
            if (!String.IsNullOrEmpty(u.comuneId))
            {
                mStrSQL += ", COMUNE_ID = @COMUNE_ID ";
                mAddParameter(command, "@COMUNE_ID", u.comuneId);
            }


            //if (!String.IsNullOrEmpty(u.profiloId))
            //{
            //    mStrSQL += ", PROFILO_ID = @PROFILO_ID ";
            //    mAddParameter(command, "@PROFILO_ID", u.profiloId);
            //}
            //else
            //{
            //    mStrSQL += ", PROFILO_ID = NULL ";
            //}


            if (u.dataDiNascita != null && u.dataDiNascita != DateTime.MinValue)
            {
                mStrSQL += ", date_of_birth = @date_of_birth ";
                mAddParameter(command, "@date_of_birth", u.dataDiNascita);
            }

            mStrSQL += ", is_enabled = @IS_ENABLED ";
            mAddParameter(command, "@IS_ENABLED", u.isEnabled);

            if (u.customerId != null && u.customerId != -1)
            {
                mStrSQL += ", customer_id = " + u.customerId;
            }


            mStrSQL += " WHERE USER_ID=" + u.userId;

            command.CommandText = mStrSQL;

            mExecuteNoQuery(command);

            return true;
        }





        public long getUserIdFromLogin(string login)
        {
            return getUserIdFromLoginAndEmail(login, "");
        }

        public long getUserIdFromLoginAndEmail(string login, string email)
        {

            mStrSQL = "select USER_ID from  UTENTE  where UPPER(MY_LOGIN)= @myLogin ";


            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();

            mAddParameter(command, "@myLogin", login.ToUpper().Trim());

            if (!String.IsNullOrEmpty(email))
            {
                mStrSQL += " and UPPER(EMAIL)= @myEmail ";
                mAddParameter(command, "@myEmail", email.ToUpper().Trim());
            }

            command.CommandText = mStrSQL;
            mDt = mFillDataTable(command);

            if (mDt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException("Login duplicata");
            }

            if (mDt.Rows.Count == 0)
            {
                //throw new MyManagerCSharp.MyException("La Login o l'e-mail inserite non sono corrette");
                return -1;
            }


            return long.Parse(mDt.Rows[0]["USER_ID"].ToString());
        }


        public string getLogin(long userId)
        {
            mStrSQL = "select MY_LOGIN  from  UTENTE  where USER_ID = " + userId;
            return mExecuteScalar(mStrSQL);
        }



        public string resetPassword(long userId)
        {
            string passwordGenerata;
            passwordGenerata = MyManagerCSharp.PasswordManager.Generate(10).Trim();

            while (!MyManagerCSharp.RegularExpressionManager.isStrongPassword(passwordGenerata))
            {
                passwordGenerata = MyManagerCSharp.PasswordManager.Generate(10).Trim();
            }


            mStrSQL = "UPDATE UTENTE SET MY_PASSWORD= @MY_Password , DATE_MODIFIED_PASSWORD = GetDate() WHERE USER_ID=" + userId;

            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();

            command.CommandText = mStrSQL;
            try
            {

                Debug.WriteLine(String.Format("psd:{0} MD5:{1} ", passwordGenerata, MyManagerCSharp.SecurityManager.getMD5Hash(passwordGenerata)));


                // Debug.WriteLine(String.Format("psd:{0} VB_MD5:{1} ", passwordGenerata, MyManager.SecurityManager.getMD5Hash(passwordGenerata)));

                mAddParameter(command, "@MY_Password", MyManagerCSharp.SecurityManager.getMD5Hash(passwordGenerata));
                mExecuteNoQuery(command);

                //Dim managerLogUser As New MyManager.LogUserManager(Me.mConnection)
                //managerLogUser.insert(userId, MyManager.LogUserManager.LogType.ResetPassword)
            }
            catch (Exception ex)
            {
                throw new MyManagerCSharp.MyException("Errore nella funzione di 'Reset Password", ex);
            }

            return passwordGenerata;
        }








        public bool updatePassword(long userId, string newPassword)
        {
            mStrSQL = "UPDATE UTENTE SET MY_PASSWORD = @MY_PASSWORD " +
                                                   ", DATE_MODIFIED = GetDate() " +
                                                    " WHERE USER_ID=" + userId;

            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();
            command.CommandText = mStrSQL;

            mAddParameter(command, "@MY_PASSWORD", MyManagerCSharp.SecurityManager.getMD5Hash(newPassword.Trim()));

            mExecuteNoQuery(command);

            // Dim managerLogUser As New MyManager.LogUserManager(Me.mConnection)
            //managerLogUser.insert(userId, MyManager.LogUserManager.LogType.UpdatePassword)
            return true;
        }



        public bool updateSID(long userId, System.Security.Principal.SecurityIdentifier sid)
        {
            mStrSQL = "UPDATE UTENTE SET SID = @SID , DATE_MODIFIED = GetDate() WHERE USER_ID=" + userId;

            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();
            command.CommandText = mStrSQL;

            mAddParameter(command, "@SID", sid.ToString());

            mExecuteNoQuery(command);

            // Dim managerLogUser As New MyManager.LogUserManager(Me.mConnection)
            //managerLogUser.insert(userId, MyManager.LogUserManager.LogType.UpdatePassword)
            return true;
        }



        public bool updateIsEnabled(long userId, bool isEnabled)
        {
            mStrSQL = "UPDATE UTENTE SET IS_ENABLED = @IS_ENABLED " +
                                                   ", DATE_MODIFIED = GetDate() " +
                                                    " WHERE USER_ID=" + userId;

            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();
            command.CommandText = mStrSQL;

            mAddParameter(command, "@IS_ENABLED", isEnabled);

            mExecuteNoQuery(command);

            // Dim managerLogUser As New MyManager.LogUserManager(Me.mConnection)
            //managerLogUser.insert(userId, MyManager.LogUserManager.LogType.UpdatePassword)
            return true;
        }

        public bool updateEmail(long userId, string email)
        {
            mStrSQL = "UPDATE UTENTE SET EMAIL = @EMAIL " +
                                                   ", DATE_MODIFIED = GetDate() " +
                                                    " WHERE USER_ID=" + userId;

            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();
            command.CommandText = mStrSQL;

            if (String.IsNullOrEmpty(email))
            {
                mAddParameter(command, "@EMAIL", DBNull.Value);
            }
            else
            {
                mAddParameter(command, "@EMAIL", email.Trim().ToLower());
            }

            mExecuteNoQuery(command);

            // Dim managerLogUser As New MyManager.LogUserManager(Me.mConnection)
            //managerLogUser.insert(userId, MyManager.LogUserManager.LogType.UpdatePassword)
            return true;
        }

        public bool updateEmail(System.Security.Principal.SecurityIdentifier sid, string email)
        {
            mStrSQL = "UPDATE UTENTE SET EMAIL = @EMAIL " +
                                                   ", DATE_MODIFIED = GetDate() " +
                                                    " WHERE SID = '" + sid.Value + "'";

            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();
            command.CommandText = mStrSQL;

            mAddParameter(command, "@EMAIL", email.Trim().ToLower());

            mExecuteNoQuery(command);

            // Dim managerLogUser As New MyManager.LogUserManager(Me.mConnection)
            //managerLogUser.insert(userId, MyManager.LogUserManager.LogType.UpdatePassword)
            return true;
        }


        public bool updateCustomerId(long userId, long? customerId)
        {
            if (customerId == null)
            {
                mStrSQL = "UPDATE UTENTE SET CUSTOMER_ID = NULL , DATE_MODIFIED = GetDate()  WHERE USER_ID=" + userId;
            }
            else
            {
                mStrSQL = "UPDATE UTENTE SET CUSTOMER_ID = " + customerId + ", DATE_MODIFIED = GetDate()  WHERE USER_ID=" + userId;
            }

            mExecuteNoQuery(mStrSQL);

            // Dim managerLogUser As New MyManager.LogUserManager(Me.mConnection)
            //managerLogUser.insert(userId, MyManager.LogUserManager.LogType.UpdatePassword)
            return true;
        }



        public List<MyManagerCSharp.Models.MyItem> getAutoCompleteLogin(string valore)
        {
            List<MyManagerCSharp.Models.MyItem> risultato = new List<MyManagerCSharp.Models.MyItem>();

            mStrSQL = "select top 7 user_id, my_login from utente where my_login like @VALORE order by my_login";

            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();
            command.CommandText = mStrSQL;

            mAddParameter(command, "@VALORE", "%" + valore + "%");

            mDt = mFillDataTable(command);

            MyManagerCSharp.Models.MyItem item;
            foreach (DataRow row in mDt.Rows)
            {
                item = new MyManagerCSharp.Models.MyItem(row["user_id"].ToString(), row["my_login"].ToString());

                risultato.Add(item);
            }

            return risultato;
        }






        public String updateEmailChanging(long userId, String email)
        {
            string codiceAttivazioneEmail;
            //codiceAttivazioneEmail = GeneraCodiceRandom()
            codiceAttivazioneEmail = Guid.NewGuid().ToString();

            mStrSQL = "UPDATE UTENTE SET EMAIL_CHANGING = @EMAIL " +
                                                 ", EMAIL_ACTIVATION_CODE = @codiceAttivazioneEmail " +
                                                 " WHERE USER_ID=" + userId;

            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();
            command.CommandText = mStrSQL;

            mAddParameter(command, "@EMAIL", email.Trim().ToLower());
            mAddParameter(command, "@codiceAttivazioneEmail", codiceAttivazioneEmail);
            mExecuteNoQuery(command);

            return codiceAttivazioneEmail;
        }

        public void updateEmailChanged(long userId, String email)
        {
            mStrSQL = "UPDATE UTENTE SET EMAIL = @EMAIL " +
                                                ", EMAIL_ACTIVATION_CODE = null " +
                                                ", EMAIL_CHANGING = null " +
                                                " WHERE USER_ID=" + userId;
            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();
            command.CommandText = mStrSQL;

            mAddParameter(command, "@EMAIL", email.Trim().ToLower());

            mExecuteNoQuery(command);
        }


        public String getNewEmailChanging(String activationCodeEmail, long userId)
        {
            mStrSQL = "SELECT EMAIL_CHANGING FROM UTENTE WHERE  (DATE_DELETED IS NULL)  AND  USER_ID = " + userId + " and EMAIL_ACTIVATION_CODE = '" + activationCodeEmail.Replace("'", "''") + "'";
            return (mExecuteScalar(mStrSQL)).ToString();
        }

    }

}
