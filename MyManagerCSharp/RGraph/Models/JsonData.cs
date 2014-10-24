using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MyManagerCSharp.RGraph.Models
{
    public class JsonData
    {
        public List<string> Labels { get; set; }
        public List<decimal> Data { get; set; }
        public List<string> Tooltips { get; set; }

        public JsonData(DataTable dataTable)
        {
            Labels = new List<string>();
            Data = new List<decimal>();
            Tooltips = new List<string>();

            foreach (DataRow row in dataTable.Rows)
            {
                Labels.Add (row["label"].ToString());
                Data.Add(decimal.Parse (row["data"].ToString()));

                if (dataTable.Columns.Contains("tooltip"))
                {
                    Tooltips.Add(row["tooltip"].ToString());
                }
            }

        }

    }
}
