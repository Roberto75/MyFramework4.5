using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp.RGraph
{
    //https://twitter.com/_RGraph



    public class RGraphManager : ManagerDB
    {
        public enum ReportType
        {
            DisplayLCD,
            Tabella,
            AngularGauge,
            Area,
            Column,
            Line,
            Bar,
            HBar,
            Pie,
            Scatter
        }

        public enum PaletteType
        {
            None,
            Palette01,
            Palette02,
            RedOrangeGreen
        }

        public enum LabelType
        {
            None,
            Label,
            Valore,
            LabelAndValore
        }

        private decimal _totale;

        //public LabelType Label { get; set; }

        //public bool ShowPercentuale { get; set; }
        public bool ShowLegend { get; set; }
        // public bool ShowLabels { get; set; }
        //public PaletteType SetPalette { get; set; }

        public bool OrderColor { get; set; }

        // public string _titolo = "";

        public bool EnableOnClick { get; set; }


        private List<string> _palette1 = new List<string> {"ffc363", "eb6033", "307ea9", "d1ccd1", "425c86" 
                                                         , "63a1fa", "92CD00", "2C6700", "ffe384", "ce694a" 
                                                         , "005dde", "6b7d94", "336699", "FFFFFF", "003366" 
                                                         , "996633", "666633", "E5E4D7", "CCCC99", "990033"};

        private List<string> _palette2 = new List<string> {"1D8BD1", "F1683C", "2AD62A", "DBDC25", "8FBC8B", "D2B48C", "FAF0E6", "20B2AA", "B0C4DE", "DDA0DD", "9C9AFF", "9C3063", "FFFFCE" 
                                                          , "CEFFFF", "630063", "FF8284", "0065CE", "CECFFF", "000084", "FF00FF", "FFFF00", "00FFFF", "840084", "840000", "008284", "0000FF" 
                                                          , "00CFFF", "CEFFFF", "CEFFCE", "FFFF9C", "9CCFFF", "FF9ACE", "CE9AFF", "FFCF9C", "3165FF", "31CFCE", "9CCF00", "FFCF00", "FF9A00" 
                                                          , "FF6500"};

        //private List<string> _palette3 = new List<string> { "red", "green", "orange", "gray" };
        private List<string> _palette3 = new List<string> { "eb6033", "92CD00", "ffe384", "DFDFDF" };


        private List<string> _palette;

        public RGraphManager(string connectionName)
            : base(connectionName)
        {
            // SetPalette = PaletteType.None;
            _palette = _palette1;

        }
        public RGraphManager(System.Data.Common.DbConnection connection)
            : base(connection)
        {
            //SetPalette = PaletteType.None;
            _palette = _palette1;
        }




        public string getChart(string name, RGraphManager.ReportType tipo, string sqlQuery, bool rotateDataSet, bool includeTable)
        {
            //eseguo la query
            System.Data.DataTable dt;
            dt = _fillDataTable(sqlQuery);


            Models.RGraphModel report = new Models.RGraphModel();
            report.Tipo = tipo;
            report.Id = name;

            getChart(report, rotateDataSet, includeTable);
            return report.Html;
        }


        public string getChart(string name, RGraphManager.ReportType tipo, System.Data.DataTable dataTable, bool rotateDataSet, bool includeTable)
        {
            Models.RGraphModel report = new Models.RGraphModel();
            report.Tipo = tipo;
            report.Id = name;
            report.Data = dataTable;

            getChart(report, rotateDataSet, includeTable);
            return report.Html;
        }



        public System.Data.DataTable rotateRigheToColonne(System.Data.DataTable dataTable)
        {



            System.Data.DataTable table = new System.Data.DataTable();
            string columnName;

            //'tutte i valori (LABEL) delle righe diventano le mie colonne
            //'mi scorro tutte le righe leggendo la prima colonna
            table.Columns.Add("LABEL");


            foreach (System.Data.DataRow r in dataTable.Rows)
            {
                columnName = (r[0] is DBNull) ? "" : r[0].ToString();
                table.Columns.Add(columnName);
            }

            //For i = 0 To dataTable.Rows.Count - 1
            //    columnName = CStr(IIf(IsDBNull(dataTable.Rows(i)(0)), "", dataTable.Rows(i)(0).ToString))
            //    '      If Not table.Columns.Contains(columnName) Then
            //    table.Columns.Add(columnName)
            //    '  End If
            //Next

            System.Data.DataRow row;
            for (int i = 1; i < dataTable.Columns.Count; i++)
            {
                //mi scorro tutta la colonna...
                row = table.NewRow();
                row[0] = dataTable.Columns[i].ColumnName;
                for (int j = 0; j < dataTable.Rows.Count; j++)
                {
                    row[j + 1] = dataTable.Rows[j][i];
                }
                table.Rows.Add(row);
            }

            //For i = 1 To dataTable.Columns.Count - 1
            //    'mi scorro tutta la colonna...
            //    row = table.NewRow
            //    row(0) = dataTable.Columns(i).ColumnName
            //    For j = 0 To dataTable.Rows.Count - 1
            //        row(j + 1) = dataTable.Rows(j)(i)
            //    Next
            //    table.Rows.Add(row)
            //Next
            return table;
        }

        public void getChart(Models.RGraphModel report, bool rotateDataSet, bool includeTable)
        {

            //'If isIndicatore(tipo) Then
            //'    Throw New MyManager.ManagerException("Con questi parametri NON è possibile creare report di tipo indicatore")
            //'End If

            if (rotateDataSet)
            {
                _dt = rotateRigheToColonne(report.Data);
            }
            else
            {
                _dt = report.Data;
            }

            if (_dt.Rows.Count == 0)
            {
                report.Html = "";
                return;
            }



            int indexColor = 0;

            string strJavaScript = "";
            string strData = "";
            string strColors = "";
            string strLegend = "";
            string strLabel = "";

            string strMyKeys = "";


            decimal totale = 0;


            int paletteColors;

            switch (report.Colors)
            {
                case PaletteType.Palette01:
                    _palette = _palette1;
                    break;
                case PaletteType.Palette02:
                    _palette = _palette2;
                    break;
                case PaletteType.RedOrangeGreen:
                    _palette = _palette3;
                    break;
            }

            paletteColors = _palette.Count;

            switch (report.Tipo)
            {
                case ReportType.Pie:
                    strData = String.Format("var {0} = new RGraph.Pie('{0}', [", report.Id);
                    break;
                case ReportType.HBar:
                    strData = String.Format("var {0} = new RGraph.HBar('{0}', [", report.Id);
                    break;
                case ReportType.Bar:
                    strData = String.Format("var {0} = new RGraph.Bar('{0}', [", report.Id);
                    break;
                case ReportType.Line:
                    strData = String.Format("var {0} = new RGraph.Line('{0}', [", report.Id);
                    break;
                case ReportType.Scatter:
                    strData = String.Format("var {0} = new RGraph.Scatter('{0}', [", report.Id);
                    break;
                default:
                    throw new MyManagerCSharp.MyException("tipo di report non gestito: " + report.Tipo.ToString());
            }

            strColors = String.Format("{0}.Set('colors', [", report.Id);
            strLegend = String.Format("{0}.Set('key', [", report.Id);
            strLabel = String.Format("{0}.Set('labels', [", report.Id);

            strMyKeys = String.Format("var {0}MyKeys =  [", report.Id);

            //' La varibile la dichiaro fuopri altrimenti non si vede
            //'If dataTable.Columns.Contains("my_key") Then
            //'strMyKeys = String.Format("var myKey{0} = new Array(", name)
            //'End If

            DateTime data;


            //ATTENZIONE:
            //La prima colonna è sempre la LABEL

            foreach (System.Data.DataRow row in _dt.Rows)
            {
                //strData += row["valore"] + ",";


                //[ ['2014-07-04',  4.0] ,['2014-07-05' , 1.0 ] ,['2014-07-12' ,1.0], ['2014-07-28' ,3.0]]);

                if (report.Tipo == ReportType.Scatter)
                {
                    strData += String.Format(System.Globalization.CultureInfo.GetCultureInfo("en-GB"), "['{0}',{1:0.0}],", row["label"], row["valore"]);
                }
                else
                {
                    strData += String.Format(System.Globalization.CultureInfo.GetCultureInfo("en-GB"), "{0:0.0},", row["valore"]);
                    //strData += String.Format(System.Globalization.CultureInfo.GetCultureInfo("en-GB"), "{0:0.0},", row[1]);
                }


                strLegend += String.Format("'{0}',", row["label"].ToString().Replace("'", "\'"));

                switch (report.Label)
                {
                    case LabelType.None:
                        break;
                    case LabelType.Label:
                        //28/07/2014
                        if (DateTime.TryParse(row["label"].ToString(), out data))
                        {
                            strLabel += String.Format("'{0}',", data.ToShortDateString());
                        }
                        else
                        {
                            strLabel += String.Format("'{0}',", row["label"].ToString().Replace("'", "\'"));
                        }
                        break;
                    case LabelType.Valore:
                        strLabel += String.Format("'{0}',", row["valore"].ToString().Replace("'", "\'"));
                        break;
                    case LabelType.LabelAndValore:
                        // strLabel += String.Format("'{0}',", row["label"].ToString().Replace("'", "\'") + " (" + row["valore"].ToString().Replace("'", "\'") + ")");
                        //strLabel += String.Format("'{0}',", row["label"].ToString().Replace("'", "\'"));
                        //28/07/2014
                        if (DateTime.TryParse(row["label"].ToString(), out data))
                        {
                            strLabel += String.Format("'{0}',", data.ToShortDateString());
                        }
                        else
                        {
                            strLabel += String.Format("'{0}',", row["label"].ToString().Replace("'", "\'"));
                        }
                        break;

                }


                if (OrderColor)
                {
                    strColors += String.Format("'{0}',", row["label"].ToString().Replace("'", "\'"));
                }
                else
                {
                    strColors += String.Format("'#{0}',", _palette[indexColor % paletteColors]);
                    indexColor = indexColor + 1;
                }

                totale = totale + Decimal.Parse(row["valore"].ToString());


                if (_dt.Columns.Contains("my_key"))
                {
                    strMyKeys += String.Format("'{0}',", row["my_key"].ToString().Replace("'", "\'"));
                    //'strMyKeys &= String.Format("'{0}',", row("my_key").ToString().Replace("'", "\'"))
                    // strMyKeys += String.Format(" myKey{0}[{1}] = '{2}';", report.Id, indice, row["my_key"].ToString().Replace("'", "\'"));
                    //indice = indice + 1;
                }

            }

            _totale = totale;


            if (OrderColor)
            {
                _dt.DefaultView.Sort = "valore desc";
                indexColor = 0;

                foreach (System.Data.DataRowView row in _dt.DefaultView)
                {
                    strColors = strColors.Replace(String.Format("'{0}'", row["label"].ToString().Replace("'", "\'")), String.Format("'{0}'", "#" + _palette[indexColor % paletteColors]));

                    indexColor = indexColor + 1;
                }

            }


            ////'cancello l'ultima ,
            //if (report.Tipo == ReportType.Scatter)
            //{
            //    //'cancello l'ultima ,
            //    strData = strData.Substring(0, strData.Length - 1);
            //}

            strData = strData.Substring(0, strData.Length - 1);
            strData += "]);";

            strColors = strColors.Substring(0, strColors.Length - 1);
            strColors += "]);";

            strLegend = strLegend.Substring(0, strLegend.Length - 1);
            strLegend += "]);";

            strLabel = strLabel.Substring(0, strLabel.Length - 1);
            strLabel += "]);";

            strMyKeys = strMyKeys.Substring(0, strMyKeys.Length - 1);
            strMyKeys += "];";

            strJavaScript = strData + Environment.NewLine;

            if (report.Colors != PaletteType.None)
            {
                strJavaScript += strColors + Environment.NewLine;
                strJavaScript += String.Format("{0}.set('colors.sequential', 'true');", report.Id) + Environment.NewLine;
            }



            if (report.ShowPercentuale)
            {
                decimal percentuale;

                foreach (System.Data.DataRow row in _dt.Rows)
                {
                    percentuale = 0;
                    if (decimal.Parse(row["valore"].ToString()) != 0)
                    {
                        percentuale = (decimal.Parse(row["valore"].ToString()) / totale) * 100;
                    }

                    strLabel = strLabel.Replace(String.Format("'{0}'", row["label"].ToString().Replace("'", "\'")), String.Format("'{0} {1:N2}%'", row["label"].ToString().Replace("'", "\'"), percentuale));
                    strLegend = strLegend.Replace(String.Format("'{0}'", row["label"].ToString().Replace("'", "\'")), String.Format("'{0} {1:N2}%'", row["label"].ToString().Replace("'", "\'"), percentuale));
                }
            }



            if (ShowLegend)
            {
                strJavaScript += Environment.NewLine + strLegend;
                strJavaScript += String.Format("{0}.Set('key.background', 'white');", report.Id);
            }

            if (report.Label != LabelType.None)
            {
                if (report.Label == LabelType.LabelAndValore)
                {
                    strJavaScript += String.Format("{0}.set('labels.above', 'true');", report.Id) + Environment.NewLine;
                }

                strJavaScript += strLabel + Environment.NewLine;
            }




            //' strJavaScript &= String.Format("{0}.Set('chart.gutter.left', 400);", name)
            strJavaScript += String.Format("{0}.Set('text.size', 8);", report.Id) + Environment.NewLine;
            //'strJavaScript &= String.Format("{0}.Set('chart.colors.sequential', true);", name)


            //'strJavaScript &= String.Format("{0}.Set('chart.labels.above', true);", name)

            //'strJavaScript &= String.Format("{0}.Set('chart.zoom.factor',1.5);", name)
            strJavaScript += String.Format("{0}.set('key.interactive', true);", report.Id) + Environment.NewLine;
            strJavaScript += String.Format("{0}.set('scale.thousand', '.');", report.Id) + Environment.NewLine;
            strJavaScript += String.Format("{0}.set('scale.point', ',');", report.Id) + Environment.NewLine;


            //Rutigliano 26/09/2014 il titolo lo visualizzo fuori dal report
            //if (!String.IsNullOrEmpty(report.Titolo))
            //{
            //    strJavaScript += String.Format("{0}.Set('title', '{1}');", report.Id, report.Titolo.Replace("'", "\'")) + Environment.NewLine;
            //}



            if (EnableOnClick)
            {
                //strJavaScript += String.Format("{0}.Set('events.click', {1});", report.Id, "myEventListener" + report.Id) + Environment.NewLine;
                strJavaScript += String.Format("{0}.Set('events.click', {1});", report.Id, report.Id + "OnClickEventListener") + Environment.NewLine;
                strJavaScript += String.Format("{0}.Set('events.mousemove', function (e, bar) {{e.target.style.cursor = 'pointer';}} );", report.Id) + Environment.NewLine;
                //'strJavaScript &= String.Format("{0}.Set('chart.events.mousemove', myMousemove );", name)
            }


            foreach (string custom in report.Settings)
            {
                strJavaScript += custom + Environment.NewLine;
            }



            //'strJavaScript &= String.Format("{0}.Set('chart.key.position', 'gutter');", name)
            strJavaScript += String.Format("{0}.Draw();", report.Id) + Environment.NewLine;


            if (_dt.Columns.Contains("my_key"))
            {
                //strJavaScript += strMyKeys + Environment.NewLine;
                report.MyKeys = strMyKeys;
            }


            report.Html = strJavaScript + Environment.NewLine;
        }


        //public string getChart(string name, RGraphManager.ReportType tipo, System.Data.DataTable dataTable, bool rotateDataSet, bool includeTable)
        //{

        //    //'If isIndicatore(tipo) Then
        //    //'    Throw New MyManager.ManagerException("Con questi parametri NON è possibile creare report di tipo indicatore")
        //    //'End If

        //    //'If (rotateDataSet) Then
        //    //'    dataTable = rotateRigheToColonne(dataTable)
        //    //'End If


        //    _dt = dataTable;


        //    int indexColor = 0;

        //    string strJavaScript = "";
        //    string strData = "";
        //    string strColors = "";
        //    string strLegend = "";
        //    string strLabel = "";

        //    string strMyKeys = "";


        //    decimal totale = 0;
        //    int paletteColors;

        //    paletteColors = _palette1.Count;

        //    switch (tipo)
        //    {
        //        case ReportType.Pie:
        //            strData = String.Format("var {0} = new RGraph.Pie('{0}', [", name);
        //            break;
        //        case ReportType.HBar:
        //            strData = String.Format("var {0} = new RGraph.HBar('{0}', [", name);
        //            break;
        //        case ReportType.Bar:
        //            strData = String.Format("var {0} = new RGraph.Bar('{0}', [", name);
        //            break;
        //        default:
        //            throw new MyManagerCSharp.MyException("tipo di report non gestito: " + tipo.ToString());
        //    }



        //    strColors = String.Format("{0}.Set('colors', [", name);
        //    strLegend = String.Format("{0}.Set('key', [", name);
        //    strLabel = String.Format("{0}.Set('labels', [", name);

        //    //' La varibile la dichiaro fuopri altrimenti non si vede
        //    //'If dataTable.Columns.Contains("my_key") Then
        //    //'strMyKeys = String.Format("var myKey{0} = new Array(", name)
        //    //'End If


        //    long indice = 0;


        //    foreach (System.Data.DataRow row in _dt.Rows)
        //    {
        //        strData += row["valore"] + ",";

        //        strLegend += String.Format("'{0}',", row["label"].ToString().Replace("'", "\'"));

        //        switch (Label)
        //        {

        //            case LabelType.None:
        //                break;
        //            case LabelType.Label:
        //                strLabel += String.Format("'{0}',", row["label"].ToString().Replace("'", "\'"));
        //                break;
        //            case LabelType.Valore:
        //                strLabel += String.Format("'{0}',", row["valore"].ToString().Replace("'", "\'"));
        //                break;
        //            case LabelType.LabelAndValore:
        //                strLabel += String.Format("'{0}',", row["label"].ToString().Replace("'", "\'") + " (" + row["valore"].ToString().Replace("'", "\'") + ")");
        //                break;

        //        }


        //        if (OrderColor)
        //        {
        //            strColors += String.Format("'{0}',", row["label"].ToString().Replace("'", "\'"));
        //        }
        //        else
        //        {
        //            strColors += String.Format("'#{0}',", _palette[indexColor % paletteColors]);
        //            indexColor = indexColor + 1;
        //        }

        //        totale = totale + Decimal.Parse(row["valore"].ToString());


        //        if (_dt.Columns.Contains("my_key"))
        //        {
        //            //'strMyKeys &= String.Format("'{0}',", row("my_key").ToString().Replace("'", "\'"))
        //            strMyKeys += String.Format(" myKey{0}[{1}] = '{2}';", name, indice, row["my_key"].ToString().Replace("'", "\'"));
        //            indice = indice + 1;
        //        }

        //    }

        //    _totale = totale;


        //    if (OrderColor)
        //    {
        //        _dt.DefaultView.Sort = "valore desc";
        //        indexColor = 0;

        //        foreach (System.Data.DataRowView row in _dt.DefaultView)
        //        {
        //            strColors = strColors.Replace(String.Format("'{0}'", row["label"].ToString().Replace("'", "\'")), String.Format("'{0}'", "#" + _palette[indexColor % paletteColors]));

        //            indexColor = indexColor + 1;
        //        }

        //    }


        //    //'cancello l'ultima ,
        //    strData = strData.Substring(0, strData.Length - 1);
        //    strData += "]);";

        //    strColors = strColors.Substring(0, strColors.Length - 1);
        //    strColors += "]);";

        //    strLegend = strLegend.Substring(0, strLegend.Length - 1);
        //    strLegend += "]);";

        //    strLabel = strLabel.Substring(0, strLabel.Length - 1);
        //    strLabel += "]);";

        //    strJavaScript = strData + Environment.NewLine;

        //    if (SetPalette != PaletteType.None)
        //    {
        //        strJavaScript = strColors + Environment.NewLine;
        //    }



        //    if (ShowPercentuale)
        //    {
        //        foreach (System.Data.DataRow row in _dt.Rows)
        //        {
        //            strLabel = strLabel.Replace(String.Format("'{0}'", row["label"].ToString().Replace("'", "\'")), String.Format("'{0} {1:N2}%'", row["label"].ToString().Replace("'", "\'"), (decimal.Parse(row["valore"].ToString()) / totale) * 100));
        //            strLegend = strLegend.Replace(String.Format("'{0}'", row["label"].ToString().Replace("'", "\'")), String.Format("'{0} {1:N2}%'", row["label"].ToString().Replace("'", "\'"), (decimal.Parse(row["valore"].ToString()) / totale) * 100));
        //        }
        //    }



        //    if (ShowLegend)
        //    {
        //        strJavaScript += Environment.NewLine + strLegend;
        //        strJavaScript += String.Format("{0}.Set('key.background', 'white');", name);
        //    }

        //    if (ShowLabels)
        //    {
        //        strJavaScript += Environment.NewLine + strLabel;
        //    }



        //    //' strJavaScript &= String.Format("{0}.Set('chart.gutter.left', 400);", name)
        //    strJavaScript += String.Format("{0}.Set('text.size', 8);", name) + Environment.NewLine;
        //    //'strJavaScript &= String.Format("{0}.Set('chart.colors.sequential', true);", name)


        //    //'strJavaScript &= String.Format("{0}.Set('chart.labels.above', true);", name)

        //    //'strJavaScript &= String.Format("{0}.Set('chart.zoom.factor',1.5);", name)
        //    strJavaScript += String.Format("{0}.Set('key.interactive', true);", name) + Environment.NewLine;


        //    if (!String.IsNullOrEmpty(_titolo))
        //    {
        //        strJavaScript += String.Format("{0}.Set('title', '{1}');", name, _titolo.Replace("'", "\'")) + Environment.NewLine;
        //    }



        //    if (EnableOnClick)
        //    {
        //        strJavaScript += String.Format("{0}.Set('events.click', {1});", name, "myEventListener" + name) + Environment.NewLine;
        //        strJavaScript += String.Format("{0}.Set('events.mousemove', function (e, bar) {{e.target.style.cursor = 'pointer';}} );", name) + Environment.NewLine;
        //        //'strJavaScript &= String.Format("{0}.Set('chart.events.mousemove', myMousemove );", name)
        //    }

        //    //'strJavaScript &= String.Format("{0}.Set('chart.key.position', 'gutter');", name)
        //    strJavaScript += String.Format("{0}.Draw();", name) + Environment.NewLine;


        //    if (_dt.Columns.Contains("my_key"))
        //    {
        //        //' strMyKeys = strMyKeys.Substring(0, strMyKeys.Length - 1)
        //        //'strMyKeys &= ");"
        //        strJavaScript += strMyKeys + Environment.NewLine;
        //    }

        //    return strJavaScript;
        //}
    }

}
