using System;
using SystemInterface.IO;
using NUnit.Framework;
using pdfforge.PDFCreator.Utilities.IO;
using Rhino.Mocks;

namespace pdfforge.PDFCreator.Utilities.Test.IO
{
    [TestFixture]
    public class UniqueDirectoryTest
    {
        private IDirectory MakeDirectoryWrap()
        {
            return MockRepository.GenerateStub<IDirectory>();
        }
        
        [Test]
        public void EnsureUniqueDirectory_GivenNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new UniqueDirectory(null));
        }

        [Test]
        public void EnsureUniqueDirectory_GivenEmptyPath_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new UniqueDirectory("").MakeUniqueDirectory());
        }

        [Test]
        public void EnsureUniqueDirectory_GivenNonExistingDirectory_ReturnsSameDirectory()
        {
            const string path = @"C:\TestFolder\MySubFolder";

            var directoryWrap = MakeDirectoryWrap();
            directoryWrap.Stub(x => x.Exists(path)).Return(false);

            var uniqueDirectory = new UniqueDirectory(path, directoryWrap);

            Assert.AreEqual(path, uniqueDirectory.MakeUniqueDirectory());
        }

        [Test]
        public void EnsureUniqueDirectory_GivenExistingDirectory_ReturnsUniquifiedDirectory()
        {
            const string path = @"C:\TestFolder\MySubFolder";
            const string expectedPath = @"C:\TestFolder\MySubFolder_2";

            var directoryWrap = MakeDirectoryWrap();
            directoryWrap.Stub(x => x.Exists(path)).Return(true);

            var uniqueDirectory = new UniqueDirectory(path, directoryWrap);

            Assert.AreEqual(expectedPath, uniqueDirectory.MakeUniqueDirectory());
        }
    }
}
