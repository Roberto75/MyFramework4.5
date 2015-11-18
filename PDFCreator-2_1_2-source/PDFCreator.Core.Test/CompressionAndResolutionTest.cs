﻿using System;
using System.Globalization;
using iTextSharp.text.pdf;
using NUnit.Framework;
using iTextSharp.text.pdf.parser;
using PDFCreator.TestUtilities;
using pdfforge.PDFCreator.Core.Settings.Enums;

namespace PDFCreator.Core.Test
{
    [TestFixture]
    [Category("LongRunning")]
    internal class CompressionAndResolutionTest
    {
        private TestHelper _th;

        [SetUp]
        public void SetUp()
        {
            _th = new TestHelper("CompressionAndResolutionTest");
        }

        [TearDown]
        public void CleanUp()
        {
            _th.CleanUp();
        }

        [Test]
        public void NoCompressionNoResample()
        {
            _th.Profile.PdfSettings.CompressColorAndGray.Enabled = false;
            _th.Profile.PdfSettings.CompressColorAndGray.Compression = CompressionColorAndGray.JpegManual;
            _th.Profile.PdfSettings.CompressColorAndGray.JpegCompressionFactor = 25;
            _th.Profile.PdfSettings.CompressColorAndGray.Resampling = true;
            _th.Profile.PdfSettings.CompressColorAndGray.Dpi = 8;

            _th.Profile.PdfSettings.CompressMonochrome.Enabled = false;
            _th.Profile.PdfSettings.CompressMonochrome.Compression = CompressionMonochrome.CcittFaxEncoding;
            _th.Profile.PdfSettings.CompressMonochrome.Resampling = true;
            _th.Profile.PdfSettings.CompressMonochrome.Dpi = 8;

            _th.GenerateGsJob(PSfiles.PDFCreatorTestpage, OutputFormat.Pdf);

            _th.RunGsJob();

            var pdf = new PdfReader(_th.Job.OutputFiles[0]);

            PdfDictionary pg = pdf.GetPageN(1);
            var res = (PdfDictionary) PdfReader.GetPdfObject(pg.Get(PdfName.RESOURCES));
            var xobj = (PdfDictionary) PdfReader.GetPdfObject(res.Get(PdfName.XOBJECT));

            var testResultsFilter = new String[4];
            var testResultsWidth = new String[4];
            var testResultsSize = new String[4];

            int i = 0;
            foreach (PdfName name in xobj.Keys)
            {
                PdfObject obj = xobj.Get(name);
                if (obj.IsIndirect())
                {
                    var tg = (PdfDictionary) PdfReader.GetPdfObject(obj);

                    if (tg.Get(PdfName.FILTER) != null)
                        testResultsFilter[i] = tg.Get(PdfName.FILTER).ToString();
                    
                    testResultsWidth[i] = tg.Get(PdfName.WIDTH).ToString();
                    
                    string height = tg.Get(PdfName.HEIGHT).ToString();
                    ImageRenderInfo imgRI =
                        ImageRenderInfo.CreateForXObject(
                            new Matrix(float.Parse(testResultsWidth[i]), float.Parse(height)), (PRIndirectReference) obj, tg);
                    testResultsSize[i] = imgRI.GetImage()
                        .GetImageAsBytes()
                        .Length.ToString(CultureInfo.InvariantCulture);
                    i++;
                }
            }

            Assert.AreEqual(null, testResultsFilter[0]);
            Assert.AreEqual(null, testResultsFilter[1]);
            Assert.AreEqual(null, testResultsFilter[2]);
            Assert.AreEqual(null, testResultsFilter[3]);

            Assert.AreEqual("200", testResultsWidth[0]);
            Assert.AreEqual("200", testResultsWidth[1]);
            Assert.AreEqual("200", testResultsWidth[2]);
            Assert.AreEqual("475", testResultsWidth[3]);

            Assert.AreEqual("1995", testResultsSize[0]);
            Assert.AreEqual("29961", testResultsSize[1]);
            Assert.AreEqual("89669", testResultsSize[2]);
            Assert.AreEqual("11481", testResultsSize[3]);
        }

