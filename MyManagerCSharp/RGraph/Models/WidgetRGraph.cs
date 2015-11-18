using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp.RGraph.Models
{
    public class WidgetRGraph
    {
        public List<RGraphMeter> RGraphMeter { get; set; }
        public List<RGraphModel> RGraph { get; set; }

        //Filtro data
        public MyManagerCSharp.ManagerDB.Days Days { get; set; }

        public WidgetRGraph()
        {

            RGraph = new List<RGraphModel>();
            RGraphMeter = new List<RGraphMeter>();

        }
    }
}
