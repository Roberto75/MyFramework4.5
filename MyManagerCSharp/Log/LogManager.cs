using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp.Log
{
    public class LogManager : ManagerDB
    {

        public enum Level
        {
            Undefined,
            Debug,
            Info,
            Warning,
            Error,
            Exception
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

            strWHERE = getWhereConditionByDate("date_added", days); 

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

        public void exception(string tipo, Exception ex)
        {
            string nota;
            nota = ex.Message;

            insert(tipo, nota, "", "", Level.Exception);
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


        public void getList(MyManagerCSharp.Log.Models.SearchMyLogs model)
        {

            List<MyManagerCSharp.Log.Models.MyLog> risultato = new List<MyManagerCSharp.Log.Models.MyLog>();
           
            _strSQL = " FROM MyLog ";

            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();

            string strWHERE = " WHERE (1=1)";
            string temp;

            if (model.levelSelected != null)
            {
                temp = "( ";
                foreach (MyManagerCSharp.Log.LogManager.Level l in model.levelSelected)
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

            //switch (model.days)
            //{
            //    case Days.Oggi:
            //        //strWHERE += " AND FORMAT (date_added,'yyyy-MM-dd') = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
            //        strWHERE += String.Format(" AND (DAY({0})={1} AND  MONTH({0})={2} AND YEAR({0})={3}) ", "date_added", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
            //        break;
            //    case Days.Ieri:
            //        //strWHERE += " AND ( FORMAT (date_added,'yyyy-MM-dd') <= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' AND  FORMAT (date_added,'yyyy-MM-dd') >= '" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "')";
            //        strWHERE += String.Format(" AND (DAY({0})<{1} AND  MONTH({0})<={2} AND YEAR({0})<={3}) ", "date_added", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
            //        strWHERE += String.Format(" AND (DAY({0})>={1} AND  MONTH({0})>={2} AND YEAR({0})>={3}) ", "date_added", DateTime.Now.AddDays(-1).Day, DateTime.Now.AddDays(-1).Month, DateTime.Now.AddDays(-1).Year);
            //        break;
            //    case Days.UltimaSettimana:
            //        //strWHERE += " AND ( FORMAT (date_added,'yyyy-MM-dd') <= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' AND  FORMAT (date_added,'yyyy-MM-dd') >= '" + DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + "')";
            //        strWHERE += String.Format(" AND (DAY({0})<={1} AND  MONTH({0})<={2} AND YEAR({0})<={3}) ", "date_added", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
            //        strWHERE += String.Format(" AND (DAY({0})>={1} AND  MONTH({0})>={2} AND YEAR({0})>={3}) ", "date_added", DateTime.Now.AddDays(-7).Day, DateTime.Now.AddDays(-7).Month, DateTime.Now.AddDays(-7).Year);
            //        break;
            //    case Days.UltimoMese:
            //        //strWHERE += " AND ( FORMAT (date_added,'yyyy-MM-dd') <= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' AND  FORMAT (date_added,'yyyy-MM-dd') >= '" + DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd") + "')";
            //        strWHERE += String.Format(" AND (DAY({0})<={1} AND  MONTH({0})<={2} AND YEAR({0})<={3}) ", "date_added", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
            //        strWHERE += String.Format(" AND (DAY({0})>={1} AND  MONTH({0})>={2} AND YEAR({0})>={3}) ", "date_added", DateTime.Now.AddDays(-30).Day, DateTime.Now.AddDays(-30).Month, DateTime.Now.AddDays(-30).Year);
            //        break;
            //}

            strWHERE += getWhereConditionByDate("date_added", model.Days);

            _strSQL = _strSQL + strWHERE;

            temp = "SELECT COUNT(*) " + _strSQL;
            command.CommandText = temp;
            model.TotalRows = int.Parse(_executeScalar(command));

      

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
                risultato.Add(new MyManagerCSharp.Log.Models.MyLog(row));
            }

            model.LogsList= risultato;

        }


        public List<MyManagerCSharp.Log.Models.MyLog> getReferenceDetail(string referenceId)
        {
            return getReferenceDetail(referenceId, new List<MyManagerCSharp.Log.LogManager.Level>());
        }


        public List<MyManagerCSharp.Log.Models.MyLog> getReferenceDetail(string referenceId, List<MyManagerCSharp.Log.LogManager.Level> levels)
        {
            List<MyManagerCSharp.Log.Models.MyLog> risultato = new List<MyManagerCSharp.Log.Models.MyLog>();

            _strSQL = " select * FROM MyLog  where reference_id = @REFERENCEID ";


            if (levels.Count > 0)
            {
                string temp;
                temp = "( ";
                foreach (MyManagerCSharp.Log.LogManager.Level l in levels)
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
                risultato.Add(new MyManagerCSharp.Log.Models.MyLog(row));
            }

            return risultato;
        }

    }
}
