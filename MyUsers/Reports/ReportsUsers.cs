using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MyUsers.Reports
{
    public class ReportsUsers : MyManagerCSharp.RGraph.RGraphManager
    {

        public ReportsUsers(string connectionName)
            : base(connectionName)
        {

        }


        public ReportsUsers(System.Data.Common.DbConnection connection)
            : base(connection)
        {

        }


        public MyManagerCSharp.RGraph.Models.RGraphModel getLoginByDate(string reportId, MyManagerCSharp.Log.LogManager.Days days, long userId)
        {
            mStrSQL = "select count(*) as valore, CAST(date_added AS DATE)  as label "
                + " from MyLogUSer as t1 ";

            mStrSQL += " WHERE  (t1.tipo = '" + MyManagerCSharp.Log.LogUserManager.LogType.Login.ToString() + "' or  t1.tipo = '" + MyManagerCSharp.Log.LogUserManager.LogType.LoginMobile.ToString() + "' )";

            mStrSQL += getWhereConditionByDate("date_added", days);

            if (userId != -1)
            {
                mStrSQL += " AND t1.user_id = " + userId;
            }

            mStrSQL += " GROUP BY CAST(date_added AS DATE) "
                + " ORDER BY CAST(date_added AS DATE) ";

            MyManagerCSharp.RGraph.Models.RGraphModel report = new MyManagerCSharp.RGraph.Models.RGraphModel();

            report.Id = reportId;
            report.Data = mFillDataTable(mStrSQL);
            report.Tipo = ReportType.Line;
            report.Label = LabelType.LabelAndValore;
            report.Titolo = "Numero di accessi eseguiti " + days.ToString().Replace("_", " ");
            //report.Colors = PaletteType.Palette01;

            //ShowLabels = true;

            if (days >= Days.Primo_semestre_anno_corrente)
            {
                report.Width = "1000px";
            }
            else
            {
                report.Width = "600px";
            }

            
            report.Height = "500px";

            //report.ShowFiltroData = true;
            report.EnableOnClick = true;

            getChart(report, false, false);
            //Debug.WriteLine("html" + report.Html);

            return report;
        }





        public System.Data.DataTable getLastLogin(MyManagerCSharp.Log.LogManager.Days days)
        {

            mStrSQL = "select date_last_login,  my_login, user_id  from Utente Where (1=1) ";
            mStrSQL += getWhereConditionByDate("date_last_login", days);
            mStrSQL += "order by date_last_login desc";

            return mFillDataTable(mStrSQL);
        }


        public MyManagerCSharp.RGraph.Models.RGraphModel getLoginTopByUser(string reportId, int top, MyManagerCSharp.Log.LogManager.Days days, MyManagerCSharp.Log.LogUserManager.LogType loginType)
        {
            mStrSQL = "select top " + top + " t2.my_login as label, count (*) as valore" +
                "  from MyLogUSer as t1 " +
                " join Utente as t2 on t1.user_id = t2.user_id " +
                " where t1.tipo = '" + loginType.ToString() + "' ";

            mStrSQL += getWhereConditionByDate("t1.date_added", days);

            mStrSQL += " GROUP BY  my_login " +
               " order by count(*) desc";

            MyManagerCSharp.RGraph.Models.RGraphModel report = new MyManagerCSharp.RGraph.Models.RGraphModel();

            report.Id = reportId;
            report.Data = mFillDataTable(mStrSQL);

            report.Tipo = ReportType.HBar;
            report.Label = LabelType.LabelAndValore;
            report.Titolo = "I primi " + top + "  utenti che hanno eseguito più accessi";

            if (loginType == MyManagerCSharp.Log.LogUserManager.LogType.LoginMobile)
            {
                report.Titolo += " da dispositivi Mobile";
            }

            report.Titolo += " ( " + days.ToString().Replace("_", " ") + " )";

            report.Colors = PaletteType.Palette01;
            // ShowLabels = true;

            report.Settings.Add(String.Format("{0}.set('chart.gutter.left', {1});", report.Id, 150));
            report.Width = "700px";
            report.Height = "400px";

            getChart(report, false, false);

            return report;
        }




        public MyManagerCSharp.RGraph.Models.RGraphModel getLoginSuccessAndFailure(string reportId, int top, MyManagerCSharp.Log.LogManager.Days days)
        {
            mStrSQL = "SELECT TOP " + top + " MY_LOGIN, LOGIN_SUCCESS, LOGIN_FAILURE FROM UTENTE WHERE ( LOGIN_SUCCESS > 0 Or LOGIN_FAILURE > 0 ) AND (DATE_DELETED is null) ";

            //mStrSQL +=" ORDER BY MY_LOGIN ";
            mStrSQL += " ORDER BY (LOGIN_SUCCESS +  LOGIN_FAILURE) desc ";


            MyManagerCSharp.RGraph.Models.RGraphModel report = new MyManagerCSharp.RGraph.Models.RGraphModel();

            report.Id = reportId;
            report.Data = mFillDataTable(mStrSQL);

            report.Tipo = ReportType.HBar;
            report.Label = LabelType.LabelAndValore;
            report.Titolo = "Last " + top + " login";
            report.Colors = PaletteType.Palette01;


            getChart(report, true, false);
            Debug.WriteLine("html" + report.Html);
            return report;
        }




        public System.Data.DataTable getLoginByDayAsTable(DateTime dataDiRiferimento, long userId)
        {

            mStrSQL = "SELECT t1.*, t2.my_login ";

            if (userId != -1)
            {
                mStrSQL = "SELECT t1.date_added as [Data], t1.ip_address as IP";
            }

            mStrSQL += " FROM MyLogUser as t1 " +
                    " left join Utente as t2 on t1.user_id = t2.user_id ";

            mStrSQL += " WHERE (tipo = '" + MyManagerCSharp.Log.LogUserManager.LogType.Login.ToString() + "' OR tipo = '" + MyManagerCSharp.Log.LogUserManager.LogType.LoginMobile.ToString() + "') ";

            mStrSQL += String.Format("AND ( DAY({0})={1} AND  MONTH({0})={2} AND YEAR({0})={3} ) ", "t1.date_added", dataDiRiferimento.Day, dataDiRiferimento.Month, dataDiRiferimento.Year);


            if (userId != -1)
            {
                mStrSQL += " AND t1.user_id = " + userId;
            }


            mStrSQL += " ORDER BY t1.date_added asc ";

            return mFillDataTable(mStrSQL);
        }

        public MyManagerCSharp.RGraph.Models.RGraphModel getLoginByDay(string reportId, DateTime dataDiRiferimento)
        {
            mStrSQL = "SELECT * FROM MyLogUser WHERE (tipo = '" + MyManagerCSharp.Log.LogUserManager.LogType.Login.ToString() + "' OR tipo = " + MyManagerCSharp.Log.LogUserManager.LogType.LoginMobile.ToString() + "') ";

            mStrSQL += String.Format("AND ( DAY({0})={1} AND  MONTH({0})={2} AND YEAR({0})={3} ) ", "date_added", dataDiRiferimento.Day, dataDiRiferimento.Month, dataDiRiferimento.Year);

            mStrSQL += " ORDER BY date_added asc";


            MyManagerCSharp.RGraph.Models.RGraphModel report = new MyManagerCSharp.RGraph.Models.RGraphModel();

            report.Id = reportId;
            report.Data = mFillDataTable(mStrSQL);

            report.Tipo = ReportType.HBar;
            report.Label = LabelType.LabelAndValore;
            report.Titolo = "";
            report.Colors = PaletteType.Palette01;


            getChart(report, true, false);
            Debug.WriteLine("html" + report.Html);
            return report;



        }





        public MyManagerCSharp.RGraph.Models.RGraphModel getLastLogin(string reportId, int top, MyManagerCSharp.Log.LogManager.Days days)
        {
            mStrSQL = "select top " + top + " t1.my_login as label, date_last_login as valore" +
                "  from Utente as t1 ";

            mStrSQL += " WHERE (1=1) " + getWhereConditionByDate("date_last_login", days);

            mStrSQL += " order by date_last_login desc";

            MyManagerCSharp.RGraph.Models.RGraphModel report = new MyManagerCSharp.RGraph.Models.RGraphModel();

            report.Id = reportId;
            report.Data = mFillDataTable(mStrSQL);

            report.Tipo = ReportType.HBar;
            report.Label = LabelType.LabelAndValore;
            report.Titolo = "Last " + top + " login";
            report.Colors = PaletteType.Palette01;
            // ShowLabels = true;

            report.Settings.Add(String.Format("{0}.set('chart.gutter.left', {1});", report.Id, 150));

            getChart(report, false, false);
            //Debug.WriteLine("html" + report.Html);
            return report;
        }

    }

}
