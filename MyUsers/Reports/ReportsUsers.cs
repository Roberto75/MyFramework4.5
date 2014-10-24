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


        public MyManagerCSharp.RGraph.Models.RGraphModel getLoginByDate(string reportId, MyManagerCSharp.Log.LogManager.Days days)
        {
            _strSQL = "select count(*) as valore, CAST(date_added AS DATE)  as label "
                + " from MyLogUSer as t1 ";

            _strSQL += " WHERE  (t1.tipo = '" + MyManagerCSharp.Log.LogUserManager.LogType.Login.ToString() + "' or  t1.tipo = '" + MyManagerCSharp.Log.LogUserManager.LogType.LoginMobile.ToString() + "' )";

            _strSQL += getWhereConditionByDate("date_added", days);

            _strSQL += " GROUP BY CAST(date_added AS DATE) "
                + " ORDER BY CAST(date_added AS DATE) ";

            MyManagerCSharp.RGraph.Models.RGraphModel report = new MyManagerCSharp.RGraph.Models.RGraphModel();

            report.Id = reportId;
            report.Data = _fillDataTable(_strSQL);
            report.Tipo = ReportType.Line;
            report.Label = LabelType.LabelAndValore;
            report.Titolo = "Login " + days.ToString().Replace("_", " ");
            //report.Colors = PaletteType.Palette01;

            //ShowLabels = true;

            getChart(report, false, false);
            //Debug.WriteLine("html" + report.Html);

            return report;
        }





        public System.Data.DataTable getLastLogin(MyManagerCSharp.Log.LogManager.Days days)
        {

            _strSQL = "select date_last_login,  my_login, user_id  from Utente Where (1=1) ";
            _strSQL += getWhereConditionByDate("date_last_login", days);
            _strSQL += "order by date_last_login desc";

            return _fillDataTable(_strSQL);
        }


        public MyManagerCSharp.RGraph.Models.RGraphModel getLoginTopByUser(string reportId, int top, MyManagerCSharp.Log.LogManager.Days days, MyManagerCSharp.Log.LogUserManager.LogType loginType)
        {
            _strSQL = "select top " + top + " t2.my_login as label, count (*) as valore" +
                "  from MyLogUSer as t1 " +
                " join Utente as t2 on t1.user_id = t2.user_id " +
                " where t1.tipo = '" + loginType.ToString() + "' ";

            _strSQL += getWhereConditionByDate("t1.date_added", days);

            _strSQL += " GROUP BY  my_login " +
               " order by my_login";

            MyManagerCSharp.RGraph.Models.RGraphModel report = new MyManagerCSharp.RGraph.Models.RGraphModel();

            report.Id = reportId;
            report.Data = _fillDataTable(_strSQL);

            report.Tipo = ReportType.HBar;
            report.Label = LabelType.LabelAndValore;
            report.Titolo = "Top " + top + " " + loginType.ToString();
            report.Colors = PaletteType.Palette01;
            // ShowLabels = true;

            report.Settings.Add(String.Format("{0}.set('chart.gutter.left', {1});", report.Id, 150));

            getChart(report, false, false);

            return report;
        }




        public MyManagerCSharp.RGraph.Models.RGraphModel getLoginSuccessAndFailure(string reportId, int top, MyManagerCSharp.Log.LogManager.Days days)
        {
            _strSQL = "SELECT TOP " + top + " MY_LOGIN, LOGIN_SUCCESS, LOGIN_FAILURE FROM UTENTE WHERE ( LOGIN_SUCCESS > 0 Or LOGIN_FAILURE > 0 ) AND (DATE_DELETED is null) ";
            
            //_strSQL +=" ORDER BY MY_LOGIN ";
            _strSQL += " ORDER BY (LOGIN_SUCCESS +  LOGIN_FAILURE) desc ";


            MyManagerCSharp.RGraph.Models.RGraphModel report = new MyManagerCSharp.RGraph.Models.RGraphModel();

            report.Id = reportId;
            report.Data = _fillDataTable(_strSQL);

            report.Tipo = ReportType.HBar;
            report.Label = LabelType.LabelAndValore;
            report.Titolo = "Last " + top + " login";
            report.Colors = PaletteType.Palette01;
         

            getChart(report, true, false);
            Debug.WriteLine("html" + report.Html);
            return report;


            
        }





        public MyManagerCSharp.RGraph.Models.RGraphModel getLastLogin(string reportId, int top, MyManagerCSharp.Log.LogManager.Days days)
        {
            _strSQL = "select top " + top + " t1.my_login as label, date_last_login as valore" +
                "  from Utente as t1 ";

            _strSQL += " WHERE (1=1) " + getWhereConditionByDate("date_last_login", days);

            _strSQL += " order by date_last_login desc";

            MyManagerCSharp.RGraph.Models.RGraphModel report = new MyManagerCSharp.RGraph.Models.RGraphModel();

            report.Id = reportId;
            report.Data = _fillDataTable(_strSQL);

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
