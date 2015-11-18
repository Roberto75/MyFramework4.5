using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp.RGraph.Models
{
    public class RGraphMeter
    {
        public string Id { get; set; }
        public string Titolo { get; set; }
        public string SQL { get; set; }

        public double? Valore { get; set; }

        public string Width { get; set; }
        public string Height { get; set; }


        public RGraphMeter()
        {
            Height = "200px";
            Width = "400px";
        }
    }
}
