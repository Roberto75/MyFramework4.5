using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.MessageQueue
{
    public class LogManager : MyManagerCSharp.ManagerDB
    {
        public enum Level
        {
            Undefined = 0,
            Debug = 1,
            Info = 2,
            Warning = 3,
            Error = 4,
            Exception = 5
        }

        public LogManager(string connectionName)
            : base(connectionName)
        {

        }

        public LogManager(System.Data.Common.DbConnection connection)
            : base(connection)
        {

        }

        public void info(long messageId, string nota)
        {
            _insert(messageId, nota, Level.Info);
        }

        public void warning(long messageId, string nota)
        {
            _insert(messageId, nota, Level.Warning);
        }

        public void error(long messageId, string nota)
        {
            _insert(messageId, nota, Level.Error);
        }

        public void exception(long messageId, Exception ex)
        {
            string temp;
            temp = ex.Message;
            if (ex.InnerException != null)
            {
                temp += Environment.NewLine;
                temp += ex.InnerException.Message;
            }
            _insert(messageId, temp, Level.Exception);
        }

        private void _insert(long messageId, string nota, Level level)
        {
            string strSQLParametri;

            mStrSQL = "INSERT INTO mmq.log ( DATE_ADDED  ";
            strSQLParametri = " VALUES ( GetDate()  ";

            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();
            command.Connection = mConnection;


            if (!String.IsNullOrEmpty(nota))
            {
                mStrSQL += ",my_note ";
                strSQLParametri += ", @nota ";
                mAddParameter(command, "@nota", nota);
            }

            mStrSQL += ",message_id ";
            strSQLParametri += ", @messageID ";
            mAddParameter(command, "@messageID", messageId);


            if (level != LogManager.Level.Undefined)
            {
                mStrSQL += ",my_level ";
                strSQLParametri += ", @MY_LEVEL ";
                mAddParameter(command, "@MY_LEVEL", (int)level);
            }

            command.CommandText = mStrSQL + " ) " + strSQLParametri + " )";
            command.CommandType = System.Data.CommandType.Text;

            mExecuteNoQuery(command);
        }




        public List<Models.Log> getLogs(long messageId)
        {
            List<Models.Log> risultato;
            risultato = new List<Models.Log>();

            mStrSQL = "SELECT * FROM  mmq.log WHERE message_id = " + messageId + " order by id";

            mDt = mFillDataTable(mStrSQL);

            foreach (System.Data.DataRow row in mDt.Rows)
            {
                risultato.Add(new Models.Log(row));
            }

            return risultato;
        }

    }
}
