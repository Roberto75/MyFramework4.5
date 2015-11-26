using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Diagnostics;

namespace UnitTestProject
{
    [TestClass]
    public class TestWordToPdf
    {
        [TestMethod]
        public void TestMethod1()
        {
            string sorceFileWord;

            sorceFileWord = @"C:\Develop.NET\EarlyWarning\public\Documentazione\Applicativi_Sicuri_Capp_3_e_4.docx";

            My.Pdf.WordToPdf pdf = new My.Pdf.WordToPdf();

            FileInfo output;
            output = pdf.convert(new FileInfo(sorceFileWord), new DirectoryInfo(@"C:\Temp\"));

            Debug.WriteLine(output.FullName);

        }
    }
}
