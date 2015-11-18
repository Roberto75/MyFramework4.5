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

        public void exception(Exception ex, string referenceId, string referenceType, string tipo)
        {
            string nota;
            nota = ex.Message;

            insert(tipo, nota, referenceId, referenceType, Level.Exception);
        }


        public void exception(string tipo, Exception ex)
        {
            string nota;
            nota = ex.Message;

            insert(tipo, nota, "", "", Level.Exception);
        }




        public void insert(string tipo, string nota, string referenceId, string referenceType, Level level)
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


            command.CommandText = temp;
            _dt = _fillDataTable(command);

            foreach (System.Data.DataRow row in _dt.Rows)
            {
                risultato.Add(new MyManagerCSharp.Log.Models.MyLog(row));
            }

            model.LogsList = risultato;

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



        public System.Data.DataTable getSummary()
        {
            //_strSQL = "SELECT my_level , count(*) as conta from MyLog";
            //_strSQL += getWhereConditionByDate("date_added", days);
            //_strSQL += " group by my_level order by my_level";

            _strSQL = " select count(*) as TOT " +
                " , my_level " +
                ",sum(case when DAY(date_added) =  DAY(GetDate()) and MONTH(date_added) =  MONTH(GetDate())  and YEAR(date_added) = YEAR(GetDate()) then 1 else 0 end) as 'Oggi' " +
                ",sum(case when DAY(date_added) =  DAY( DATEADD( day , -1 ,GETDATE() )) and MONTH(date_added) =  MONTH( DATEADD( day , -1 ,GETDATE() ))  and YEAR(date_added) = YEAR( DATEADD( day , -1 ,GETDATE() )) then 1 else 0 end) as 'Ieri'  " +
                ",sum(case when CONVERT(date, date_added )  Between CONVERT(date, GetDate() - 7)  AND  CONVERT(date, GetDate())  then 1 else 0 end) as 'Ultimi 7 giorni' " +
                ",sum(case when CONVERT(date, date_added )  Between CONVERT(date, GetDate() - 30)  AND  CONVERT(date, GetDate())  then 1 else 0 end) as 'Ultimi 30 giorni' " +
                " from mylog " +
                " group by my_level";


            return _fillDataTable(_strSQL);
        }



        public System.Data.DataTable getSummaryV2(MyManagerCSharp.Log.LogManager.Days days)
        {

            _strSQL = "select reference_id , my_type,MIN (date_added) as data_inizio, MAX(date_added) as data_fine " +
                ",sum(case when my_level =  'Exception' then 1 else 0 end) as 'Exception'" +
                ",sum(case when my_level =  'Error' then 1 else 0 end) as 'Error'" +
                ",sum(case when my_level =  'Warning' then 1 else 0 end) as 'Warning'" +
                ",sum(case when my_level =  'Info' then 1 else 0 end) as 'Info'" +
                " from mylog";

            _strSQL += " WHERE (1=1)" + getWhereConditionByDate("date_added", days);

            _strSQL += " group by reference_id,  my_type" +
                " order by MIN (date_added) desc";
            return _fillDataTable(_strSQL);
        }

    }
}