        [Test]
        public void CompressionZipNoResample()
        {
            _th.Profile.PdfSettings.CompressColorAndGray.Enabled = true;
            _th.Profile.PdfSettings.CompressColorAndGray.Compression = CompressionColorAndGray.Zip;
            _th.Profile.PdfSettings.CompressColorAndGray.JpegCompressionFactor = 25;
            _th.Profile.PdfSettings.CompressColorAndGray.Resampling = false;
            _th.Profile.PdfSettings.CompressColorAndGray.Dpi = 24;

            _th.Profile.PdfSettings.CompressMonochrome.Enabled = true;
            _th.Profile.PdfSettings.CompressMonochrome.Compression = CompressionMonochrome.Zip;
            _th.Profile.PdfSettings.CompressMonochrome.Resampling = false;
            _th.Profile.PdfSettings.CompressMonochrome.Dpi = 24;

            _th.GenerateGsJob(PSfiles.PDFCreatorTestpage, OutputFormat.Pdf);

            _th.RunGsJob();

            var pdf = new PdfReader(_th.Job.OutputFiles[0]);

            var pg = pdf.GetPageN(1);
            var res = (PdfDictionary) PdfReader.GetPdfObject(pg.Get(PdfName.RESOURCES));
            var xobj = (PdfDictionary) PdfReader.GetPdfObject(res.Get(PdfName.XOBJECT));

            var testResultsFilter = new string[4];
            var testResultsWidth = new string[4];
            var testResultsSize = new string[4];

            int i = 0;
            foreach (PdfName name in xobj.Keys)
            {
                PdfObject obj = xobj.Get(name);
                if (obj.IsIndirect())
                {
                    var tg = (PdfDictionary)PdfReader.GetPdfObject(obj);

                    if (tg.Get(PdfName.FILTER) != null)
                        testResultsFilter[i] = tg.Get(PdfName.FILTER).ToString();

                    testResultsWidth[i] = tg.Get(PdfName.WIDTH).ToString();

                    string height = tg.Get(PdfName.HEIGHT).ToString();
                    ImageRenderInfo imgRI =
                        ImageRenderInfo.CreateForXObject(
                            new Matrix(float.Parse(testResultsWidth[i]), float.Parse(height)), (PRIndirectReference)obj, tg);
                    testResultsSize[i] = imgRI.GetImage()
                        .GetImageAsBytes()
                        .Length.ToString(CultureInfo.InvariantCulture);
                    i++;
                }
            }

            Assert.AreEqual("/FlateDecode", testResultsFilter[0]);
            Assert.AreEqual("/FlateDecode", testResultsFilter[1]);
            Assert.AreEqual("/FlateDecode", testResultsFilter[2]);
            Assert.AreEqual("/FlateDecode", testResultsFilter[3]);

            Assert.AreEqual("200", testResultsWidth[0]);
            Assert.AreEqual("200", testResultsWidth[1]);
            Assert.AreEqual("200", testResultsWidth[2]);
            Assert.AreEqual("475", testResultsWidth[3]);

            Assert.AreEqual("1995", testResultsSize[0]);
            Assert.AreEqual("29961", testResultsSize[1]);
            Assert.AreEqual("89669", testResultsSize[2]);
            Assert.AreEqual("11481", testResultsSize[3]);
        }

