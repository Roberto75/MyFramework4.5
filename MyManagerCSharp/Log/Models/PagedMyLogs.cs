using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp.Log.Models
{
    public class PagedMyLogs : MyManagerCSharp.Models.Paged 
    {
        public IEnumerable<MyLog> LogsList { get; set; }
    }
}
