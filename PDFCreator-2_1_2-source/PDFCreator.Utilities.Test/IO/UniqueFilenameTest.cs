using System;
using SystemInterface.IO;
using NUnit.Framework;
using pdfforge.PDFCreator.Utilities.IO;
using Rhino.Mocks;

namespace pdfforge.PDFCreator.Utilities.Test.IO
{
    [TestFixture]
    public class UniqueFilenameTest
    {
        private IFile MakeFileWrap()
        {
            return MockRepository.GenerateStub<IFile>();
        }

        [Test]
        public void UniqueFile_GivenNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new UniqueFilename(null));
        }

        [Test]
        public void UniqueFile_GivenEmptyString_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new UniqueFilename("").MakeUniqueFilename());
        }

        [Test]
        public void UniqueFile_GivenNonexistingFile_ReturnsSameFile()
        {
            const string filename = @"C:\test.txt";

            var fileWrap = MakeFileWrap();
            fileWrap.Stub(x => x.Exists(filename)).Return(false);

            var uniqueFilename = new UniqueFilename(filename, fileWrap);

            Assert.AreEqual(filename, uniqueFilename.MakeUniqueFilename());
        }

        [Test]
        public void UniqueFile_GivenExistingFile_ReturnsUniquifiedFile()
        {
            const string filename = @"C:\test.txt";
            const string expectedFilename = @"C:\test_2.txt";

            var fileWrap = MakeFileWrap();
            fileWrap.Stub(x => x.Exists(filename)).Return(true);
            fileWrap.Stub(x => x.Exists(expectedFilename)).Return(false);

            var uniqueFilename = new UniqueFilename(filename, fileWrap);

            Assert.AreEqual(expectedFilename, uniqueFilename.MakeUniqueFilename());
        }

        [Test]
        public void UniqueFile_GivenExistingFileWithContinuance_ReturnsUniquifiedFile()
        {
            const string filename = @"C:\test_5.txt";
            const string expectedFilename = @"C:\test_6.txt";

            var fileWrap = MakeFileWrap();
            fileWrap.Stub(x => x.Exists(filename)).Return(true);
            fileWrap.Stub(x => x.Exists(expectedFilename)).Return(false);

            var uniqueFilename = new UniqueFilename(filename, fileWrap);

            Assert.AreEqual(expectedFilename, uniqueFilename.MakeUniqueFilename(true));
        }

        [Test]
        public void UniqueFile_GivenExistingFileWithContinuanceAndWithoutIndex_ReturnsUniquifiedFileWithIndex2()
        {
            const string filename = @"C:\test_x.txt";
            const string expectedFilename = @"C:\test_x_2.txt";

            var fileWrap = MakeFileWrap();
            fileWrap.Stub(x => x.Exists(filename)).Return(true);
            fileWrap.Stub(x => x.Exists(expectedFilename)).Return(false);

            var uniqueFilename = new UniqueFilename(filename, fileWrap);

            Assert.AreEqual(expectedFilename, uniqueFilename.MakeUniqueFilename(true));
        }

        [Test]
        public void UniqueFile_GivenExistingFileWithoutExtension_ReturnsUniquifiedFile()
        {
            const string filename = @"C:\test";
            const string expectedFilename = @"C:\test_2";

            var fileWrap = MakeFileWrap();
            fileWrap.Stub(x => x.Exists(filename)).Return(true);
            fileWrap.Stub(x => x.Exists(expectedFilename)).Return(false);

            var uniqueFilename = new UniqueFilename(filename, fileWrap);

            Assert.AreEqual(expectedFilename, uniqueFilename.MakeUniqueFilename());
        }
    }
}