        [Test]
        public void CompressionZipFaxResample24Dpi()
        {
            _th.Profile.PdfSettings.CompressColorAndGray.Enabled = true;
            _th.Profile.PdfSettings.CompressColorAndGray.Compression = CompressionColorAndGray.Zip;
            _th.Profile.PdfSettings.CompressColorAndGray.JpegCompressionFactor = 25;
            _th.Profile.PdfSettings.CompressColorAndGray.Resampling = true;
            _th.Profile.PdfSettings.CompressColorAndGray.Dpi = 24;

            _th.Profile.PdfSettings.CompressMonochrome.Enabled = true;
            _th.Profile.PdfSettings.CompressMonochrome.Compression = CompressionMonochrome.CcittFaxEncoding;
            _th.Profile.PdfSettings.CompressMonochrome.Resampling = true;

            _th.Profile.PdfSettings.CompressMonochrome.Dpi = 24;

            _th.GenerateGsJob(PSfiles.PDFCreatorTestpage, OutputFormat.Pdf);

            _th.RunGsJob();

            var pdf = new PdfReader(_th.Job.OutputFiles[0]);

            PdfDictionary pg = pdf.GetPageN(1);
            var res = (PdfDictionary) PdfReader.GetPdfObject(pg.Get(PdfName.RESOURCES));
            var xobj = (PdfDictionary) PdfReader.GetPdfObject(res.Get(PdfName.XOBJECT));

            var testResultsFilter = new string[4];
            var testResultsWidth = new string[4];
            var testResultsSize = new string[4];

            int i = 0;
            foreach (PdfName name in xobj.Keys)
            {
                PdfObject obj = xobj.Get(name);
                if (obj.IsIndirect())
                {
                    var tg = (PdfDictionary)PdfReader.GetPdfObject(obj);

                    if (tg.Get(PdfName.FILTER) != null)
                        testResultsFilter[i] = tg.Get(PdfName.FILTER).ToString();

                    testResultsWidth[i] = tg.Get(PdfName.WIDTH).ToString();

                    string height = tg.Get(PdfName.HEIGHT).ToString();
                    ImageRenderInfo imgRI =
                        ImageRenderInfo.CreateForXObject(
                            new Matrix(float.Parse(testResultsWidth[i]), float.Parse(height)), (PRIndirectReference)obj, tg);
                    testResultsSize[i] = imgRI.GetImage()
                        .GetImageAsBytes()
                        .Length.ToString(CultureInfo.InvariantCulture);
                    i++;
                }
            }

            Assert.AreEqual("/CCITTFaxDecode", testResultsFilter[0]);
            Assert.AreEqual("/FlateDecode", testResultsFilter[1]);
            Assert.AreEqual("/FlateDecode", testResultsFilter[2]);
            Assert.AreEqual("/FlateDecode", testResultsFilter[3]);

            Assert.AreEqual("48", testResultsWidth[0]);
            Assert.AreEqual("48", testResultsWidth[1]);
            Assert.AreEqual("48", testResultsWidth[2]);
            Assert.AreEqual("79", testResultsWidth[3]);

            Assert.AreEqual("289", testResultsSize[0]);
            Assert.AreEqual("1930", testResultsSize[1]);
            Assert.AreEqual("5548", testResultsSize[2]);
            Assert.AreEqual("634", testResultsSize[3]);
        }

        [Test]
        public void CompressionFactor25RunLengthResample24Dpi()
        {
            _th.Profile.PdfSettings.CompressColorAndGray.Enabled = true;
            _th.Profile.PdfSettings.CompressColorAndGray.Compression = CompressionColorAndGray.JpegManual;
            _th.Profile.PdfSettings.CompressColorAndGray.JpegCompressionFactor = 25;
            _th.Profile.PdfSettings.CompressColorAndGray.Resampling = true;
            _th.Profile.PdfSettings.CompressColorAndGray.Dpi = 24;

            _th.Profile.PdfSettings.CompressMonochrome.Enabled = false;
            _th.Profile.PdfSettings.CompressMonochrome.Compression = CompressionMonochrome.RunLengthEncoding;
            _th.Profile.PdfSettings.CompressMonochrome.Resampling = true;
            _th.Profile.PdfSettings.CompressMonochrome.Dpi = 24;

            _th.GenerateGsJob(PSfiles.PDFCreatorTestpage, OutputFormat.Pdf);

            _th.RunGsJob();

            var pdf = new PdfReader(_th.Job.OutputFiles[0]);

            PdfDictionary pg = pdf.GetPageN(1);
            var res = (PdfDictionary) PdfReader.GetPdfObject(pg.Get(PdfName.RESOURCES));
            var xobj = (PdfDictionary) PdfReader.GetPdfObject(res.Get(PdfName.XOBJECT));

            var testResultsFilter = new string[4];
            var testResultsWidth = new string[4];
            var testResultsSize = new string[4];

            int i = 0;
            foreach (PdfName name in xobj.Keys)
            {
                PdfObject obj = xobj.Get(name);
                if (obj.IsIndirect())
                {
                    var tg = (PdfDictionary)PdfReader.GetPdfObject(obj);

                    if (tg.Get(PdfName.FILTER) != null)
                        testResultsFilter[i] = tg.Get(PdfName.FILTER).ToString();

                    testResultsWidth[i] = tg.Get(PdfName.WIDTH).ToString();

                    string height = tg.Get(PdfName.HEIGHT).ToString();
                    ImageRenderInfo imgRI =
                        ImageRenderInfo.CreateForXObject(
                            new Matrix(float.Parse(testResultsWidth[i]), float.Parse(height)), (PRIndirectReference)obj, tg);
                    testResultsSize[i] = imgRI.GetImage()
                        .GetImageAsBytes()
                        .Length.ToString(CultureInfo.InvariantCulture);
                    i++;
                }
            }

            Assert.AreEqual(null, testResultsFilter[0]);
            Assert.AreEqual("/DCTDecode", testResultsFilter[1]);
            Assert.AreEqual("/DCTDecode", testResultsFilter[2]);
            Assert.AreEqual("/DCTDecode", testResultsFilter[3]);

            Assert.AreEqual("200", testResultsWidth[0]);
            Assert.AreEqual("48", testResultsWidth[1]);
            Assert.AreEqual("48", testResultsWidth[2]);
            Assert.AreEqual("79", testResultsWidth[3]);

            Assert.AreEqual("1995", testResultsSize[0]);
            Assert.AreEqual("387", testResultsSize[1]);
            Assert.AreEqual("728", testResultsSize[2]);
            Assert.AreEqual("662", testResultsSize[3]);
        }

