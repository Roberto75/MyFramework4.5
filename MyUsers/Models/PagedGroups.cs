﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyUsers.Models
{
    public class PagedGroups : MyManagerCSharp.Models.Paged
    {
        public List<MyGroup> Gruppi { get; set; }
    }

}
