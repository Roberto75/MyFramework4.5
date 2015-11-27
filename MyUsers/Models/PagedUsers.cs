using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyUsers.Models
{
    public class PagedUsers : My.Shared.Models.Paged
    {
        public List<MyUser> Utenti { get; set; }
    }

        
}
