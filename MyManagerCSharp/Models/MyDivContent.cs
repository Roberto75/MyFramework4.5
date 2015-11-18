using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp.Models
{
    public class MyDivContent
    {
        // public string DivId { get; set; }

        public string DivId
        {
            get
            {

                return "Div" + reportId;
            }
        }
        public string Action { get; set; }

        public string reportId { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        //public string JavaScriptOnDone { get; set; }

        private bool DISPLAY_DEBUG_ALERT = true;
        public const string LOADING_BACKGROUD_COLOR = "#efefef";

        // public const string WAIT = "<img style='display:block; margin-left:auto; margin-right:auto; margin-top:200px; vertical-align:middle;' src='/Content/Images/Shared/wait01.gif' />";

        //public  string WAIT = "<h3 class='animated fadeIn' style='margin-left:auto; margin-right:auto;' >Loading ...</h3>";

        public System.Web.HtmlString callInitFunction()
        {
            return callInitFunction(0);
        }

        public System.Web.HtmlString callInitFunction(int delay)
        {
            string js = "";

            if (delay == 0)
            {
                js += String.Format("init{0} ();", DivId);
            }
            else
            {
                js += "setTimeout(function(){ ";


                js += String.Format("init{0} ();", DivId);


                js += "}, " + delay + "); ";

            }

            js += Environment.NewLine;
            return new System.Web.HtmlString(js);
        }

        public System.Web.HtmlString getInitFunction()
        {
            string js = "";

            js += Environment.NewLine;

            js += String.Format("function init{0} () {{", DivId);

            if (DISPLAY_DEBUG_ALERT)
            {
                js += String.Format("alert('init{0}');", DivId);
            }

            js += String.Format("var request{0} = $.ajax({{", DivId);
            js += String.Format("url:\"{0}\",", Action);
            js += "type: \"POST\"," +
                  "cache: false, " +
                  "dataType: \"html\"";


            js += String.Format(",data: {{ reporId: \"{0}\", width: \"{1}\" , height:\"{2}\"  }}", reportId, width -30 , height-80);


            js += beforeSend();

            js += " });";

            js += requestOnDone();

            js += requestOnFail();


            js += "}"; // fine function initDiv


            // if (!String.IsNullOrEmpty(reportId))
            //{
            js += refreshReport();
            //}
            js += Environment.NewLine;
            return new System.Web.HtmlString(js);
        }


        private string getWait()
        {
            return String.Format("<h3 style='padding-top:{0:N1}px;padding-left:{1:N1}px;' >Loading ...</h3>", (height / 2).ToString().Replace(",", "."), ((width / 2) - 30).ToString().Replace(",", "."));
        }




        private string beforeSend()
        {

            string temp = "";
            temp += ",beforeSend: function( xhr ) {";


            if (DISPLAY_DEBUG_ALERT)
            {
                temp += "alert('beforeSend');";
            }


            // js += String.Format("$(\"#{0}\").css('background-color', '{1}');", DivId, LOADING_BACKGROUD_COLOR);

           //temp += String.Format("$(\"#{0}\").html(\"{1}\");", DivId, getWait());

            temp += String.Format("$(\"#{0}\").html(\"{1}\");", DivId, "Loading ...");

            temp += "}";


            return temp;


        }


        private string requestOnDone()
        {

            string temp = "";
            temp += Environment.NewLine;
            temp += String.Format("request{0}.done(function (msg) {{ ", DivId);

            if (DISPLAY_DEBUG_ALERT)
            {
                //temp += "alert(msg);";
            }

            temp += String.Format("$(\"#{0}\").css('background-color', '');", DivId);
            temp += String.Format("$(\"#{0}\").html(msg);", DivId);

            //  if (!String.IsNullOrEmpty(reportId))
            //{
            temp += String.Format("initReport{0}();", reportId);
            //}
            //jquerymobile
            temp += String.Format("$(\"#{0}\").trigger('create');", DivId);
            temp += " });";

            return temp;

        }


        private string requestOnFail()
        {
            string temp = "";

            temp += Environment.NewLine;

            temp += String.Format("request{0}.fail(function (jqXHR, textStatus) {{", DivId);
            temp += "alert(\"Request failed: \" + textStatus);";
            temp += "});";

            return temp;

        }


        private string refreshReport()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(String.Format("function refresh{0}() {{", reportId));

            if (DISPLAY_DEBUG_ALERT)
            {
                sb.Append("alert('refresh');");
            }
            //sb.Append(String.Format("$(\"#{0}\").html('');", DivId));

            sb.Append(String.Format(" init{0}();", DivId));

            sb.Append("}");

            return sb.ToString();
        }




    }
}
