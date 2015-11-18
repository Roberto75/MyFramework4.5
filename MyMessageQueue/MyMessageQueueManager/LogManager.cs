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

            _strSQL = "INSERT INTO mmq.log ( DATE_ADDED  ";
            strSQLParametri = " VALUES ( GetDate()  ";

            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();
            command.Connection = _connection;


            if (!String.IsNullOrEmpty(nota))
            {
                _strSQL += ",my_note ";
                strSQLParametri += ", @nota ";
                _addParameter(command, "@nota", nota);
            }

            _strSQL += ",message_id ";
            strSQLParametri += ", @messageID ";
            _addParameter(command, "@messageID", messageId);


            if (level != LogManager.Level.Undefined)
            {
                _strSQL += ",my_level ";
                strSQLParametri += ", @MY_LEVEL ";
                _addParameter(command, "@MY_LEVEL", (int)level);
            }

            command.CommandText = _strSQL + " ) " + strSQLParametri + " )";
            command.CommandType = System.Data.CommandType.Text;

            _executeNoQuery(command);
        }




        public List<Models.Log> getLogs(long messageId)
        {
            List<Models.Log> risultato;
            risultato = new List<Models.Log>();

            _strSQL = "SELECT * FROM  mmq.log WHERE message_id = " + messageId + " order by id";

            _dt = _fillDataTable(_strSQL);

            foreach (System.Data.DataRow row in _dt.Rows)
            {
                risultato.Add(new Models.Log(row));
            }

            return risultato;
        }

    }
}
