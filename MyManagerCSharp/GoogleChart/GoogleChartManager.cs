using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp.GoogleChart
{
    public class GoogleChartManager: ManagerDB
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


         public GoogleChartManager(string connectionName)
            : base(connectionName)
        {
            
           // _palette = _palette1;

        }
         public GoogleChartManager(System.Data.Common.DbConnection connection)
            : base(connection)
        {
           
           // _palette = _palette1;
        }
    
    }
}
