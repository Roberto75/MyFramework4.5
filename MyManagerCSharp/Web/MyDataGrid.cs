﻿using Microsoft.CSharp.RuntimeBinder;
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
            string header_footer = "ui-bar-b";

            StringBuilder sb = new StringBuilder();

            sb.Append("<table class=\"MyTable ui-responsive\" data-role=\"table\" data-mode=\"reflow\" >");
            sb.Append("<thead>");
            sb.Append(String.Format("<tr class=\"{0}\">", header_footer));

            foreach (MyDataColumn col in Columns)
            {
                sb.Append("<th>");
                if (col.CanSort)
                {
                    if (page.Sort == col.ColumnName)
                    {
                        if (page.SortDir == "ASC")
                        {
                            sb.Append(String.Format("<a href=\"javascript:paging(1,'{0}','{1}');\" >{2}</a>", page.Sort, "DESC", col.Header + "▲"));
                        }
                        else
                        {
                            sb.Append(String.Format("<a href=\"javascript:paging(1,'{0}','{1}');\" >{2}</a> ", page.Sort, "ASC", col.Header + "▼"));
                        }
                    }
                    else
                    {
                        sb.Append(String.Format("<a href=\"javascript:paging(1,'{0}','{1}');\" >{2}</a>", col.ColumnName, "ASC", col.Header));
                    }
                }
                else
                {
                    sb.Append(col.Header);
                }

                sb.Append("</th>");
            }
            sb.Append("</tr>");
            sb.Append("</thead>");


            Debug.WriteLine("Rows #" + Rows.Count());


            sb.Append("<tbody>");
            foreach (var row in Rows)
            {
                sb.Append("<tr>");

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


                        if (col.Format == null)
                        {
                            sb.Append("<td>" + value.ToString() + "</td>");
                        }
                        else
                        {
                            Debug.WriteLine("Format: ");
                            var result = col.Format(value);
                            sb.Append("<td>" + result.ToString() + "</td>");
                        }

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
            sb.Append("</tbody>");


            sb.Append(String.Format("<tfoot class=\"{0}\" >", header_footer));
            sb.Append("<tr>");
            sb.Append("<td colspan=\"" + (Columns.Count() - 1) + "\">");
            sb.Append(String.Format("Page {0:N0} of {1:N0}", page.PageNumber, page.TotalPages));
            sb.Append("<td>");
            sb.Append("</tr>");
            sb.Append("</tfoot>");


            sb.Append("</table>");

            return new HtmlString(sb.ToString());
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
