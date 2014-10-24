using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp
{
   public  class StringManager
    {

       public static string formatLabelByte(long value)
       {

           if (value == 0)
           {
               return "0 GB";
           }

           decimal size;

           // KB 1024 
           // MB 1024 * 1024 = 1048576
           // GB 1024 * 1024 * 1024 =   1073741824

           size = (decimal)value / 1073741824;
           if ((uint)size > 0)
           {
               return String.Format("{0:0.00} GB", size);
           }

           size = (decimal)value / 1048576;
           if ((uint)size > 0)
           {
               return String.Format("{0:0.00} MB", size); ;
           }

           size = (decimal)value / 1024;
           if ((uint)size > 0)
           {
               return String.Format("{0:0.00} KB", size); ;
           }

           return value + " B";
       }
    }
}
