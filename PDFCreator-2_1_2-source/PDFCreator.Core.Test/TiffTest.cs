using System;
using NUnit.Framework;
using System.IO;
using BitMiracle.LibTiff.Classic;
using PDFCreator.TestUtilities;
using pdfforge.PDFCreator.Core.Settings.Enums;


namespace PDFCreator.Core.Test
{
    [TestFixture]
    [Category("LongRunning")]
    class TiffTest
    {
        private TestHelper _th;

        [SetUp]
        public void SetUp()
        {
            _th = new TestHelper("TiffTest");
        }

        [TearDown]
        public void CleanUp()
        {
            _th.CleanUp();
        }

        [Test]
        public void TestGrayscale()
        {
            _th.Profile.TiffSettings.Color = TiffColor.Gray8Bit;
            _th.Profile.TiffSettings.Dpi = 300;
            MakeTiffTest();
        }

        [Test]
        public void Test12Bit()
        {
            _th.Profile.TiffSettings.Color = TiffColor.Color12Bit;
            _th.Profile.TiffSettings.Dpi = 601;
            MakeTiffTest();
        }

        [Test]
        public void Test24Bit()
        {
            _th.Profile.TiffSettings.Color = TiffColor.Color24Bit;
            _th.Profile.TiffSettings.Dpi = 257;
            MakeTiffTest();
        }

        [Test]
        public void Test1Bit()
        {
            _th.Profile.TiffSettings.Color = TiffColor.BlackWhite;
            _th.Profile.TiffSettings.Dpi = 303;
            MakeTiffTest();
        }

        private static byte[] GetBytesFromFile(string fullFilePath)
        {
            // this method is limited to 2^32 byte files (4.2 GB)
            FileStream fs = File.OpenRead(fullFilePath);
            try
            {
                var bytes = new byte[32];
                fs.Read(bytes, 0, Convert.ToInt32(32));
                return bytes;
            }
            finally
            {
                fs.Close();
            }
        }

        public void MakeTiffTest()
        {
            _th.GenerateGsJob(PSfiles.ThreePDFCreatorTestpages, OutputFormat.Tif);
            _th.RunGsJob();
            
            Assert.True(_th.Job.Completed, "The job has not completed yet!");
            Assert.True(_th.Job.Success, "The job did not convert successfully");

            Assert.AreEqual(1, _th.Job.OutputFiles.Count, "More than one Tiff-File");

            byte[] bytes = GetBytesFromFile(_th.Job.OutputFiles[0]);
                
            //check if tif
            if ((bytes[0] == 'I') && (bytes[1] == 'I')) //little-endian
            {
                Assert.AreEqual(42, bytes[2], "No Tiff-Form: 3rd byte is not 42 (little-endian)");
                Assert.AreEqual(0, bytes[3], "No Tiff-Form: 4th byte is not 0 (little-endian)");
            }
            else if ((bytes[0] == 'M') && (bytes[1] == 'M')) //big-endian
            {
                Assert.AreEqual(0, bytes[2], "No Tiff-Form: 3rd byte is not 0 (big-endian)");
                Assert.AreEqual(42, bytes[3], "No Tiff-Form: 4th byte is not 42 (big-endian)"); //42 is 4th Byte!
            }
            else
                Assert.IsTrue(false, "No Tiff-Form: 1st and 2nd Byte are not \"II\" or \"MM\"");

            using (Tiff image = Tiff.Open(_th.Job.OutputFiles[0], "r"))
            {
                int pages = image.NumberOfDirectories();

                Assert.AreEqual(3, pages);

                for (int i = 0; i < pages; ++i)
                {
                    image.SetDirectory((short)i);

                    Assert.AreEqual(image.GetField(TiffTag.XRESOLUTION)[0].ToInt(), image.GetField(TiffTag.YRESOLUTION)[0].ToInt(), "vertical resolution != horizontal resolution on page " + (i+1));
                    Assert.AreEqual(_th.Profile.TiffSettings.Dpi, image.GetField(TiffTag.XRESOLUTION)[0].ToInt(), 0.013, "vertical resolution != choosen dpi on page " + (i+1));

                    switch (_th.Profile.TiffSettings.Color)
                    {
                        case TiffColor.BlackWhite:
                            Assert.AreEqual(1, image.GetField(TiffTag.BITSPERSAMPLE)[0].ToInt(), "Colormode: 1 BitPerPixel required, but was " + image.GetField(TiffTag.BITSPERSAMPLE)[0] + " on page " + (i+1));
                            break;
                        case TiffColor.Gray8Bit:
                            Assert.AreEqual(8, image.GetField(TiffTag.BITSPERSAMPLE)[0].ToInt(), "Colormode: 8 BitPerPixel required, but was " + image.GetField(TiffTag.BITSPERSAMPLE)[0] + " on page " + (i + 1));
                            break;
                        case TiffColor.Color12Bit:
                            Assert.AreEqual(4, image.GetField(TiffTag.BITSPERSAMPLE)[0].ToInt(), "Colormode: 4 BitsPerPixel required, but was " + image.GetField(TiffTag.BITSPERSAMPLE)[0] + " on page " + (i + 1));
                            break;
                        case TiffColor.Color24Bit:
                            Assert.AreEqual(8, image.GetField(TiffTag.BITSPERSAMPLE)[0].ToInt(), "Colormode: 8 BitsPerPixel required, but was " + image.GetField(TiffTag.BITSPERSAMPLE)[0] + " on page " + (i + 1));
                            break;
                    }

                    /*
                    int width = image.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
                    int height = image.GetField(TiffTag.IMAGELENGTH)[0].ToInt();

                    int imageSize = height * width;
                    int[] raster = new int[imageSize*2];
                                                                                      //stop on error is true  
                    Assert.AreEqual(true, image.ReadRGBAImage(width, height, raster, true), "Image on Page " + (i+1) + " is corrupted");
                    */
                }
            }
        }
    }
}
