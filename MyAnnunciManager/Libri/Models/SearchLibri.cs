using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annunci.Libri.Models
{
    public class SearchLibri : PagedLibri
    {

        public Libro filter { get; set; }

        public MyManagerCSharp.ManagerDB.Days? days { get; set; }



        public List<MyManagerCSharp.Models.MyItem> comboCategorie { get; set; }
        public List<MyManagerCSharp.Models.MyItem> comboSubCategorie { get; set; }
        public List<MyManagerCSharp.Models.MyItem> comboRegioni { get; set; }

        public SearchLibri()
        {

            //Sort = "ANNUNCIO.date_added desc,  titolo";
            Sort = "ANNUNCIO.date_added";
            SortDir = "desc";
            days = MyManagerCSharp.ManagerDB.Days.Tutti;
            filter = new Models.Libro();

            comboSubCategorie = new List<MyManagerCSharp.Models.MyItem>();
        }
    }
}
