using System;
using System.Collections.Generic;
using NUnit.Framework;
using pdfforge.PDFCreator.Core.Jobs;
using pdfforge.PDFCreator.Core.Settings;

namespace PDFCreator.Core.Test.Jobs
{
    [TestFixture]
    class TitleReplacerTest
    {
        [Test]
        public void WithEmptyConstructor_DoesNotReplaceAnything()
        {
            var titleReplacer = new TitleReplacer();
            const string originalTitle = "My Sample Title - Microsoft Word";

            string title = titleReplacer.Replace(originalTitle);

            Assert.AreEqual(originalTitle, title);
        }

        [Test]
        public void WithAddedReplacements_WhenReplacing_ReplacesTitleParts()
        {
            var titleReplacer = new TitleReplacer();
            titleReplacer.AddReplacement(" - Microsoft Word", "");
            const string originalTitle = "My Sample Title - Microsoft Word";

            string title = titleReplacer.Replace(originalTitle);

            Assert.AreEqual("My Sample Title", title);
        }

        [Test]
        public void WithAddedReplacementAsTitleReplacementObject_WhenReplacing_ReplacesTitleParts()
        {
            var titleReplacer = new TitleReplacer();
            titleReplacer.AddReplacement(new TitleReplacement(" - Microsoft Word", ""));
            const string originalTitle = "My Sample Title - Microsoft Word";

            string title = titleReplacer.Replace(originalTitle);

            Assert.AreEqual("My Sample Title", title);
        }

        [Test]
        public void AfterAddingReplacementCollection_WhenReplacing_ReplacesTitleParts()
        {
            var replacements = new List<TitleReplacement>();
            replacements.Add(new TitleReplacement("One", "Two"));
            replacements.Add(new TitleReplacement("Alpha", "Beta"));
            var titleReplacer = new TitleReplacer();
            titleReplacer.AddReplacements(replacements);
            const string originalTitle = "Alpha - One";

            string title = titleReplacer.Replace(originalTitle);

            Assert.AreEqual("Beta - Two", title);
        }

        [Test]
        public void WithMultipleReplacement_ReplacedInCorrectOrder()
        {
            var titleReplacer = new TitleReplacer();
            titleReplacer.AddReplacement(" - Microsoft Word", "Two");
            titleReplacer.AddReplacement("TitleTwo", "File");
            const string originalTitle = "My Sample Title - Microsoft Word";

            string title = titleReplacer.Replace(originalTitle);

            Assert.AreEqual("My Sample File", title);
        }

        [Test]
        public void Replace_WithNullTitle_ThrowsArgumentException()
        {
            var titleReplacer = new TitleReplacer();

            Assert.Throws<ArgumentException>(() => titleReplacer.Replace(null));
        }
    }
}
