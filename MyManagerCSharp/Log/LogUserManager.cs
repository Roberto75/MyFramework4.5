using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp.Log
{
    public class LogUserManager : ManagerDB
    {

        public enum LogType
        {
            Login,
            LoginMobile,
            Logout,
            Reset_password,
            Update_password,
            Download,
            Not_authorized,
            Not_authenticated,
            Page_not_found,
            Account_enable,
            Account_disable,
            Account_delete,
            Exception,
            Error,
            Report,
            ControllerAction,
            Access_denied
        }


        public LogUserManager(string connectionName)
            : base(connectionName)
        {

        }

        public LogUserManager(System.Data.Common.DbConnection connection)
            : base(connection)
        {

        }

        public void insert(long userId, string login, LogType tipo)
        {
            insert(userId, login, tipo, "", null, "", "", null,"");
        }

        public void insert(long userId, string login, LogType tipo, System.Net.IPAddress ipAddress)
        {
            insert(userId, login, tipo, "", ipAddress, "", "", null, "");
        }

        public void insert(long userId, string login, LogType tipo, string nota)
        {
            insert(userId, login, tipo, nota, null, "", "", null, "");
        }

        public void insert(long userId, string login, LogType tipo, string nota, System.Net.IPAddress ipAddress, string controller, string action, Guid? sessionId,  string httpMethod)
        {
            string strSQLParametri;

            _strSQL = "INSERT INTO MyLogUser ( DATE_ADDED , user_id , tipo";
            strSQLParametri = " VALUES ( GetDate() , " + userId + ", '" + tipo.ToString() + "' ";

            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();
            command.Connection = _connection;

            if (!String.IsNullOrEmpty(nota))
            {
                _strSQL += ",nota ";
                strSQLParametri += ", @nota ";
                _addParameter(command, "@nota", nota);
            }

            if (!String.IsNullOrEmpty(login))
            {
                _strSQL += ",login ";
                strSQLParametri += ", @LOGIN ";
                _addParameter(command, "@LOGIN", login);
            }

            if (!String.IsNullOrEmpty(controller))
            {
                _strSQL += ",controller ";
                strSQLParametri += ", @CONTROLLER ";
                _addParameter(command, "@CONTROLLER", controller);
            }

            if (!String.IsNullOrEmpty(action))
            {
                _strSQL += ",action ";
                strSQLParametri += ", @ACTION ";
                _addParameter(command, "@ACTION", action);
            }

            if (!String.IsNullOrEmpty(httpMethod))
            {
                _strSQL += ",http_method ";
                strSQLParametri += ", @HTTP_METHOD ";
                _addParameter(command, "@HTTP_METHOD", httpMethod);
            }
            

            if (ipAddress != null)
            {
                _strSQL += ",ip_address ";
                strSQLParametri += ", @IP ";
                _addParameter(command, "@IP", ipAddress.ToString());
            }

            if (sessionId != null)
            {
                _strSQL += ",session_id ";
                strSQLParametri += ", @SESSION_ID ";
                _addParameter(command, "@SESSION_ID", sessionId);
            }

            command.CommandText = _strSQL + " ) " + strSQLParametri + " )";
            command.CommandType = System.Data.CommandType.Text;

            _executeNoQuery(command);
        }







    }
}
