using System;
using NUnit.Framework;

namespace pdfforge.PDFCreator.Utilities.Test
{
    /// <summary>
    /// IntegrationTest to test the file association verb functions
    /// </summary>
    [TestFixture]
    class FileAssocTest
    {
        [TestCase]
        public static void TestDefaultAssoc()
        {
            Assert.IsTrue(FileUtil.FileAssocHasPrintTo(".txt"), "PrintTo association for .txt files not detected!");
            Assert.IsTrue(FileUtil.FileAssocHasPrint(".txt"), "Print association for .txt files not detected!");
            Assert.IsTrue(FileUtil.FileAssocHasPrintTo("txt"), "PrintTo association for .txt files not detected!");
            Assert.IsFalse(FileUtil.FileAssocHasPrintTo(".invalidfileextension"), "PrintTo association for .invalidfileextension files detected, but should not exist!");
            Assert.IsFalse(FileUtil.FileAssocHasPrint(".invalidfileextension"), "Print association for .invalidfileextension files detected, but should not exist!");
        }

        [TestCase]
        public static void TestExceptions()
        {
            Assert.Throws<ArgumentException>(IllegalDotAssocCall);
            Assert.Throws<ArgumentException>(IllegalShortAssocCall);
            Assert.Throws<ArgumentNullException>(NullAssocCall);
            Assert.Throws<ArgumentNullException>(EmptyAssocCall);
        }

        public static void IllegalDotAssocCall()
        {
            FileUtil.FileAssocHasPrint(".illegal.fileextension");
        }

        private static void IllegalShortAssocCall()
        {
            FileUtil.FileAssocHasPrint(".");
        }

        private static void NullAssocCall()
        {
            FileUtil.FileAssocHasPrint(null);
        }

        private static void EmptyAssocCall()
        {
            FileUtil.FileAssocHasPrint("");
        }
    }
}
