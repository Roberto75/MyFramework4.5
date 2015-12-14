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
            mStrSQL = "DELETE FROM MYLOG ";

            string strWHERE = "";

            strWHERE = getWhereConditionByDate("date_added", days);

            mStrSQL = mStrSQL + strWHERE;

            return mExecuteNoQuery(mStrSQL);
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
            string nota = "";
            Exception e = ex;

            while (e != null)
            {
                nota += e.Message + Environment.NewLine;
                e = e.InnerException;
            }

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

            mStrSQL = "INSERT INTO MyLog ( DATE_ADDED  ";
            strSQLParametri = " VALUES ( GetDate()  ";

            System.Data.Common.DbCommand command;
            command =  mConnection.CreateCommand();
            command.Connection =  mConnection;

            if (!String.IsNullOrEmpty(nota))
            {
                mStrSQL += ",my_note ";
                strSQLParametri += ", @nota ";
                mAddParameter(command, "@nota", nota);
            }

            if (!String.IsNullOrEmpty(tipo))
            {
                mStrSQL += ",my_type ";
                strSQLParametri += ", @tipo ";
                mAddParameter(command, "@tipo", tipo);
            }

            if (!String.IsNullOrEmpty(referenceId))
            {
                mStrSQL += ",reference_id ";
                strSQLParametri += ", @referenceId ";
                mAddParameter(command, "@referenceId", referenceId);
            }

            if (!String.IsNullOrEmpty(referenceType))
            {
                mStrSQL += ",reference_type ";
                strSQLParametri += ", @referenceType ";
                mAddParameter(command, "@referenceType", referenceType);
            }

            if (level != LogManager.Level.Undefined)
            {
                mStrSQL += ",my_level ";
                strSQLParametri += ", @MY_LEVEL ";
                mAddParameter(command, "@MY_LEVEL", level.ToString());
            }

            command.CommandText = mStrSQL + " ) " + strSQLParametri + " )";
            command.CommandType = System.Data.CommandType.Text;

            mExecuteNoQuery(command);

        }


        public string[] getMyType()
        {
            mStrSQL = "select  distinct my_type from mylog order by 1";

            mDt = mFillDataTable(mStrSQL);

            List<string> risultato = new List<string>();

            foreach (System.Data.DataRow row in mDt.Rows)
            {
                risultato.Add(row[0].ToString());
            }

            return risultato.ToArray();
        }


        public void getList(MyManagerCSharp.Log.Models.SearchMyLogs model)
        {

            List<MyManagerCSharp.Log.Models.MyLog> risultato = new List<MyManagerCSharp.Log.Models.MyLog>();

            mStrSQL = " FROM MyLog ";

            System.Data.Common.DbCommand command;
            command =  mConnection.CreateCommand();

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

            mStrSQL = mStrSQL + strWHERE;

            temp = "SELECT COUNT(*) " + mStrSQL;
            command.CommandText = temp;
            model.TotalRows = int.Parse(mExecuteScalar(command));



            temp = "SELECT * " + mStrSQL + " ORDER BY " + model.Sort + " " + model.SortDir;

            if (model.PageSize > 0 && model.PageNumber >= 0)
            {

                temp += " OFFSET " + ((model.PageNumber - 1) * model.PageSize) + " ROWS FETCH NEXT " + model.PageSize + " ROWS ONLY";
            }


            command.CommandText = temp;
            mDt = mFillDataTable(command);

            foreach (System.Data.DataRow row in mDt.Rows)
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

            mStrSQL = " select * FROM MyLog  where reference_id = @REFERENCEID ";


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

                mStrSQL += " AND " + temp;
            }

            mStrSQL += " ORDER BY date_added asc";

            System.Data.Common.DbCommand command;
            command =  mConnection.CreateCommand();
            mAddParameter(command, "@REFERENCEID", referenceId);

            command.CommandText = mStrSQL;
            mDt = mFillDataTable(command);

            foreach (System.Data.DataRow row in mDt.Rows)
            {
                risultato.Add(new MyManagerCSharp.Log.Models.MyLog(row));
            }

            return risultato;
        }



        public System.Data.DataTable getSummary()
        {
            // mStrSQL = "SELECT my_level , count(*) as conta from MyLog";
            // mStrSQL += getWhereConditionByDate("date_added", days);
            // mStrSQL += " group by my_level order by my_level";

            mStrSQL = " select count(*) as TOT " +
                " , my_level " +
                ",sum(case when DAY(date_added) =  DAY(GetDate()) and MONTH(date_added) =  MONTH(GetDate())  and YEAR(date_added) = YEAR(GetDate()) then 1 else 0 end) as 'Oggi' " +
                ",sum(case when DAY(date_added) =  DAY( DATEADD( day , -1 ,GETDATE() )) and MONTH(date_added) =  MONTH( DATEADD( day , -1 ,GETDATE() ))  and YEAR(date_added) = YEAR( DATEADD( day , -1 ,GETDATE() )) then 1 else 0 end) as 'Ieri'  " +
                ",sum(case when CONVERT(date, date_added )  Between CONVERT(date, GetDate() - 7)  AND  CONVERT(date, GetDate())  then 1 else 0 end) as 'Ultimi 7 giorni' " +
                ",sum(case when CONVERT(date, date_added )  Between CONVERT(date, GetDate() - 30)  AND  CONVERT(date, GetDate())  then 1 else 0 end) as 'Ultimi 30 giorni' " +
                " from mylog " +
                " group by my_level";


            return mFillDataTable(mStrSQL);
        }



        public System.Data.DataTable getSummaryV2(MyManagerCSharp.Log.LogManager.Days days)
        {

            mStrSQL = "select reference_id , my_type,MIN (date_added) as data_inizio, MAX(date_added) as data_fine " +
                ",sum(case when my_level =  'Exception' then 1 else 0 end) as 'Exception'" +
                ",sum(case when my_level =  'Error' then 1 else 0 end) as 'Error'" +
                ",sum(case when my_level =  'Warning' then 1 else 0 end) as 'Warning'" +
                ",sum(case when my_level =  'Info' then 1 else 0 end) as 'Info'" +
                " from mylog";

            mStrSQL += " WHERE (1=1)" + getWhereConditionByDate("date_added", days);

            mStrSQL += " group by reference_id,  my_type" +
                " order by MIN (date_added) desc";
            return mFillDataTable(mStrSQL);
        }

    }
}
