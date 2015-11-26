using My.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {

            string sorceFileWord;

            sorceFileWord = @"C:\Develop.NET\EarlyWarning\public\Documentazione\Applicativi_Sicuri_Capp_3_e_4.docx";

            WordToPdf pdf = new WordToPdf();

            FileInfo output;
            output = pdf.convert(new FileInfo(sorceFileWord), new DirectoryInfo(@"C:\Temp\"));

            Debug.WriteLine(output.FullName);

        }
    }
}