        [Test]
        public void CompressionFactorJpegMaximum_NoResample()
        {
            _th.Profile.PdfSettings.CompressColorAndGray.Enabled = true;
            _th.Profile.PdfSettings.CompressColorAndGray.Compression = CompressionColorAndGray.JpegMaximum;
            _th.Profile.PdfSettings.CompressColorAndGray.JpegCompressionFactor = 25;
            _th.Profile.PdfSettings.CompressColorAndGray.Resampling = false;
            _th.Profile.PdfSettings.CompressColorAndGray.Dpi = 8;

            _th.Profile.PdfSettings.CompressMonochrome.Enabled = false;
            _th.Profile.PdfSettings.CompressMonochrome.Compression = CompressionMonochrome.CcittFaxEncoding;
            _th.Profile.PdfSettings.CompressMonochrome.Resampling = true;
            _th.Profile.PdfSettings.CompressMonochrome.Dpi = 8;

            _th.GenerateGsJob(PSfiles.PDFCreatorTestpage, OutputFormat.Pdf);

            _th.RunGsJob();

            var pdf = new PdfReader(_th.Job.OutputFiles[0]);

            PdfDictionary pg = pdf.GetPageN(1);
            var res = (PdfDictionary) PdfReader.GetPdfObject(pg.Get(PdfName.RESOURCES));
            var xobj = (PdfDictionary) PdfReader.GetPdfObject(res.Get(PdfName.XOBJECT));

            var testResultsFilter = new String[4];
            var testResultsWidth = new String[4];
            var testResultsSize = new String[4];

            int i = 0;
            foreach (PdfName name in xobj.Keys)
            {
                PdfObject obj = xobj.Get(name);
                if (obj.IsIndirect())
                {
                    var tg = (PdfDictionary)PdfReader.GetPdfObject(obj);

                    if (tg.Get(PdfName.FILTER) != null)
                        testResultsFilter[i] = tg.Get(PdfName.FILTER).ToString();

                    testResultsWidth[i] = tg.Get(PdfName.WIDTH).ToString();

                    string height = tg.Get(PdfName.HEIGHT).ToString();
                    ImageRenderInfo imgRI =
                        ImageRenderInfo.CreateForXObject(
                            new Matrix(float.Parse(testResultsWidth[i]), float.Parse(height)), (PRIndirectReference)obj, tg);
                    testResultsSize[i] = imgRI.GetImage()
                        .GetImageAsBytes()
                        .Length.ToString(CultureInfo.InvariantCulture);
                    i++;
                }
            }

            Assert.AreEqual(null, testResultsFilter[0]);
            Assert.AreEqual("/DCTDecode", testResultsFilter[1]);
            Assert.AreEqual("/DCTDecode", testResultsFilter[2]);
            Assert.AreEqual("/DCTDecode", testResultsFilter[3]);

            Assert.AreEqual("200", testResultsWidth[0]);
            Assert.AreEqual("200", testResultsWidth[1]);
            Assert.AreEqual("200", testResultsWidth[2]);
            Assert.AreEqual("475", testResultsWidth[3]);

            Assert.AreEqual("1995", testResultsSize[0]);
            Assert.AreEqual("4202", testResultsSize[1]);
            Assert.AreEqual("6179", testResultsSize[2]);
            Assert.AreEqual("4197", testResultsSize[3]);
        }

