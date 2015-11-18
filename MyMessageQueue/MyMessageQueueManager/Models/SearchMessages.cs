using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.MessageQueue.Models
{
    public class SearchMessages: PagedMessages
    {
        //public MyUser filter = new MyUser();

        public SearchMessages()
        {
            this.Sort = "date_added";
            this.SortDir = "ASC";
        }
    }
}
