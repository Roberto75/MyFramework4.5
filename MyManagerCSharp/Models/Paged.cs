using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Shared.Models
{
    public class Paged
    {
        public int PageNumber { get; set; }
        public int TotalRows { get; set; }
        public int PageSize { get; set; }

        public int TotalPages
        {
            get
            {
                if (PageSize == 0)
                {
                    return 0;
                }

                int temp;
                temp = (TotalRows + PageSize - 1) / PageSize;

                return temp;
            }
        }
        public bool HasPreviousPage { get { return (PageNumber > 1); } }
        public bool HasNextPage { get { return (PageNumber < TotalPages); } }

        public string Sort { get; set; }
        public string SortDir { get; set; }

        public Paged()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
        }
    }
}
