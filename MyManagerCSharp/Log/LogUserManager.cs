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
            ResetPassword,
            UpdatePassword,
            Download,
            NotAuthorized,
            NotAuthenticated,
            PageNotFound,
            AccountEnable,
            AccountDisable,
            AccountDelete,
            Exception,
            Error
        }


        public LogUserManager(string connectionName)
            : base(connectionName)
        {

        }

        public LogUserManager(System.Data.Common.DbConnection connection)
            : base(connection)
        {

        }
        
        public void insert(long userId, LogType tipo)
        {
            insert(userId, tipo, "", null);
        }

        public void insert(long userId, LogType tipo, System.Net.IPAddress ipAddress)
        {
            insert(userId, tipo, "", ipAddress);
        }

        public void insert(long userId, LogType tipo, string nota)
        {
            insert(userId, tipo, nota, null);
        }

        public void insert(long userId, LogType tipo, string nota, System.Net.IPAddress ipAddress)
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

            if (ipAddress != null)
            {
                _strSQL += ",ip_address ";
                strSQLParametri += ", @IP ";
                _addParameter(command, "@IP", ipAddress.ToString() );
            }

            command.CommandText = _strSQL + " ) " + strSQLParametri + " )";
            command.CommandType = System.Data.CommandType.Text;

            _executeNoQuery(command);
        }




       


    }
}
