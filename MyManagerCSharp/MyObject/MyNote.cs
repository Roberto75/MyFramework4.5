using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp.MyObject
{
    public class MyNote
    {

        public string OutolookId { get; set; }
        public string GoogleId { get; set; }

        public string Titolo { get; set; }
        public string Contenuto { get; set; }
        public string Folder { get; set; }
        public string FolderId { get; set; }
        public DateTime DateUpdated { get; set; }

    }
}
