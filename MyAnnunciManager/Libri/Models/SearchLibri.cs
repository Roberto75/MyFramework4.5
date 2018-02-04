using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annunci.Libri.Models
{
    public class SearchLibri : PagedLibri
    {


        public int countFilter() {
            int conta = 0;

            if (days != MyManagerCSharp.ManagerDB.Days.Tutti)
            {
                conta++;
            }

            if (!String.IsNullOrEmpty(filter.titolo))
            {
                conta++;
            }

            if (!String.IsNullOrEmpty(filter.autore))
            {
                conta++;
            }

            if (!String.IsNullOrEmpty(filter.isbn))
            {
                conta++;
            }

            if (filter.tipo != null)
            {
                conta++;
            }

            if (filter.categoriaId != null)
            {
                conta++;
            }

            if (filter.subCategoriaId != null)
            {
                conta++;
            }

            if (filter.regioneId != null)
            {
                conta++;
            }

            if (filter.provinciaId != null)
            {
                conta++;
            }

            if (filter.comuneId != null)
            {
                conta++;
            }
            return conta;
        }

        public int collapseShow { get; set; }
        public Libro filter { get; set; }
        public MyManagerCSharp.ManagerDB.Days? days { get; set; }
        public List<MyManagerCSharp.Models.MyItem> comboCategorie { get; set; }
        public List<MyManagerCSharp.Models.MyItem> comboSubCategorie { get; set; }
        public List<MyManagerCSharp.Models.MyItem> comboRegioni { get; set; }
        public List<MyManagerCSharp.Models.MyItem> comboProvince { get; set; }
        public List<MyManagerCSharp.Models.MyItem> comboComuni { get; set; }

        public SearchLibri()
        {
            collapseShow = 0;

            //Sort = "ANNUNCIO.date_added desc,  titolo";
            Sort = "ANNUNCIO.date_added";
            SortDir = "desc";
            days = MyManagerCSharp.ManagerDB.Days.Tutti;
            filter = new Models.Libro();

            comboSubCategorie = new List<MyManagerCSharp.Models.MyItem>();
            comboProvince = new List<MyManagerCSharp.Models.MyItem>();
            comboComuni = new List<MyManagerCSharp.Models.MyItem>();
        }
    }
}
