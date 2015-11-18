using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp.RGraph.Models
{
    public class RGraphModel
    {
        public string Id { get; set; }
        public string Titolo { get; set; }

        public string PngFileName { get; set; }

        public string SQL { get; set; } 


      //  public bool includeTitoloInChart { get; set; }
        public string Html { get; set; }
        public string MyKeys { get; set; }

        public bool EnableOnClick { get; set; }
        public bool ShowLegend { get; set; }
        public bool ShowPercentuale { get; set; }
        public bool ShowFiltroData { get; set; }

        public System.Data.DataTable Data { get; set; }

        public List<MyManagerCSharp.Models.MySeries> Series { get; set; }


        public RGraph.RGraphManager.ReportType Tipo { get; set; }
        public RGraph.RGraphManager.LabelType Label { get; set; }
        public RGraph.RGraphManager.PaletteType Colors { get; set; }

        public string Width { get; set; }
        public string Height { get; set; }

        public MyManagerCSharp.ManagerDB.Days? Days { get; set; }


        public List<string> Settings { get; set; }

        public RGraphModel()
        {
            Settings = new List<string>();
            ShowPercentuale = false;
            ShowFiltroData = false;
            Series = new List<MyManagerCSharp.Models.MySeries>();
        }


    }
}
