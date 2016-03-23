using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MyManagerCSharp
{
    public class MyJQueryMobile
    {

        public enum IconType
        {
            undefined,
            edit,
            delete,
            search
        }
        protected static string DATA_MINI = "true";
        protected static bool UNOBTRUSIVE_VALIDATION = true;

        public static System.Globalization.CultureInfo CultureInfoEN = new System.Globalization.CultureInfo("en-GB");


        public static HtmlString MyCheckBox(string name, string id ,bool isChecked, string label)
        {
            if (String.IsNullOrEmpty(id) )
            {
                id = name.Replace(".", "_");
            }
            string temp;
            string strIsChecked = "";

            if (isChecked)
            {
                strIsChecked = String.Format("checked = \"checked\"");
            }

            temp = String.Format("<input type=\"checkbox\" id=\"{0}\" name=\"{1}\" value=\"true\"  data-mini=\"true\" {2} />", id, name, strIsChecked);

            if (!String.IsNullOrEmpty(label))
            {
                //  temp += Environment.NewLine + String.Format("<label for=\"{0}\" >{1}</label>", id, label);
                temp = "<label> " + temp + label + "</label>";
            }
            return new HtmlString(temp);
        }

        public static HtmlString MyCheckBox(string name, bool isChecked, string label)
        {
            return MyCheckBox(name, "", isChecked, label);          
        }

        public static HtmlString MyAnchorPopup(string url, string label, IconType ico)
        {
            if (!url.StartsWith("#"))
            {
                throw new ApplicationException("La url deve iniziare con un #");
            }
            string css = "";

            if (String.IsNullOrEmpty(label))
            {
                label = "No text";
                css = " ui-btn-icon-notext";
            }
            else
            {
                css = " ui-btn-icon-left";
            }


            if (ico != IconType.undefined)
            {
                css += " ui-icon-" + ico.ToString();
            }

            if (DATA_MINI == "true")
            {
                css += " ui-mini";
            }

            string temp;
            temp = String.Format("<a href=\"{0}\" data-rel=\"popup\" data-position-to=\"window\"  class=\"ui-btn ui-btn-inline ui-shadow ui-corner-all {1}\" >{2}</a> ", url.Trim(), css.Trim(), label.Trim());


            return new HtmlString(temp);

        }

        public static HtmlString MyAnchor(string url, string label, IconType ico)
        {
            string temp;
            string css = "";

            if (String.IsNullOrEmpty(label))
            {
                label = "No text";
                css = " ui-btn-icon-notext";
            }
            else
            {
                css = " ui-btn-icon-left";
            }

            if (String.IsNullOrEmpty(url))
            {
                url = "#";
            }


            if (ico != IconType.undefined)
            {
                css += " ui-icon-" + ico.ToString();
            }

            if (DATA_MINI == "true")
            {
                css += " ui-mini";
            }

            temp = String.Format("<a href=\"{0}\"  class=\"ui-btn ui-btn-inline ui-corner-all ui-shadow {1}\" >{2}</a> ", url.Trim(), css.Trim(), label.Trim());


            return new HtmlString(temp);
        }



        #region "___ MyInputType ___"


        public static HtmlString MyInputType(string name, string displayName, DateTime? value, bool isRequired)
        {
            string id = name.Replace(".", "_");

            StringBuilder unobtrusiveValidation = new StringBuilder();
            
            StringBuilder sb = new StringBuilder();
            m_label(sb, id, displayName, isRequired);

            string tempValue;
            if (value == null || value == DateTime.MinValue)
            {
                tempValue = "";
            }
            else
            {
                tempValue = value.Value.ToShortDateString();
            }


            //temp.Append("<input type=\"text\" ");
            sb.Append("<input type=\"date\" ");
            if (isRequired)
            {
                m_isRequired(sb, unobtrusiveValidation, name, displayName);
            }

  
           // temp.Append(String.Format("data-role=\"date\" id=\"{0}\" name=\"{1}\" value=\"{2}\"  data-inline=\"true\"  data-mini=\"{3}\" />", id, name, tempValue, DATA_MINI));
            sb.Append(String.Format("id=\"{0}\" name=\"{1}\" value=\"{2}\"  data-inline=\"true\"  data-mini=\"{3}\"  data-clear-btn=\"true\" />", id, name, tempValue, DATA_MINI));
            sb.Append(Environment.NewLine);
            sb.Append(unobtrusiveValidation);
            return new HtmlString(sb.ToString());
        }

        public static HtmlString MyInputType(string name, string displayName, decimal? value, bool isRequired)
        {
            string id = name.Replace(".", "_");

            StringBuilder unobtrusiveValidation = new StringBuilder();

            StringBuilder sb = new StringBuilder();
            m_label(sb, id, displayName, isRequired);

            string tempValue;
            if (value == null)
            {
                tempValue = "";
            }
            else
            {
                tempValue = String.Format(CultureInfoEN, "{0:N2}", value).Replace(",", "");
            }


            sb.Append("<input type=\"number\" ");
            if (isRequired)
            {
                m_isRequired(sb, unobtrusiveValidation, name, displayName);
            }
            sb.Append(String.Format("step=\"0.1\" min=\"0\" id=\"{0}\" name=\"{1}\" value=\"{2}\" data-mini=\"{3}\" data-clear-btn=\"true\" />", id, name, tempValue, DATA_MINI));
            sb.Append(Environment.NewLine);
            sb.Append(unobtrusiveValidation);
            return new HtmlString(sb.ToString());
        }

        public static HtmlString MyInputType(string name, string displayName, float? value, bool isRequired)
        {
            string id = name.Replace(".", "_");

            StringBuilder unobtrusiveValidation = new StringBuilder();

            StringBuilder sb = new StringBuilder();
            m_label(sb, id, displayName, isRequired);


            string tempValue;
            if (value == null)
            {
                tempValue = "";
            }
            else
            {
                tempValue = String.Format("{0:N2}", value);
            }

            sb.Append("<input type=\"number\" ");
            if (isRequired)
            {
                m_isRequired(sb, unobtrusiveValidation, name, displayName);
            }
            sb.Append(String.Format("step=\"0.1\" min=\"0\" id=\"{0}\" name=\"{1}\" value=\"{2}\" data-mini=\"{3}\" />", id, name, tempValue, DATA_MINI));
            sb.Append(Environment.NewLine);
            sb.Append(unobtrusiveValidation);

            return new HtmlString(sb.ToString());
        }

        public static HtmlString MyInputType(string name, string displayName, int? value, bool isRequired)
        {
            string id = name.Replace(".", "_");

            StringBuilder unobtrusiveValidation = new StringBuilder();

            StringBuilder sb = new StringBuilder();
            m_label(sb, id, displayName, isRequired);

            string tempValue;
            if (value == null)
            {
                tempValue = "";
            }
            else
            {
                tempValue = String.Format("{0:N0}", value);
            }


            sb.Append("<input type=\"number\" ");
            if (isRequired)
            {
                m_isRequired(sb, unobtrusiveValidation, name, displayName);
            }
            sb.Append(String.Format("step=\"1\" min=\"0\" id=\"{0}\" name=\"{1}\" value=\"{2}\" data-mini=\"{3}\" />", id, name, tempValue, DATA_MINI));
            sb.Append(Environment.NewLine);
            sb.Append(unobtrusiveValidation);

            return new HtmlString(sb.ToString());
        }

        public static HtmlString MyInputType(string name, string displayName, string value, bool isRequired)
        {
            string id = name.Replace(".", "_");

            StringBuilder unobtrusiveValidation = new StringBuilder();

            StringBuilder sb = new StringBuilder();
            m_label(sb, id, displayName, isRequired);


            sb.Append("<input type=\"text\" ");
            if (isRequired)
            {
                m_isRequired(sb, unobtrusiveValidation, name, displayName);
            }
            sb.Append(String.Format(" id=\"{0}\" name=\"{1}\" value=\"{2}\" data-clear-btn=\"true\" data-inline=\"true\" data-mini=\"{3}\" />", id, name, value, DATA_MINI));
            sb.Append(Environment.NewLine);
            sb.Append(unobtrusiveValidation);

            return new HtmlString(sb.ToString());
        }


        private static void m_isRequired(StringBuilder input, StringBuilder validation, string name, string displayName)
        {
            if (UNOBTRUSIVE_VALIDATION)
            {
                //input.Append(String.Format("data-val=\"true\" data-val-required=\"Il campo {0} è obbligatorio.\" ", displayName.Replace("*", "")));
                input.Append(String.Format("data-val=\"true\" data-val-required=\"Il campo è obbligatorio.\" "));
                validation.Append(String.Format("<div class=\"field-validation\"  data-valmsg-for=\"{0}\" data-valmsg-replace=\"true\"></div>", name));
                validation.Append(Environment.NewLine);
            }
            else
            {
                input.Append(" required ");
            }

        }

        private static void m_label(StringBuilder sb, string id, string displayName, bool isRequired)
        {
            if (!String.IsNullOrEmpty(displayName))
            {

                if (isRequired)
                {
                    sb.Append(String.Format("<label for=\"{0}\">{1} *</label>", id, displayName));
                }
                else
                {
                    sb.Append(String.Format("<label for=\"{0}\">{1}</label>", id, displayName));
                }
                
                sb.Append(Environment.NewLine);
            }
        }



        #endregion


    }
}
