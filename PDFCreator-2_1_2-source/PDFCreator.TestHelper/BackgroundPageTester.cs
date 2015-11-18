﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using NUnit.Framework;
using pdfforge.PDFCreator.Core.Jobs;

namespace PDFCreator.TestUtilities
{
    public class BackgroundPageTester
    {
        public static void BackgroundOnPage(IJob job, int pageNumber = 1)
        {
            BackgroundOnPage(job.OutputFiles[0], pageNumber);
        }   
     
        public static void BackgroundOnPage(string pdfFile, int pageNumber=1)
        {
            var reader = new PdfReader(pdfFile);
            string pageText = PdfTextExtractor.GetTextFromPage(reader, pageNumber).Replace("\n", "").Replace(" ", "").Replace("1", "");
            Assert.IsTrue(pageText.Contains("Background"), "Did not add background to " + pageNumber + ". page of document.");
        }
    }
}
