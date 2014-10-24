using System;
using System.Collections.Generic;
using System.Linq;

namespace MyManagerCSharp
{
    public class CSVManager
    {

        public static string toCSV(System.Data.DataTable dtable, string delimitedBy, bool includeColumnsHeader)
        {


            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            //If Not _progressBar Is Nothing Then
            //    _progressBar.Maximum = dtable.Rows.Count
            //    _progressBar.Value = 0
            //    _progressBar.Step = 1
            //End If

            string result = "";
            int icolcount = dtable.Columns.Count;

            if (includeColumnsHeader)
            {

                for (int i = 0; i < icolcount; i++)
                {
                    sb.Append(dtable.Columns[i]);
                    if (i < icolcount - 1)
                        sb.Append(delimitedBy);
                }

                sb.AppendLine();

            }

            int conta = 0;

            foreach (System.Data.DataRow row in dtable.Rows){

                for( int  i = 0 ; i < icolcount; i++){
                    if (! Convert.IsDBNull(row[i])) {
                        sb.Append(row[i].ToString());
                    }
                   if (i < icolcount - 1)
                        sb.Append(delimitedBy);
                }

                sb.AppendLine();
                conta = conta + 1;

                //If Not _statusBar Is Nothing Then
                //    _statusBar.Text = "Generazione file CSV: Record " & conta & "/" & dtable.Rows.Count
                //    Windows.Forms.Application.DoEvents()
                //End If

                //If Not _progressBar Is Nothing Then
                //    _progressBar.PerformStep()
                //    Windows.Forms.Application.DoEvents()
                //End If
            }

            //If Not _progressBar Is Nothing Then
            //    _progressBar.Value = 0
            //End If
            //If Not _statusBar Is Nothing Then
            //    _statusBar.Text = ""
            //End If

            result = sb.ToString();
            return result;
        }
    }

}

