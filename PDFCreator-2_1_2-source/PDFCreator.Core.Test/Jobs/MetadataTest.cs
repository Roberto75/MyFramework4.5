using NUnit.Framework;
using pdfforge.PDFCreator.Core.Jobs;

namespace PDFCreator.Core.Test.Jobs
{
    [TestFixture]
    public class MetadataTest
    {
        [Test]
        public void MetadataWithTitle_OnCopy_ContainsSameTitle()
        {
            var metadata = new Metadata {Title = "MyTitle"};

            var clone = metadata.Copy();

            Assert.AreEqual(metadata.Title, clone.Title);
        }

        [Test]
        public void MetadataWithTitle_OnCopyAndEdit_OriginalNotModified()
        {
            var metadata = new Metadata {Title = "MyTitle"};

            var clone = metadata.Copy();
            clone.Title = "Title2";

            Assert.AreNotEqual(metadata.Title, clone.Title);
        }

        [Test]
        public void MetadataWithAuthor_OnCopy_ContainsSameAuthor()
        {
            var metadata = new Metadata {Author = "MyAuthor"};

            var clone = metadata.Copy();

            Assert.AreEqual(metadata.Author, clone.Author);
        }

        [Test]
        public void MetadataWithAuthor_OnCopyAndEdit_OriginalNotModified()
        {
            var metadata = new Metadata {Author = "MyAuthor"};

            var clone = metadata.Copy();
            clone.Author = "Author2";

            Assert.AreNotEqual(metadata.Author, clone.Author);
        }

        [Test]
        public void MetadataWithSubject_OnCopy_ContainsSameSubject()
        {
            var metadata = new Metadata {Subject = "MySubject"};

            var clone = metadata.Copy();

            Assert.AreEqual(metadata.Subject, clone.Subject);
        }

        [Test]
        public void MetadataWithSubject_OnCopyAndEdit_OriginalNotModified()
        {
            var metadata = new Metadata {Subject = "MySubject"};

            var clone = metadata.Copy();
            clone.Subject = "Subject2";

            Assert.AreNotEqual(metadata.Subject, clone.Subject);
        }

        [Test]
        public void MetadataWithKeywords_OnCopy_ContainsSameKeywords()
        {
            var metadata = new Metadata {Keywords = "MyKeywords"};

            var clone = metadata.Copy();

            Assert.AreEqual(metadata.Keywords, clone.Keywords);
        }

        [Test]
        public void MetadataWithKeywords_OnCopyAndEdit_OriginalNotModified()
        {
            var metadata = new Metadata {Keywords = "MyKeywords"};

            var clone = metadata.Copy();
            clone.Keywords = "Keywords2";

            Assert.AreNotEqual(metadata.Keywords, clone.Keywords);
        }

        [Test]
        public void MetadataWithAllProperties_OnCopy_ContainsSameValues()
        {
            var metadata = new Metadata
            {
                Title = "MyTitle",
                Author = "MyAuthor",
                Subject = "MySubject",
                Keywords = "MyKeywords"
            };

            var clone = metadata.Copy();

            Assert.AreEqual(metadata.Keywords, clone.Keywords);
            Assert.AreEqual(metadata.Author, clone.Author);
            Assert.AreEqual(metadata.Subject, clone.Subject);
            Assert.AreEqual(metadata.Keywords, clone.Keywords);
        }

        [Test]
        public void MetadataWithAllProperties_OnCopyAndEdit_OriginalNotModified()
        {
            var metadata = new Metadata
            {
                Title = "MyTitle",
                Author = "MyAuthor",
                Subject = "MySubject",
                Keywords = "MyKeywords"
            };

            var clone = metadata.Copy();

            metadata.Title = "MyTitle2";
            metadata.Author = "MyAuthor2";
            metadata.Subject = "MySubject2";
            metadata.Keywords = "MyKeywords2";

            Assert.AreNotEqual(metadata.Keywords, clone.Keywords);
            Assert.AreNotEqual(metadata.Author, clone.Author);
            Assert.AreNotEqual(metadata.Subject, clone.Subject);
            Assert.AreNotEqual(metadata.Keywords, clone.Keywords);
        }
    }
}
