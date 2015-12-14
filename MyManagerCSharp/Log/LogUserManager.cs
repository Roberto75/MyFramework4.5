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

            mStrSQL = "INSERT INTO MyLogUser ( DATE_ADDED , user_id , tipo";
            strSQLParametri = " VALUES ( GetDate() , " + userId + ", '" + tipo.ToString() + "' ";

            System.Data.Common.DbCommand command;
            command =  mConnection.CreateCommand();
            command.Connection =  mConnection;

            if (!String.IsNullOrEmpty(nota))
            {
                mStrSQL += ",nota ";
                strSQLParametri += ", @nota ";
                mAddParameter(command, "@nota", nota);
            }

            if (!String.IsNullOrEmpty(login))
            {
                mStrSQL += ",login ";
                strSQLParametri += ", @LOGIN ";
                mAddParameter(command, "@LOGIN", login);
            }

            if (!String.IsNullOrEmpty(controller))
            {
                mStrSQL += ",controller ";
                strSQLParametri += ", @CONTROLLER ";
                mAddParameter(command, "@CONTROLLER", controller);
            }

            if (!String.IsNullOrEmpty(action))
            {
                mStrSQL += ",action ";
                strSQLParametri += ", @ACTION ";
                mAddParameter(command, "@ACTION", action);
            }

            if (!String.IsNullOrEmpty(httpMethod))
            {
                mStrSQL += ",http_method ";
                strSQLParametri += ", @HTTP_METHOD ";
                mAddParameter(command, "@HTTP_METHOD", httpMethod);
            }
            

            if (ipAddress != null)
            {
                mStrSQL += ",ip_address ";
                strSQLParametri += ", @IP ";
                mAddParameter(command, "@IP", ipAddress.ToString());
            }

            if (sessionId != null)
            {
                mStrSQL += ",session_id ";
                strSQLParametri += ", @SESSION_ID ";
                mAddParameter(command, "@SESSION_ID", sessionId);
            }

            command.CommandText = mStrSQL + " ) " + strSQLParametri + " )";
            command.CommandType = System.Data.CommandType.Text;

            mExecuteNoQuery(command);
        }







    }
}
