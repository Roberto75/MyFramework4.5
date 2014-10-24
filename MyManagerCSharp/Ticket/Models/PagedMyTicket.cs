using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp.Ticket.Models
{
    public class PagedMyTicket : MyManagerCSharp.Models.Paged  
    {
        public List<MyTicket> TicketsList { get; set; }
    }
}
