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
            _strSQL = "select t2.my_login as Login , t1.date_added as Access, " +
                " CASE tipo  " +
                "   WHEN '" + MyManagerCSharp.Log.LogUserManager.LogType.LoginMobile.ToString() + "' THEN 'Mobile' " +
                "	ELSE '' " +
                " END as Mobile " +
                " from MyLogUser as t1 " +
                " join Utente as t2 on t1.user_id = t2.user_id " +
                " where tipo='" + MyManagerCSharp.Log.LogUserManager.LogType.Login.ToString() + "' or tipo = '" + MyManagerCSharp.Log.LogUserManager.LogType.LoginMobile.ToString() + "' " +
                " order by t2.my_login, t1.date_added";

            _dt = _fillDataTable(_strSQL);

            string content;
            content = MyManagerCSharp.CSVManager.toCSV(_dt, ";", true);

            byte[] contentByte = new byte[content.Length * sizeof(char)];
            System.Buffer.BlockCopy(content.ToCharArray(), 0, contentByte, 0, contentByte.Length);

            return contentByte;
        }



        public void getList(Models.SearchUsers model)
        {

            List<Models.MyUser> risultato;
            risultato = new List<Models.MyUser>();

            _strSQL = " FROM UTENTE as t1";

            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();



            string strWHERE = "";
            if (model.filter != null)
            {
                if (!String.IsNullOrEmpty(model.filter.nome))
                {
                    strWHERE += " AND UPPER(nome) like  @NOME";
                    _addParameter(command, "@NOME", "%" + model.filter.nome.ToUpper().Trim() + "%");
                }

                if (!String.IsNullOrEmpty(model.filter.cognome))
                {
                    strWHERE += " AND UPPER(cognome) like  @COGNOME";
                    _addParameter(command, "@COGNOME", "%" + model.filter.cognome.ToUpper().Trim() + "%");
                }

                if (!String.IsNullOrEmpty(model.filter.email))
                {
                    strWHERE += " AND UPPER(email) like  @EMAIL";
                    _addParameter(command, "@EMAIL", "%" + model.filter.email.ToUpper().Trim() + "%");
                }

                if (!String.IsNullOrEmpty(model.filter.login))
                {
                    strWHERE += " AND UPPER(my_login) like  @MY_LOGIN";
                    _addParameter(command, "@MY_LOGIN", "%" + model.filter.login.ToUpper().Trim() + "%");
                }
            }


            if (!String.IsNullOrEmpty(strWHERE))
            {
                _strSQL += " WHERE (1=1) " + strWHERE;
            }

            string temp;
            int totalRecords;

            //paginazione
            if (model.PageSize > 0 && (_connection is System.Data.SqlClient.SqlConnection))
            {
                temp = "SELECT COUNT(*) " + _strSQL;
                command.CommandText = temp;

                totalRecords = int.Parse(_executeScalar(command));
                model.TotalRows = totalRecords;
            }



            temp = _sqlElencoUtenti + _strSQL + " ORDER BY " + model.Sort + " " + model.SortDir;


            if (model.PageSize > 0 && (_connection is System.Data.SqlClient.SqlConnection))
            {
                temp += " OFFSET " + ((model.PageNumber - 1) * model.PageSize) + " ROWS FETCH NEXT " + model.PageSize + " ROWS ONLY";
            }


            command.CommandText = temp;
            command.Connection = _connection;
            _dt = _fillDataTable(command);

            if (model.PageSize > 0 && !(_connection is System.Data.SqlClient.SqlConnection))
            {
                model.TotalRows = _dt.Rows.Count;

                // apply paging
                IEnumerable<DataRow> rows = _dt.AsEnumerable().Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize);
                foreach (DataRow row in rows)
                {
                    risultato.Add(new Models.MyUser(row, Models.MyUser.SelectFileds.Lista));
                }
            }
            else
            {
                foreach (DataRow row in _dt.Rows)
                {
                    risultato.Add(new Models.MyUser(row, Models.MyUser.SelectFileds.Lista));
                }
            }

            model.Utenti = risultato;
        }



        public Models.MyUser getUser(long id)
        {
            _strSQL = "SELECT * FROM UTENTE WHERE user_id = " + id;
            _dt = _fillDataTable(_strSQL);

            if (_dt.Rows.Count == 0)
            {
                return null;
            }

            if (_dt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException("id > 0");
            }

            Models.MyUser u;
            u = new Models.MyUser(_dt.Rows[0], Models.MyUser.SelectFileds.Full);
            //u = new Models.MyUser(_dt.Rows[0]);

            return u;
        }


        public Models.MyUser getUserFromSID(System.Security.Principal.SecurityIdentifier sid)
        {

            _strSQL = "SELECT * FROM UTENTE WHERE SID = '" + sid.ToString() + "'";

            _dt = _fillDataTable(_strSQL);

            if (_dt.Rows.Count == 0)
            {
                return null;
            }

            if (_dt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException("id > 0");
            }

            Models.MyUser u;
            u = new Models.MyUser(_dt.Rows[0], Models.MyUser.SelectFileds.Full);
            //u = new Models.MyUser(_dt.Rows[0]);

            return u;
        }

        public long getUserIdFromSID(System.Security.Principal.SecurityIdentifier sid)
        {
            string temp;
            temp = _executeScalar("SELECT user_id FROM UTENTE WHERE SID = '" + sid.ToString() + "'");

            if (String.IsNullOrEmpty(temp))
            {
                return -1;
            }

            return long.Parse(temp);
        }



        public int addLoginSucces(System.Security.Principal.SecurityIdentifier sid, string login, string ip)
        {
            _strSQL = "update UTENTE set date_last_login= GetDate() , login_success= login_success +1 , ip_address = '" + ip + "' where UPPER(SID) = @SID ";

            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();
            command.CommandText = _strSQL;

            _addParameter(command, "@SID", sid.ToString().ToUpper());

            try
            {
                int esito;
                esito = _executeNoQuery(command);

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
            _strSQL = "update UTENTE set date_last_login= GetDate() , login_success= login_success +1 , [date_previous_login] = date_last_login ";

            if (!String.IsNullOrEmpty(ip))
            {
                _strSQL += ", ip_address = '" + ip + "'";
            }
            _strSQL += " WHERE user_id = " + userId;

            _executeNoQuery(_strSQL);
        }

        public long isAuthenticated(string myLogin, string myPassword)
        {
            return isAuthenticated(myLogin, myPassword, "");
        }

        public long isAuthenticated(string myLogin, string myPassword, string ip)
        {
            long userId = -1;

            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();

            _strSQL = "select user_id, is_enabled, my_password, email from utente where UPPER(my_login) = @mylogin ";
            _addParameter(command, "@mylogin", myLogin.ToUpper());
            command.CommandText = _strSQL;

            try
            {
                _dt = _fillDataTable(command);
            }
            catch (Exception ex)
            {
                throw new MyManagerCSharp.MyException("isAuthenticated", ex);
            }


            // Controllo se l'utente è censito
            if (_dt.Rows.Count == 0)
            {
                //utente non è censito
                throw new MyManagerCSharp.MyException(MyManagerCSharp.MyException.ErrorNumber.LoginPasswordErrati);
            }

            if (_dt.Rows.Count > 1)
            {
                //utente non è censito
                throw new MyManagerCSharp.MyException(MyManagerCSharp.MyException.ErrorNumber.LoginPasswordErrati);
            }

            userId = long.Parse(_dt.Rows[0]["USER_ID"].ToString());

            if (userId == -1)
            {

            }

            if (!bool.Parse(_dt.Rows[0]["is_enabled"].ToString()))
            {
                throw new MyManagerCSharp.MyException(MyManagerCSharp.MyException.ErrorNumber.UtenteDisabilitato);
            }


            string passwordMD5;
            passwordMD5 = MyManagerCSharp.SecurityManager.getMD5Hash(myPassword.Trim());

            //Debug.WriteLine(String.Format("psw:{0} MD5:{1} USER:{2} ", myPassword.Trim(), passwordMD5, _dt.Rows[0]["my_password"]));


            if (!passwordMD5.Equals(_dt.Rows[0]["my_password"]))
            {
                _strSQL = "update UTENTE set login_failure=login_failure +1 where user_id=  " + userId;
                _executeNoQuery(_strSQL);
                throw new MyManagerCSharp.MyException(MyManagerCSharp.MyException.ErrorNumber.LoginPasswordErrati);
            }

            // TUTTO OK!!!!
            //metto i dati dell'utente in sessione 
            addLoginSucces(userId, ip);

            return userId;
        }


        public string getRoles(long userId)
        {
            _strSQL = "select t1.ruolo_id from ruolo as t1 " +
                " join GruppoRuolo as t2 on t1.ruolo_id = t2.ruolo_id " +
                " join UtenteGruppo as t3 on t2.gruppo_id = t3.gruppo_id " +
                " where t3.user_id = " + userId;

            _dt = _fillDataTable(_strSQL);

            string roles;
            roles = "";
            foreach (DataRow row in _dt.Rows)
            {
                roles += row[0] + ";";
            }

            return roles;
        }


        public List<Models.MyRole> getRoles()
        {
            List<Models.MyRole> risultato = new List<Models.MyRole>();
            _strSQL = "select t1.ruolo_id , t1.nome from ruolo as t1  order by nome";

            _dt = _fillDataTable(_strSQL);
            Models.MyRole ruolo;
            foreach (DataRow row in _dt.Rows)
            {
                ruolo = new Models.MyRole(row);
                risultato.Add(ruolo);
            }

            return risultato;
        }


        public bool updateProfili(IEnumerable<Models.MyProfile> profili, long userId)
        {
            _strSQL = "DELETE FROM  UtenteProfilo WHERE user_id = " + userId;
            _executeNoQuery(_strSQL);

            foreach (Models.MyProfile p in profili)
            {
                _strSQL = "INSERT INTO UtenteProfilo ( date_added,  profilo_id, user_id ) VALUES ( GetDate() , '" + p.profiloId + "'," + userId + ")";
                _executeNoQuery(_strSQL);
            }

            return true;
        }



        public string getRoles(System.Security.Principal.SecurityIdentifier sid)
        {
            _strSQL = "select t1.ruolo_id from ruolo as t1 " +
                " join GruppoRuolo as t2 on t1.ruolo_id = t2.ruolo_id " +
                " join UtenteGruppo as t3 on t2.gruppo_id = t3.gruppo_id " +
                " join Utente as t4 on t3.user_id = t4.user_id " +
                " where t4.sid = '" + sid.ToString() + "'";

            _dt = _fillDataTable(_strSQL);

            string roles;
            roles = "";
            foreach (DataRow row in _dt.Rows)
            {
                roles += row[0] + ";";
            }

            return roles;
        }


        public List<MyManagerCSharp.Models.MyGroupSmall> getGroupSmall(long userId)
        {
            List<MyManagerCSharp.Models.MyGroupSmall> risultato = new List<MyManagerCSharp.Models.MyGroupSmall>();
            MyManagerCSharp.Models.MyGroupSmall gruppo;

            _strSQL = "select t2.* from UtenteGruppo as t1 left join gruppo  as t2 on (t1.gruppo_id = t2.gruppo_id) WHERE t1.user_id = " + userId;

            _dt = _fillDataTable(_strSQL);

            foreach (DataRow row in _dt.Rows)
            {
                gruppo = new MyManagerCSharp.Models.MyGroupSmall(row);
                risultato.Add(gruppo);
            }

            return risultato;
        }


        public string getProfili(long userId)
        {
            _strSQL = "select t1.profilo_id from UtenteProfilo as t1  where t1.user_id = " + userId;

            _dt = _fillDataTable(_strSQL);

            string profili;
            profili = "";
            foreach (DataRow row in _dt.Rows)
            {
                profili += row[0] + ";";
            }

            return profili;
        }

        public string getEmail(long userId)
        {
            _strSQL = "SELECT EMAIL FROM UTENTE WHERE USER_ID = " + userId;
            return _executeScalar(_strSQL);
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

            _strSQL = "INSERT INTO UTENTE ( date_added  ";
            strSQLParametri = " VALUES ( GetDate()  ";

            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();

            if (!String.IsNullOrEmpty(u.nome))
            {
                _strSQL += ",NOME ";
                strSQLParametri += ", @NOME ";
                _addParameter(command, "@NOME", u.nome);
            }

            if (!String.IsNullOrEmpty(u.cognome))
            {
                _strSQL += ",COGNOME ";
                strSQLParametri += ", @COGNOME ";
                _addParameter(command, "@COGNOME", u.cognome);
            }

            if (!String.IsNullOrEmpty(u.email))
            {
                _strSQL += ",EMAIL ";
                strSQLParametri += ", @EMAIL ";
                _addParameter(command, "@EMAIL", u.email);
            }

            if (!String.IsNullOrEmpty(u.login))
            {
                _strSQL += ",MY_LOGIN ";
                strSQLParametri += ", @MY_LOGIN ";
                _addParameter(command, "@MY_LOGIN", u.login);
            }

            if (!String.IsNullOrEmpty(u.password))
            {
                _strSQL += ",MY_PASSWORD";
                strSQLParametri += ", @MY_PASSWORD ";
                //_addParameter(command, "@MY_PASSWORD", u.password);
                _addParameter(command, "@MY_PASSWORD", MyManagerCSharp.SecurityManager.getMD5Hash(u.password));
            }

            if (!String.IsNullOrEmpty(u.indirizzo))
            {
                _strSQL += ",INDIRIZZO ";
                strSQLParametri += " ,@INDIRIZZO ";
                _addParameter(command, "@INDIRIZZO", u.indirizzo);
            }

            if (!String.IsNullOrEmpty(u.telefono))
            {
                _strSQL += ",TELEFONO ";
                strSQLParametri += ", @TELEFONO ";
                _addParameter(command, "@TELEFONO", u.telefono);
            }

            if (!String.IsNullOrEmpty(u.numero_civico))
            {
                _strSQL += ",NUMERO_CIVICO ";
                strSQLParametri += ", @NUMERO_CIVICO ";
                _addParameter(command, "@NUMERO_CIVICO", u.numero_civico);
            }

            //if (!String.IsNullOrEmpty(u.citta)) {
            //    _strSQL += ",CITTA ";
            //    strSQLParametri += ", @CITTA ";
            //    _addParameter(command, "@CITTA", u.citta);
            //}

            if (u.isEnabled != null)
            {
                _strSQL += ",is_enabled ";
                strSQLParametri += ", @IS_ENABLED ";
                _addParameter(command, "@IS_ENABLED", u.isEnabled);
            }

            if (!String.IsNullOrEmpty(u.cap))
            {
                _strSQL += ",CAP ";
                strSQLParametri += ", @CAP ";
                _addParameter(command, "@CAP", u.cap);
            }

            if (u.havePhoto != null)
            {
                _strSQL += ",PHOTO ";
                strSQLParametri += ", @PHOTO ";
                _addParameter(command, "@PHOTO", u.havePhoto);
            }

            if (u.dataDiNascita != null && u.dataDiNascita != DateTime.MinValue)
            {
                _strSQL += ",date_of_birth ";
                strSQLParametri += ", @data_nascita ";
                _addParameter(command, "@data_nascita", u.dataDiNascita);
            }

            //if (!String.IsNullOrEmpty(u.cittaNascita)) {
            //    _strSQL += ",CITTA_NASCITA ";
            //    strSQLParametri += ", @CITTA_NASCITA ";
            //    _addParameter(command, "@CITTA_NASCITA", u.cittaNascita);
            //}

            if (!String.IsNullOrEmpty(u.mobile))
            {
                _strSQL += ",MOBILE ";
                strSQLParametri += ", @MOBILE ";
                _addParameter(command, "@MOBILE", u.mobile);
            }

            if (!String.IsNullOrEmpty(u.codiceFiscale))
            {
                _strSQL += ",CODICE_FISCALE ";
                strSQLParametri += ", @CODICEFISCALE ";
                _addParameter(command, "@CODICEFISCALE", u.codiceFiscale);
            }

            if (!String.IsNullOrEmpty(u.sesso))
            {
                _strSQL += ",SESSO ";
                strSQLParametri += ", @SESSO ";
                _addParameter(command, "@SESSO", u.sesso);
            }

            //'Giugno 2007
            if (!String.IsNullOrEmpty(u.http))
            {
                _strSQL += ",HTTP ";
                strSQLParametri += ", @HTTP ";
                _addParameter(command, "@HTTP", u.http);
            }

            if (!String.IsNullOrEmpty(u.fax))
            {
                _strSQL += ",FAX ";
                strSQLParametri += ", @FAX ";
                _addParameter(command, "@FAX", u.fax);
            }

            if (!String.IsNullOrEmpty(u.regione))
            {
                _strSQL += ",REGIONE ";
                strSQLParametri += ", @REGIONE ";
                _addParameter(command, "@REGIONE", u.regione);
            }

            if (!String.IsNullOrEmpty(u.provincia))
            {
                _strSQL += ",PROVINCIA ";
                strSQLParametri += ", @PROVINCIA ";
                _addParameter(command, "@PROVINCIA", u.provincia);
            }

            if (!String.IsNullOrEmpty(u.comune))
            {
                _strSQL += ", COMUNE ";
                strSQLParametri += ", @COMUNE ";
                _addParameter(command, "@COMUNE", u.comune);
            }

            if (u.regioneId != null && u.regioneId != -1)
            {
                _strSQL += ",REGIONE_ID ";
                strSQLParametri += ", @REGIONE_ID ";
                _addParameter(command, "@REGIONE_ID", u.regioneId);
            }

            //' If _provinciaId <> -1 ){
            if (!String.IsNullOrEmpty(u.provinciaId))
            {
                _strSQL += ",PROVINCIA_ID ";
                strSQLParametri += ", @PROVINCIA_ID ";
                _addParameter(command, "@PROVINCIA_ID", u.provinciaId);
            }

            //'If _comuneId <> -1 ){
            if (!String.IsNullOrEmpty(u.comuneId))
            {
                _strSQL += ", COMUNE_ID ";
                strSQLParametri += ", @COMUNE_ID ";
                _addParameter(command, "@COMUNE_ID", u.comuneId);
            }

            //if (!String.IsNullOrEmpty(u.profiloId))
            //{
            //    _strSQL += ", PROFILO_ID ";
            //    strSQLParametri += ", @PROFILO_ID ";
            //    _addParameter(command, "@PROFILO_ID", u.profiloId);
            //}
            //else
            //{
            //    _strSQL += ", PROFILO_ID ";
            //    strSQLParametri += ", NULL ";
            //}

            if (u.customerId != null && u.customerId != -1)
            {
                _strSQL += ", customer_id ";
                strSQLParametri += ", @CUSTOMER_ID ";
                _addParameter(command, "@CUSTOMER_ID", u.customerId);
            }

            if (u.SID != null)
            {
                _strSQL += ", SID ";
                strSQLParametri += ", @SID ";
                _addParameter(command, "@SID", u.SID.ToString());
            }


            command.CommandText = _strSQL + " ) " + strSQLParametri + " )";

            //If test_mode = True) {
            //    Me._transactionBegin()
            //    _executeNoQuery(command)
            //    Me._transactionRollback()
            //    Return -1
            //}

            _executeNoQuery(command);

            newId = _getIdentity();

            return newId;
        }


        public bool delete(long userId)
        {
            _strSQL = "DELETE FROM UtenteGruppo WHERE user_id = " + userId;
            _executeNoQuery(_strSQL);


            _strSQL = "DELETE FROM UtenteProfilo WHERE user_id = " + userId;
            _executeNoQuery(_strSQL);

            //Cancello l'integrità referenziale sull DB tra la tabella dei Logs e la tabella Utente in modo da poter conservare i log degli utenti eliminati
            //_strSQL = "DELETE FROM MyLogUser WHERE user_id = " + userId;
            //_executeNoQuery(_strSQL);

            _strSQL = "DELETE FROM UTENTE WHERE user_id = " + userId;
            return _executeNoQuery(_strSQL) == 1;
        }



        public List<Models.MyProfile> getProfili()
        {

            List<Models.MyProfile> risultato;
            risultato = new List<Models.MyProfile>();

            _strSQL = "SELECT * FROM PROFILO ORDER BY NOME";

            _dt = _fillDataTable(_strSQL);

            foreach (DataRow row in _dt.Rows)
            {
                risultato.Add(new Models.MyProfile(row));
            }

            return risultato;
        }




        public Models.MyProfile getProfilo(string profiloId)
        {
            _strSQL = "SELECT * FROM PROFILO WHERE profilo_id = '" + profiloId + "'";

            _dt = _fillDataTable(_strSQL);

            if (_dt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException("id > 0");
            }


            if (_dt.Rows.Count == 0)
            {
                return null;
            }


            Models.MyProfile p;
            p = new Models.MyProfile(_dt.Rows[0]);

            return p;

        }

        public bool setProfili(Models.MyUser u)
        {

            _strSQL = "select t2.* from UtenteProfilo as t1 left join profilo  as t2 on (t1.profilo_id = t2.profilo_id) WHERE t1.user_id = " + u.userId;

            _dt = _fillDataTable(_strSQL);


            List<Models.MyProfile> listaProfili = new List<Models.MyProfile>();

            Models.MyProfile p;

            foreach (DataRow row in _dt.Rows)
            {
                p = new Models.MyProfile(row);
                listaProfili.Add(p);
            }


            u.Profili = listaProfili;

            return true;
        }


        //public bool setGroupsWithoutAdministrators(Models.MyUser u)
        //{
        //    GroupManager gManager = new GroupManager(this._connection);
        //    return gManager.setGroups(u, true);
        //}


        public bool setGroups(Models.MyUser u)
        {
            GroupManager gManager = new GroupManager(this._connection);
            return gManager.setGroups(u);
        }


        public bool setRoles(Models.MyUser u)
        {
            GroupManager gManager = new GroupManager(this._connection);
            return gManager.setRoles(u);
        }



        public bool update(Models.MyUser u)
        {

            _strSQL = "UPDATE UTENTE SET DATE_MODIFIED = GetDate()  ";

            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();


            if (!String.IsNullOrEmpty(u.email))
            {
                _strSQL += " ,EMAIL = @EMAIL ";
                _addParameter(command, "@EMAIL", u.email);
            }

            if (!String.IsNullOrEmpty(u.login))
            {
                _strSQL += " ,MY_LOGIN = @LOGIN ";
                _addParameter(command, "@LOGIN", u.login);
            }

            if (!String.IsNullOrEmpty(u.nome))
            {
                _strSQL += ",NOME = @NOME ";
                _addParameter(command, "@NOME", u.nome);
            }


            if (!String.IsNullOrEmpty(u.cognome))
            {
                _strSQL += ",COGNOME = @COGNOME ";
                _addParameter(command, "@COGNOME", u.cognome);
            }

            if (!String.IsNullOrEmpty(u.indirizzo))
            {
                _strSQL += ",INDIRIZZO = @INDIRIZZO ";
                _addParameter(command, "@INDIRIZZO", u.indirizzo);
            }

            if (!String.IsNullOrEmpty(u.telefono))
            {
                _strSQL += ",TELEFONO = @TELEFONO ";
                _addParameter(command, "@TELEFONO", u.telefono);
            }

            if (!String.IsNullOrEmpty(u.numero_civico))
            {
                _strSQL += ",NUMERO_CIVICO = @NUMERO_CIVICO ";
                _addParameter(command, "@NUMERO_CIVICO", u.numero_civico);
            }


            if (!String.IsNullOrEmpty(u.cap))
            {
                _strSQL += ",CAP = @CAP ";
                _addParameter(command, "@CAP", u.cap);
            }


            if (!String.IsNullOrEmpty(u.mobile))
            {
                _strSQL += ",MOBILE = @MOBILE ";
                _addParameter(command, "@MOBILE", u.mobile);
            }

            if (!String.IsNullOrEmpty(u.codiceFiscale))
            {
                _strSQL += ",CODICE_FISCALE = @CODICEFISCALE ";
                _addParameter(command, "@CODICEFISCALE", u.codiceFiscale);
            }

            if (!String.IsNullOrEmpty(u.sesso))
            {
                _strSQL += ",SESSO  = @SESSO ";
                _addParameter(command, "@SESSO", u.sesso);
            }

            if (!String.IsNullOrEmpty(u.http))
            {
                _strSQL += ",HTTP = @HTTP ";
                _addParameter(command, "@HTTP", u.http);
            }

            if (!String.IsNullOrEmpty(u.fax))
            {
                _strSQL += ",FAX = @FAX ";
                _addParameter(command, "@FAX", u.fax);
            }

            if (!String.IsNullOrEmpty(u.regione))
            {
                _strSQL += ",REGIONE = @REGIONE ";
                _addParameter(command, "@REGIONE", u.regione);
            }

            if (!String.IsNullOrEmpty(u.provincia))
            {
                _strSQL += ",PROVINCIA = @PROVINCIA ";
                _addParameter(command, "@PROVINCIA", u.provincia);
            }

            if (!String.IsNullOrEmpty(u.comune))
            {
                _strSQL += ", COMUNE = @COMUNE ";
                _addParameter(command, "@COMUNE", u.comune);
            }

            if (u.regioneId != null && u.regioneId != -1)
            {
                _strSQL += ",REGIONE_ID = @REGIONE_ID ";
                _addParameter(command, "@REGIONE_ID", u.regioneId);
            }

            //' If _provinciaId <> -1 ){
            if (!String.IsNullOrEmpty(u.provinciaId))
            {
                _strSQL += ",PROVINCIA_ID = @PROVINCIA_ID ";
                _addParameter(command, "@PROVINCIA_ID", u.provinciaId);
            }

            //'If _comuneId <> -1 ){
            if (!String.IsNullOrEmpty(u.comuneId))
            {
                _strSQL += ", COMUNE_ID = @COMUNE_ID ";
                _addParameter(command, "@COMUNE_ID", u.comuneId);
            }


            //if (!String.IsNullOrEmpty(u.profiloId))
            //{
            //    _strSQL += ", PROFILO_ID = @PROFILO_ID ";
            //    _addParameter(command, "@PROFILO_ID", u.profiloId);
            //}
            //else
            //{
            //    _strSQL += ", PROFILO_ID = NULL ";
            //}


            if (u.dataDiNascita != null && u.dataDiNascita != DateTime.MinValue)
            {
                _strSQL += ", date_of_birth = @date_of_birth ";
                _addParameter(command, "@date_of_birth", u.dataDiNascita);
            }

            _strSQL += ", is_enabled = @IS_ENABLED ";
            _addParameter(command, "@IS_ENABLED", u.isEnabled);

            if (u.customerId != null && u.customerId != -1)
            {
                _strSQL += ", customer_id = " + u.customerId;
            }


            _strSQL += " WHERE USER_ID=" + u.userId;

            command.CommandText = _strSQL;

            _executeNoQuery(command);

            return true;
        }





        public long getUserIdFromLogin(string login)
        {
            return getUserIdFromLoginAndEmail(login, "");
        }

        public long getUserIdFromLoginAndEmail(string login, string email)
        {

            _strSQL = "select USER_ID from  UTENTE  where UPPER(MY_LOGIN)= @myLogin ";


            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();

            _addParameter(command, "@myLogin", login.ToUpper().Trim());

            if (!String.IsNullOrEmpty(email))
            {
                _strSQL += " and UPPER(EMAIL)= @myEmail ";
                _addParameter(command, "@myEmail", email.ToUpper().Trim());
            }

            command.CommandText = _strSQL;
            _dt = _fillDataTable(command);

            if (_dt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException("Login duplicata");
            }

            if (_dt.Rows.Count == 0)
            {
                //throw new MyManagerCSharp.MyException("La Login o l'e-mail inserite non sono corrette");
                return -1;
            }


            return long.Parse(_dt.Rows[0]["USER_ID"].ToString());
        }


        public string getLogin(long userId)
        {
            _strSQL = "select MY_LOGIN  from  UTENTE  where USER_ID = " + userId;
            return _executeScalar(_strSQL);
        }



        public string resetPassword(long userId)
        {
            string passwordGenerata;
            passwordGenerata = MyManagerCSharp.PasswordManager.Generate(10).Trim();

            while (!MyManagerCSharp.RegularExpressionManager.isStrongPassword(passwordGenerata))
            {
                passwordGenerata = MyManagerCSharp.PasswordManager.Generate(10).Trim();
            }


            _strSQL = "UPDATE UTENTE SET MY_PASSWORD= @MY_Password , DATE_MODIFIED_PASSWORD = GetDate() WHERE USER_ID=" + userId;

            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();

            command.CommandText = _strSQL;
            try
            {

                Debug.WriteLine(String.Format("psd:{0} MD5:{1} ", passwordGenerata, MyManagerCSharp.SecurityManager.getMD5Hash(passwordGenerata)));


                // Debug.WriteLine(String.Format("psd:{0} VB_MD5:{1} ", passwordGenerata, MyManager.SecurityManager.getMD5Hash(passwordGenerata)));

                _addParameter(command, "@MY_Password", MyManagerCSharp.SecurityManager.getMD5Hash(passwordGenerata));
                _executeNoQuery(command);

                //Dim managerLogUser As New MyManager.LogUserManager(Me._connection)
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
            _strSQL = "UPDATE UTENTE SET MY_PASSWORD = @MY_PASSWORD " +
                                                   ", DATE_MODIFIED = GetDate() " +
                                                    " WHERE USER_ID=" + userId;

            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();
            command.CommandText = _strSQL;

            _addParameter(command, "@MY_PASSWORD", MyManagerCSharp.SecurityManager.getMD5Hash(newPassword.Trim()));

            _executeNoQuery(command);

            // Dim managerLogUser As New MyManager.LogUserManager(Me._connection)
            //managerLogUser.insert(userId, MyManager.LogUserManager.LogType.UpdatePassword)
            return true;
        }



        public bool updateSID(long userId, System.Security.Principal.SecurityIdentifier sid)
        {
            _strSQL = "UPDATE UTENTE SET SID = @SID , DATE_MODIFIED = GetDate() WHERE USER_ID=" + userId;

            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();
            command.CommandText = _strSQL;

            _addParameter(command, "@SID", sid.ToString());

            _executeNoQuery(command);

            // Dim managerLogUser As New MyManager.LogUserManager(Me._connection)
            //managerLogUser.insert(userId, MyManager.LogUserManager.LogType.UpdatePassword)
            return true;
        }



        public bool updateIsEnabled(long userId, bool isEnabled)
        {
            _strSQL = "UPDATE UTENTE SET IS_ENABLED = @IS_ENABLED " +
                                                   ", DATE_MODIFIED = GetDate() " +
                                                    " WHERE USER_ID=" + userId;

            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();
            command.CommandText = _strSQL;

            _addParameter(command, "@IS_ENABLED", isEnabled);

            _executeNoQuery(command);

            // Dim managerLogUser As New MyManager.LogUserManager(Me._connection)
            //managerLogUser.insert(userId, MyManager.LogUserManager.LogType.UpdatePassword)
            return true;
        }

        public bool updateEmail(long userId, string email)
        {
            _strSQL = "UPDATE UTENTE SET EMAIL = @EMAIL " +
                                                   ", DATE_MODIFIED = GetDate() " +
                                                    " WHERE USER_ID=" + userId;

            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();
            command.CommandText = _strSQL;

            if (String.IsNullOrEmpty(email))
            {
                _addParameter(command, "@EMAIL", DBNull.Value);
            }
            else
            {
                _addParameter(command, "@EMAIL", email.Trim().ToLower());
            }

            _executeNoQuery(command);

            // Dim managerLogUser As New MyManager.LogUserManager(Me._connection)
            //managerLogUser.insert(userId, MyManager.LogUserManager.LogType.UpdatePassword)
            return true;
        }

        public bool updateEmail(System.Security.Principal.SecurityIdentifier sid, string email)
        {
            _strSQL = "UPDATE UTENTE SET EMAIL = @EMAIL " +
                                                   ", DATE_MODIFIED = GetDate() " +
                                                    " WHERE SID = '" + sid.Value + "'";

            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();
            command.CommandText = _strSQL;

            _addParameter(command, "@EMAIL", email.Trim().ToLower());

            _executeNoQuery(command);

            // Dim managerLogUser As New MyManager.LogUserManager(Me._connection)
            //managerLogUser.insert(userId, MyManager.LogUserManager.LogType.UpdatePassword)
            return true;
        }


        public List<MyManagerCSharp.Models.MyItem> getAutoCompleteLogin(string valore)
        {
            List<MyManagerCSharp.Models.MyItem> risultato = new List<MyManagerCSharp.Models.MyItem>();

            _strSQL = "select top 7 user_id, my_login from utente where my_login like @VALORE order by my_login";

            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();
            command.CommandText = _strSQL;

            _addParameter(command, "@VALORE", "%" + valore + "%");

            _dt = _fillDataTable(command);

            MyManagerCSharp.Models.MyItem item;
            foreach (DataRow row in _dt.Rows)
            {
                item = new MyManagerCSharp.Models.MyItem(row["user_id"].ToString(), row["my_login"].ToString());

                risultato.Add(item);
            }

            return risultato;
        }


    }

}
