using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyUsers.Models
{
    public class SearchGroups : PagedGroups
    {
        public MyGroup filter { get; set; }
        public List<MyManagerCSharp.Models.MyItem> ListaTipi { get; set; }

        public SearchGroups()
        {
            filter = new MyGroup();

        }
    }
}
