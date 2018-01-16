using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annunci.Libri.Models
{
    public class PagedLibri : My.Shared.Models.Paged
    {
        public IEnumerable<Libro> Libri { get; set; }
    }
}
