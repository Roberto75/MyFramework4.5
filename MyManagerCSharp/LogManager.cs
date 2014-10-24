using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp
{
    public class LogManager : ManagerDB
    {

        public enum Level
        {
            Undefined,
            Debug,
            Info,
            Warning,
            Error
        }

        public enum Days
        {
            Tutti = 0,
            Oggi = 1,
            Ieri = 2,
            UltimaSettimana = 3,
            UltimoMese = 4
        }


        public LogManager(string connectionName)
            : base(connectionName)
        {

        }

        public LogManager(System.Data.Common.DbConnection connection)
            : base(connection)
        {

        }


        public int delete(LogManager.Days days)
        {
            _strSQL = "DELETE FROM MYLOG ";

            string strWHERE = "";

            switch (days)
            {
                case Days.Oggi:
                    //strWHERE = " WHERE FORMAT (date_added,'yyyy-MM-dd') = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                    strWHERE = " WHERE FORMAT (date_added,'yyyy-MM-dd') < '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                    break;
                case Days.Ieri:
                    //strWHERE = " WHERE ( FORMAT (date_added,'yyyy-MM-dd') <= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' AND  FORMAT (date_added,'yyyy-MM-dd') >= '" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "')";
                    strWHERE = " WHERE FORMAT (date_added,'yyyy-MM-dd') < '" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "'";
                    break;
                case Days.UltimaSettimana:
                    //strWHERE = " WHERE ( FORMAT (date_added,'yyyy-MM-dd') <= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' AND  FORMAT (date_added,'yyyy-MM-dd') >= '" + DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + "')";
                    strWHERE = " WHERE FORMAT (date_added,'yyyy-MM-dd') < '" + DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + "'";
                    break;
                case Days.UltimoMese:
                    //strWHERE = " WHERE ( FORMAT (date_added,'yyyy-MM-dd') <= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' AND  FORMAT (date_added,'yyyy-MM-dd') >= '" + DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd") + "')";
                    strWHERE = " WHERE FORMAT (date_added,'yyyy-MM-dd') < '" + DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd") + "'";
                    break;
            }

            _strSQL = _strSQL + strWHERE;

            return _executeNoQuery(_strSQL);
        }


        public void info(string nota, string tipo)
        {
            insert(tipo, nota, "", "", Level.Info);
        }

        public void info(string nota, string referenceId, string referenceType, string tipo)
        {
            insert(tipo, nota, referenceId, referenceType, Level.Info);
        }

        public void warning(string nota, string tipo)
        {
            insert(tipo, nota, "", "", Level.Warning);
        }

        public void warning(string nota, string referenceId, string referenceType, string tipo)
        {
            insert(tipo, nota, referenceId, referenceType, Level.Warning);
        }

        public void error(string nota, string tipo)
        {
            insert(tipo, nota, "", "", Level.Error);
        }

        public void error(string nota, string referenceId, string referenceType, string tipo)
        {
            insert(tipo, nota, referenceId, referenceType, Level.Error);
        }

        private void insert(string tipo, string nota, string referenceId, string referenceType, Level level)
        {
            string strSQLParametri;

            _strSQL = "INSERT INTO MyLog ( DATE_ADDED  ";
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

            if (!String.IsNullOrEmpty(tipo))
            {
                _strSQL += ",my_type ";
                strSQLParametri += ", @tipo ";
                _addParameter(command, "@tipo", tipo);
            }

            if (!String.IsNullOrEmpty(referenceId))
            {
                _strSQL += ",reference_id ";
                strSQLParametri += ", @referenceId ";
                _addParameter(command, "@referenceId", referenceId);
            }

            if (!String.IsNullOrEmpty(referenceType))
            {
                _strSQL += ",reference_type ";
                strSQLParametri += ", @referenceType ";
                _addParameter(command, "@referenceType", referenceType);
            }

            if (level != LogManager.Level.Undefined)
            {
                _strSQL += ",my_level ";
                strSQLParametri += ", @MY_LEVEL ";
                _addParameter(command, "@MY_LEVEL", level.ToString());
            }

            command.CommandText = _strSQL + " ) " + strSQLParametri + " )";
            command.CommandType = System.Data.CommandType.Text;

            _executeNoQuery(command);

        }


        public string[] getMyType()
        {
            _strSQL = "select  distinct my_type from mylog order by 1";

            _dt = _fillDataTable(_strSQL);

            List<string> risultato = new List<string>();

            foreach (System.Data.DataRow row in _dt.Rows)
            {
                risultato.Add(row[0].ToString());
            }

            return risultato.ToArray();
        }


        public List<MyManagerCSharp.Models.MyLog> getList(MyManagerCSharp.Models.SearchMyLogs model, out int totalRecords)
        {

            List<MyManagerCSharp.Models.MyLog> risultato = new List<MyManagerCSharp.Models.MyLog>();


            _strSQL = " FROM MyLog ";

            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();

            string strWHERE = " WHERE (1=1)";
            string temp;

            if (model.levelSelected != null)
            {
                temp = "( ";
                foreach (MyManagerCSharp.LogManager.Level l in model.levelSelected)
                {
                    temp += "  MY_LEVEL = '" + l.ToString() + "' OR";
                }

                temp = temp.Substring(0, temp.Length - 2);

                temp += ")";

                strWHERE += " AND " + temp;
            }

            if (model.myTypeSelected != null && model.myTypeSelected.Count() != 0)
            {
                temp = "( ";
                foreach (string l in model.myTypeSelected)
                {
                    temp += "  MY_TYPE = '" + l.ToString() + "' OR";
                }

                temp = temp.Substring(0, temp.Length - 2);

                temp += ")";

                strWHERE += " AND " + temp;
            }

            switch (model.days)
            {
                case Days.Oggi:
                    strWHERE += " AND FORMAT (date_added,'yyyy-MM-dd') = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                    break;
                case Days.Ieri:
                    strWHERE += " AND ( FORMAT (date_added,'yyyy-MM-dd') <= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' AND  FORMAT (date_added,'yyyy-MM-dd') >= '" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "')";
                    break;
                case Days.UltimaSettimana:
                    strWHERE += " AND ( FORMAT (date_added,'yyyy-MM-dd') <= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' AND  FORMAT (date_added,'yyyy-MM-dd') >= '" + DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + "')";
                    break;
                case Days.UltimoMese:
                    strWHERE += " AND ( FORMAT (date_added,'yyyy-MM-dd') <= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' AND  FORMAT (date_added,'yyyy-MM-dd') >= '" + DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd") + "')";
                    break;
            }


            _strSQL = _strSQL + strWHERE;

            temp = "SELECT COUNT(*) " + _strSQL;
            command.CommandText = temp;
            totalRecords = int.Parse(_executeScalar(command));


            temp = "SELECT * " + _strSQL + " ORDER BY " + model.Sort + " " + model.SortDir;

            if (model.PageSize > 0 && model.PageNumber >= 0)
            {

                temp += " OFFSET " + ((model.PageNumber - 1) * model.PageSize) + " ROWS FETCH NEXT " + model.PageSize + " ROWS ONLY";
            }


            // Esegue la query e mette gli n record (n numero dato da model.PageNumber) in un oggetto DataTable:
            command.CommandText = temp;
            _dt = _fillDataTable(command);

            // Per ogni riga dentro il DataRow crea un oggetto VulnSmall e lo aggiunge alla collezione risultato:
            foreach (System.Data.DataRow row in _dt.Rows)
            {
                risultato.Add(new MyManagerCSharp.Models.MyLog(row));
            }

            return risultato;
        }


        public List<MyManagerCSharp.Models.MyLog> getReferenceDetail(string referenceId)
        {
            return getReferenceDetail(referenceId, new List<MyManagerCSharp.LogManager.Level>());
        }


        public List<MyManagerCSharp.Models.MyLog> getReferenceDetail(string referenceId, List<MyManagerCSharp.LogManager.Level> levels)
        {
            List<MyManagerCSharp.Models.MyLog> risultato = new List<MyManagerCSharp.Models.MyLog>();

            _strSQL = " select * FROM MyLog  where reference_id = @REFERENCEID ";


            if (levels.Count > 0)
            {
                string temp;
                temp = "( ";
                foreach (MyManagerCSharp.LogManager.Level l in levels)
                {
                    temp += "  MY_LEVEL = '" + l.ToString() + "' OR";
                }

                temp = temp.Substring(0, temp.Length - 2);

                temp += ")";

                _strSQL += " AND " + temp;
            }

            _strSQL += " ORDER BY date_added asc";

            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();
            _addParameter(command, "@REFERENCEID", referenceId);

            command.CommandText = _strSQL;
            _dt = _fillDataTable(command);

            foreach (System.Data.DataRow row in _dt.Rows)
            {
                risultato.Add(new MyManagerCSharp.Models.MyLog(row));
            }

            return risultato;
        }

    }
}
