using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp.Log.Models
{
    public class SearchMyLogs : PagedMyLogs
    {
        public MyLog filter { get; set; }
        public List<LogManager.Level> levelSelected { get; set; }
        public string[] myType { get; set; }
        public List<string> myTypeSelected { get; set; }

        public LogManager.Days Days { get; set; }
        

        public SearchMyLogs()
        {
            this.Sort = "date_added";
            this.SortDir = "DESC";

            Days = LogManager.Days.Oggi;

            filter = new MyLog();

            levelSelected = new List<LogManager.Level>();
            levelSelected.Add(LogManager.Level.Debug);
            levelSelected.Add(LogManager.Level.Error);
            levelSelected.Add(LogManager.Level.Info);
            levelSelected.Add(LogManager.Level.Warning);
            levelSelected.Add(LogManager.Level.Exception);


            myTypeSelected = new List<string>();

        }

    }
}
