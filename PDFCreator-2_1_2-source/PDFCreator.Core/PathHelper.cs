using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms.VisualStyles;

namespace pdfforge.PDFCreator.Core
{
    public class PathHelper
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetShortPathName(
                 [MarshalAs(UnmanagedType.LPTStr)]
                   string path,
                 [MarshalAs(UnmanagedType.LPTStr)]
                   StringBuilder shortPath,
                 int shortPathLength
                 );

        public static string GetShortPathName(string path)
        {        
            StringBuilder buffer = new StringBuilder(256);
            GetShortPathName(path, buffer, buffer.Capacity);

            return buffer.ToString();
        }
    }
}
