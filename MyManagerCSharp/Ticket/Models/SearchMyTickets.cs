using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp.Ticket.Models
{
    public class SearchMyTickets : PagedMyTicket
    {
        public TicketManager.TicketStatus ticketStatus { get; set; }
        public string SourceId { get; set; }
        public List<string> SourceList { get; set; }
    }
}
