﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyUsers.Models
{
    public class PagedUsers : MyManagerCSharp.Models.Paged
    {
        public IEnumerable<MyUser> Utenti { get; set; }
    }

        
}
