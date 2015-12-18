using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp.Log.Models
{
    public class MyLogDetail
    {
        public List<MyManagerCSharp.Log.Models.MyLog> Logs { get; set; }
        public List<LogManager.Level> LevelSelected { get; set; }

        public string sessionId { get; set; }

        public MyLogDetail()
        {
            LevelSelected = new List<LogManager.Level>();

            //foreach (var value in Enum.GetValues(typeof(LogManager.Level)))
            //{
            //    LevelSelected.Add((LogManager.Level)value);
            //}

        }
    }
}
