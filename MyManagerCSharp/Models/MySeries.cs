using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MyManagerCSharp.Models
{
    public class MySeries
    {
        public string Label { get; set; }
        public List<MyItemV2 > Values { get; set; }


        public MySeries(string label)
        {
            Values = new List<MyItemV2>();
            Label = label;
        }


        public string getArrayValues()
        {
            var query = from v in Values select v.getValue()  ;
            
            string temp;
            temp = string.Join(",", query.ToArray());

            return String.Format("[{0}]", temp);
        }

        public string getArrayLabels()
        {
            var query = from v in Values select "'" + v.Text.Replace("'", "\\'")  + "'";

            //foreach (string s in query)
            //{
            //    Debug.WriteLine(s.Replace("'", "gggg"));

            //}

            string temp;
            temp = string.Join(",", query.ToArray());

            
            return String.Format("[{0}]", temp);
        }


    }
}
