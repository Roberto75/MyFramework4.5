using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp.Models
{
    public  class MyCheckItem
    {
        public enum StatusType {
            Passed
            ,Failed
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public StatusType Status { get; set; }
    }
}
