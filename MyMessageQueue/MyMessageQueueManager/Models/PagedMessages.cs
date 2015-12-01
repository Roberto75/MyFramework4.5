using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.MessageQueue.Models
{
    public class PagedMessages : My.Shared.Models.Paged
    {
        public List<MessageBase> Messages { get; set; }

    }
}
