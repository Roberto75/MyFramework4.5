using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp.Models
{
    public class MyItem
    {
        public string Value { get; set; }
        public string Text { get; set; }

        public MyItem()
        {
        }

        public MyItem(string value, string text)
        {
            Value = value;
            Text = text;
        }

    }
}
