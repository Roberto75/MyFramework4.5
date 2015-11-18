using System;
using System.IO;
using NUnit.Framework;
using PDFCreator.TestUtilities;

namespace pdfforge.PDFCreator.Utilities.Test
{
    [TestFixture]
    class FileUtilTest
    {
        [TearDown]
        public void CleanUp()
        {
            TempFileHelper.CleanUp();
        }

        [Test]
        public void TestValidRootedPath()
        {
            string[] validPaths = { @"C:\Test.txt", @"X:\Test\Folder\With\Many\Sub\Folders\test.txt", @"\\TestServer\SomeFolder\Test.txt" };
            string[] invalidPaths = { "text.txt", @":@!|", @"\Test\test.txt", @":\test.txt", @"_:\Test.txt", "", "a", "C:MyDir", @"C:\Mydir:" };

            foreach (var p in validPaths)
                Assert.IsTrue(FileUtil.IsValidRootedPath(p), "Expected '" + p + "' to be a valid path");

            foreach (var p in invalidPaths)
                Assert.IsFalse(FileUtil.IsValidRootedPath(p), "Expected '" + p + "' to be an invalid path");
        }

        [Test]
        public void MakeValidFileName_GivenValidFilename_ReturnsSameString()
        {
            const string filename = @"Test ! File.txt";
            Assert.AreEqual(filename, FileUtil.MakeValidFileName(filename));
        }

        [Test]
        public void MakeValidFileName_GivenInvalidFilename_ReturnsSanitizedString()
        {
            Assert.AreEqual(@"File_Name", FileUtil.MakeValidFileName(@"File:Name"));
        }

        [Test]
        public void MakeValidFolderName_GivenValidFolder_ReturnsSameString()
        {
            const string filename = @"C:\Some _ !Folder";
            Assert.AreEqual(filename, FileUtil.MakeValidFolderName(filename));
        }

        [Test]
        public void MakeValidFolderName_GivenInvalidFolder_ReturnsSanitizedString()
        {
            Assert.AreEqual(@"C:\Some _ Folder", FileUtil.MakeValidFolderName(@"C:\Some | Folder"));
        }

        [Test]
        public void MakeValidFolderName_GivenMisplacedColon_ReturnsSanitizedString()
        {
            Assert.AreEqual(@"C:\Some_Folder", FileUtil.MakeValidFolderName(@"C:\Some:Folder"));
        }

        [Test]
        public void TestIsValidFilename()
        {
            string[] validPaths = { @"C:\Test.txt", @"X:\Test\Folder\With\Many\Sub\Folders\test.txt" };
            string[] invalidPaths = { @"C:\Test<.txt", @"C:\Test>.txt", @"C:\Test?.txt", @"C:\Test*.txt", @"C:\Test|.txt", @"C:\Test"".txt" };

            foreach (var p in validPaths)
                Assert.IsTrue(FileUtil.IsValidFilename(p), "Expected '" + p + "' to be a valid path");

            foreach (var p in invalidPaths)
                Assert.IsFalse(FileUtil.IsValidFilename(p), "Expected '" + p + "' to be an invalid path");
        }

        [Test]
        public void FileAssocHasPrint_GivenTxt_HasPrintVerb()
        {
            Assert.IsTrue(FileUtil.FileAssocHasPrint("txt"));
        }

        [Test]
        public void FileAssocHasPrint_GivenTxt_HasPrintToVerb()
        {
            Assert.IsTrue(FileUtil.FileAssocHasPrintTo("txt"));
        }

        [Test]
        public void FileAssocHasPrint_GivenTxt_HasOpenVerb()
        {
            Assert.IsTrue(FileUtil.FileAssocHasOpen("txt"));
        }

        [Test]
        public void FileAssocHasPrint_GivenEmpty_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => FileUtil.FileAssocHasPrintTo(""));
        }

        [Test]
        public void FileAssocHasPrint_GivenDot_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => FileUtil.FileAssocHasPrintTo("."));
        }

        [Test]
        public void FileAssocHasPrint_GivenDoubleDot_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => FileUtil.FileAssocHasPrintTo(".."));
        }

        [Test]
        public void FileAssocHasPrint_GivenUnknownExtension_ReturnsFalse()
        {
            Assert.IsFalse(FileUtil.FileAssocHasPrintTo(".unkownFileExtension"));
        }

        [Test]
        public void CommandLineToArgs_GivenSimpleCommandLine_ReturnsGoodArray()
        {
            const string commandLine = "/Test /Quote=\"This is a Test\"";
            var expected = new[] { "/Test", "/Quote=This is a Test" };
            Assert.AreEqual(expected, FileUtil.CommandLineToArgs(commandLine));
        }

        [Test]
        public void GetLongDirectoryName_GivenNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => FileUtil.GetLongDirectoryName(null));
        }

        [Test]
        public void GetLongDirectoryName_GivenEmptyPath_ReturnsNull()
        {
            Assert.IsNull(FileUtil.GetLongDirectoryName(""));
        }

        [Test]
        public void GetLongDirectoryName_GivenShortPath_ReturnsSamePath()
        {
            string folder = @"C:\folder";
            string file = folder + "\\test.txt";

            Assert.AreEqual(folder, FileUtil.GetLongDirectoryName(file));
        }

        [Test]
        public void GetLongDirectoryName_GivenDriveRootWithFile_ReturnsDriveRoot()
        {
            string folder = @"C:\";
            string file = folder + "\\test.txt";

            Assert.AreEqual(folder, FileUtil.GetLongDirectoryName(file));
        }

        [Test]
        public void CalculateMd5_WithNullFile_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => FileUtil.CalculateMd5(null));
        }

        [Test]
        public void CalculateMd5_WithEmptyFile_CalculatesCorrectHash()
        {
            string tmpFile = TempFileHelper.CreateTempFile("MD5_Test", "file.txt");
            File.WriteAllText(tmpFile, "");

            Assert.AreEqual("d41d8cd98f00b204e9800998ecf8427e", FileUtil.CalculateMd5(tmpFile));
        }

        [Test]
        public void CalculateMd5_WithTestString_CalculatesCorrectHash()
        {
            string tmpFile = TempFileHelper.CreateTempFile("MD5_Test", "file.txt");
            File.WriteAllText(tmpFile, "test");

            Assert.AreEqual("098f6bcd4621d373cade4e832627b4f6", FileUtil.CalculateMd5(tmpFile));
        }

        [Test]
        public void VerifyMd5_WithTestStringAndCorrectMd5_ReturnsTrue()
        {
            string tmpFile = TempFileHelper.CreateTempFile("MD5_Test", "file.txt");
            File.WriteAllText(tmpFile, "test");

            Assert.IsTrue(FileUtil.VerifyMd5(tmpFile, "098f6bcd4621d373cade4e832627b4f6"));
        }

        [Test]
        public void VerifyMd5_WithTestStringAndIncorrectMd5_ReturnsFalse()
        {
            string tmpFile = TempFileHelper.CreateTempFile("MD5_Test", "file.txt");
            File.WriteAllText(tmpFile, "test");

            Assert.IsFalse(FileUtil.VerifyMd5(tmpFile, "d41d8cd98f00b204e9800998ecf8427e"));
        }

        [Test]
        public void CheckWritability_WithWritableFolder_ReturnsTrue()
        {
            string tmpFolder = TempFileHelper.CreateTempFolder("CheckWritability");
            var fileUtil = new FileUtil();
            Assert.IsTrue(fileUtil.CheckWritability(tmpFolder));
        }
    }
}