        [Test]
        public void CompressionFactorJpegMinimum_NoResample()
        {
            _th.Profile.PdfSettings.CompressColorAndGray.Enabled = true;
            _th.Profile.PdfSettings.CompressColorAndGray.Compression = CompressionColorAndGray.JpegMinimum;
            _th.Profile.PdfSettings.CompressColorAndGray.JpegCompressionFactor = 25;
            _th.Profile.PdfSettings.CompressColorAndGray.Resampling = false;
            _th.Profile.PdfSettings.CompressColorAndGray.Dpi = 8;

            _th.Profile.PdfSettings.CompressMonochrome.Enabled = false;
            _th.Profile.PdfSettings.CompressMonochrome.Compression = CompressionMonochrome.CcittFaxEncoding;
            _th.Profile.PdfSettings.CompressMonochrome.Resampling = true;
            _th.Profile.PdfSettings.CompressMonochrome.Dpi = 8;

            _th.GenerateGsJob(PSfiles.PDFCreatorTestpage, OutputFormat.Pdf);

            _th.RunGsJob();

            var pdf = new PdfReader(_th.Job.OutputFiles[0]);

            PdfDictionary pg = pdf.GetPageN(1);
            var res = (PdfDictionary)PdfReader.GetPdfObject(pg.Get(PdfName.RESOURCES));
            var xobj = (PdfDictionary)PdfReader.GetPdfObject(res.Get(PdfName.XOBJECT));

            var testResultsFilter = new String[4];
            var testResultsWidth = new String[4];
            var testResultsSize = new String[4];

            int i = 0;
            foreach (PdfName name in xobj.Keys)
            {
                PdfObject obj = xobj.Get(name);
                if (obj.IsIndirect())
                {
                    var tg = (PdfDictionary)PdfReader.GetPdfObject(obj);

                    if (tg.Get(PdfName.FILTER) != null)
                        testResultsFilter[i] = tg.Get(PdfName.FILTER).ToString();

                    testResultsWidth[i] = tg.Get(PdfName.WIDTH).ToString();

                    string height = tg.Get(PdfName.HEIGHT).ToString();
                    ImageRenderInfo imgRI =
                        ImageRenderInfo.CreateForXObject(
                            new Matrix(float.Parse(testResultsWidth[i]), float.Parse(height)), (PRIndirectReference)obj, tg);
                    testResultsSize[i] = imgRI.GetImage()
                        .GetImageAsBytes()
                        .Length.ToString(CultureInfo.InvariantCulture);
                    i++;
                }
            }

            Assert.AreEqual(null, testResultsFilter[0]);
            Assert.AreEqual("/DCTDecode", testResultsFilter[1]);
            Assert.AreEqual("/DCTDecode", testResultsFilter[2]);
            Assert.AreEqual("/DCTDecode", testResultsFilter[3]);

            Assert.AreEqual("200", testResultsWidth[0]);
            Assert.AreEqual("200", testResultsWidth[1]);
            Assert.AreEqual("200", testResultsWidth[2]);
            Assert.AreEqual("475", testResultsWidth[3]);

            Assert.AreEqual("1995", testResultsSize[0]);
            Assert.AreEqual("17960", testResultsSize[1]);
            Assert.AreEqual("31460", testResultsSize[2]);
            Assert.AreEqual("16899", testResultsSize[3]);


            
        }
    }
}
