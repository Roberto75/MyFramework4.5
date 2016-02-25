using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace My.Shared.Web
{
    public class MyDataGrid
    {
        public IEnumerable<MyDataColumn> Columns { get; set; }
        public IEnumerable<dynamic> Rows { get; set; }

        private My.Shared.Models.Paged page;

        public MyDataGrid(object model)
        {
            page = (My.Shared.Models.Paged)model;
        }

        public void SetButtonDetails(string href)
        {
            //<a class="ui-btn-inline ui-btn ui-icon-info ui-btn-icon-notext ui-corner-all" title="Detail" href="@Url.Action("Details", "Segnalazioni", new { id = item.id })">Info</a>
        }

        public HtmlString GetHtml()
        {
            return GetHtml("ui-bar-c", "ui-body-a", "ui-body-b");
        }

        public HtmlString GetHtml(string css_header_footer, string css_rowStyle, string css_alternatingRow)
        {
            //css_header_footer = "";
            //css_rowStyle = "";
            //css_alternatingRow = "";


            StringBuilder sb = new StringBuilder();

            sb.Append("<table class=\"MyTable ui-responsive\" data-role=\"table\" data-mode=\"reflow\" >");

            sb.AppendFormat("<caption>Record #{0:N0}</caption>", page.TotalRows);


            //sb.Append("<table class=\"MyTable\" >");
            sb.Append("<thead>");
            #region   ___  HEAD ___

            sb.Append(String.Format("<tr class=\"{0}\">", css_header_footer));

            foreach (MyDataColumn col in Columns)
            {
                sb.Append("<th>");
                if (col.CanSort)
                {
                    if (page.Sort == col.ColumnName)
                    {
                        if (page.SortDir == "ASC")
                        {
                            sb.Append(String.Format("<a href=\"javascript:paging(1,'{0}','{1}');\" {3} >{2}</a>", page.Sort, "DESC", col.Header + "▲", String.IsNullOrEmpty(col.HeaderHrefTitle)?"":"title=\"" + col.HeaderHrefTitle   + "\"" ));
                        }
                        else
                        {
                            sb.Append(String.Format("<a href=\"javascript:paging(1,'{0}','{1}');\" {3} >{2}</a> ", page.Sort, "ASC", col.Header + "▼",String.IsNullOrEmpty(col.HeaderHrefTitle)?"":"title=\"" + col.HeaderHrefTitle   + "\"" ));
                        }
                    }
                    else
                    {
                        sb.Append(String.Format("<a href=\"javascript:paging(1,'{0}','{1}');\" {3} >{2}</a>", col.ColumnName, "ASC", col.Header, String.IsNullOrEmpty(col.HeaderHrefTitle)?"":"title=\"" + col.HeaderHrefTitle   + "\"" ));
                    }
                }
                else
                {
                    sb.Append(col.Header);
                }

                sb.Append("</th>");
            }
            sb.Append("</tr>");
            #endregion
            sb.Append("</thead>");


            sb.Append("<tfoot>");
            //  sb.Append(String.Format("<tfoot class=\"{0}\">", css_header_footer));
            #region ___ FOOTER __
            sb.Append(String.Format("<tr class=\"{0}\">", css_header_footer));
            //sb.Append("<tr>");
            sb.Append("<td colspan=\"" + Columns.Count() + "\">");

            int temp = (((page.PageNumber - 1) * page.PageSize) + page.PageSize);
            if (temp > page.TotalRows)
            {
                temp = page.TotalRows;
            }


            sb.Append("<span style=\"margin-left: 5px; margin-top: 0px; display: inline-block;\">");
            sb.Append(String.Format("Record {0:N0} to {1:N0} of {2:N0}", ((page.PageNumber - 1) * page.PageSize) + 1, temp, page.TotalRows));
            sb.Append("</span>");


            sb.Append("<span style=\"margin-left: 10px; margin-top: 0px; display: inline-block;\">");
            previousButtons(sb);
            sb.Append("</span>");


            sb.Append("<span style=\"margin-left: 10px; margin-top: 0px; display: inline-block;\">");
            sb.Append(String.Format("Page {0:N0} of {1:N0}", page.PageNumber, page.TotalPages));
            sb.Append("</span>");


            sb.Append("<span style=\"margin-left: 10px; margin-top: 0px; display: inline-block;\">");
            nextButtons(sb);
            sb.Append("</span>");

            sb.Append("<div style=\"float: right; text-align: right; display: inline-block; white-space: nowrap; margin-top: 10px; margin-right: 10px;\">");
            comboPagaeSize(sb);
            sb.Append("</div>");

            sb.Append("</td>");
            sb.Append("</tr>");
            #endregion
            sb.Append("</tfoot>");



            sb.Append("<tbody>");
            #region ___ BODY ___

            Debug.WriteLine("Rows #" + Rows.Count());
            int conta = 0;

            foreach (var row in Rows)
            {
                conta++;

                if (conta % 2 == 0)
                {
                    sb.Append(String.Format("<tr class=\"{0}\">", css_rowStyle));
                }
                else
                {
                    sb.Append(String.Format("<tr class=\"{0}\">", css_alternatingRow));
                }




                Type ty = row.GetType();
                PropertyInfo[] properties;
                properties = ty.GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    Debug.WriteLine(property.Name + " " + property.PropertyType);
                }


                //MemberInfo[] memberArray = ty.GetMembers();
                //foreach (MemberInfo member in memberArray)
                //{
                //    Debug.WriteLine(member.Name + " " + member.DeclaringType);
                //}



                foreach (MyDataColumn col in Columns)
                {

                    if (col.Button == null)
                    {
                        object value = null;
                        bool esito;
                        if (col.PropertyName.IndexOf(".") == -1)
                        {
                            esito = TryGetMember(row, col.PropertyName, out value);
                        }
                        else
                        {
                            esito = TryGetComplexMember(row, col.PropertyName, out value);
                        }

                        //PropertyInfo property = row.GetType().GetProperty(col.PropertyName);

                        //property.co

                        if (esito == false)
                        {
                            throw new ApplicationException("ColummName non valido: " + col.PropertyName);
                        }

                        Debug.WriteLine(col.PropertyName + ": " + value.ToString());

                        //apro TD la colonna!
                        if (col.Option == null)
                        {
                            sb.Append("<td>");
                        }
                        else
                        {
                            sb.Append("<td style=\"white-space:nowrap;\">");
                        }


                        if (col.Format == null)
                        {
                            sb.Append(value.ToString() + "</td>");
                        }
                        else
                        {
                            Debug.WriteLine("Format: ");
                            var result = col.Format(value);
                            sb.Append(result.ToString() + "</td>");
                        }
                        // chiudo TD

                    }
                    else
                    {
                        var url = col.Format(row);
                        sb.Append(String.Format("<td><a class=\"ui-btn-inline ui-btn ui-icon-info ui-btn-icon-notext ui-corner-all\" title=\"{0}\" href=\"{1}\">{0}</a></td>", col.Header, url.ToString()));
                    }


                    //var value = property.GetValue(row, null);



                    //     var binder = Binder.GetMember(CSharpBinderFlags.None, "", typeof(ty), new[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) });



                }

                sb.Append("</tr>");
            }

            #endregion
            sb.Append("</tbody>");




            sb.Append("</table>");

            return new HtmlString(sb.ToString());
        }



        private void nextButtons(StringBuilder sb)
        {

            if (page.HasNextPage)
            {
                sb.AppendFormat("<a class=\"ui-btn ui-btn-inline ui-corner-all ui-mini\" href=\"javascript:paging({0}, '{1}', '{2}')\"> Next &gt; </a>", page.PageNumber + 1, page.Sort, page.SortDir);
                sb.AppendFormat("<a class=\"ui-btn ui-btn-inline ui-corner-all ui-mini\" href=\"javascript:paging({0}, '{1}', '{2}')\"> Last &gt;&gt;</a>", page.TotalPages, page.Sort, page.SortDir);
            }
            else
            {
                //sb.Append("Next &gt;  Last &gt; &gt; ");
                sb.Append("<a class=\"ui-btn ui-btn-inline ui-corner-all ui-mini ui-state-disabled\" > Next &gt; </a>");
                sb.Append("<a class=\"ui-btn ui-btn-inline ui-corner-all ui-mini ui-state-disabled\" > Last &gt;&gt;</a>");
            
            }

        }

        private void previousButtons(StringBuilder sb)
        {

            if (page.HasPreviousPage)
            {
                sb.AppendFormat("<a class=\"ui-btn ui-btn-inline ui-corner-all ui-mini\" href=\"javascript:paging(1, '{0}', '{1}')\"> &lt;&lt; First</a>", page.Sort, page.SortDir);

                //@Html.Raw(" ");
                //<a class="ui-btn ui-btn-inline ui-corner-all ui-mini" href="javascript:paging(@(Model.PageNumber - 1), '@Model.Sort', '@Model.SortDir')">< Prev</a>
                sb.AppendFormat("<a class=\"ui-btn ui-btn-inline ui-corner-all ui-mini\" href=\"javascript:paging({0}, '{1}', '{2}')\">&lt; Prev</a>", page.PageNumber - 1, page.Sort, page.SortDir);
            }
            else
            {
               // sb.Append("&lt;&lt; First &lt;Prev");
                sb.Append("<a class=\"ui-btn ui-btn-inline ui-corner-all ui-mini ui-state-disabled\" > &lt;&lt; First</a>");
                sb.Append("<a class=\"ui-btn ui-btn-inline ui-corner-all ui-mini ui-state-disabled\" >&lt; Prev</a>");
            }

        }

        private void comboPagaeSize(StringBuilder sb)
        {
            sb.Append("Page size:");

            sb.AppendFormat("<select data-inline=\"true\" data-mini=\"true\" data-role=\"none\" id=\"PageSize\" name=\"PageSize\" onchange=\"paging(1, '{0}', '{1}')\">", page.Sort, page.SortDir);
            sb.AppendFormat("<option value=\"5\" {0} >5</option>", (page.PageSize == 5 ? "selected=\"selected\"" : ""));
            sb.AppendFormat("<option value=\"10\" {0} >10</option>", (page.PageSize == 10 ? "selected=\"selected\"" : ""));
            sb.AppendFormat("<option value=\"20\" {0} >20</option>", (page.PageSize == 20 ? "selected=\"selected\"" : ""));
            sb.AppendFormat("<option value=\"50\" {0} >50</option>", (page.PageSize == 50 ? "selected=\"selected\"" : ""));
            sb.Append("</select>");
        }

        private static bool TryGetComplexMember(object obj, string name, out object result)
        {
            result = null;

            string[] names = name.Split('.');
            for (int i = 0; i < names.Length; i++)
            {
                if ((obj == null) || !TryGetMember(obj, names[i], out result))
                {
                    result = null;
                    return false;
                }
                obj = result;
            }
            return true;
        }

        private static bool TryGetMember(object obj, string name, out object result)
        {
            PropertyInfo property = obj.GetType().GetProperty(name);
            if ((property != null) && (property.GetIndexParameters().Length == 0))
            {
                result = property.GetValue(obj, null);
                return true;
            }
            result = null;
            return false;
        }

    }
}
