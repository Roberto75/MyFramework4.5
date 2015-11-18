using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using pdfforge.PrinterHelper;
using Rhino.Mocks;

namespace PrinterHelper.Test
{
    [TestFixture]
    public class PortApplication
    {
        private const string CommandLineParameter = "/PortApplication";
        private const string PortApplicationFileName1 = @"C:\Program files\PDFCreator\PDFCreator1.exe";
        private const string PortApplicationFileName2 = @"C:\Program files\PDFCreator\PDFCreator2.exe";

        private readonly string _commandLineParameterKey = CommandLineParameter.Substring(1).ToLower();

        [Test]
        public void PortApplication_WithEmptyArgumentList_ThrowsKeyNotFoundException()
        {
            string[] cmdStrings = { "" };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);
            string portApplication;

            // ReSharper disable once ConvertToLambdaExpression
            TestDelegate errorMethod = delegate
            {
                new Printer().CheckAndGetPortApplication(p[_commandLineParameterKey], out portApplication);
            };

            Assert.Throws<KeyNotFoundException>(errorMethod);
        }

        [Test]
        public void PortApplication_WithoutPortApplicationList_ReturnsErrorObject()
        {
            string[] cmdStrings = { CommandLineParameter };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            string portApplication;
            var result = new Printer().CheckAndGetPortApplication(p[_commandLineParameterKey], out portApplication);

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_PARAMETER, result.Code);
        }

        [Test]
        public void PortApplication_WithEmptyPortApplicationList_ReturnsErrorObject()
        {
            string[] cmdStrings = { CommandLineParameter, "" };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            string portApplication;
            var result = new Printer().CheckAndGetPortApplication(p[_commandLineParameterKey], out portApplication);

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_PARAMETER, result.Code);
        }

        [Test]
        public void PortApplication_WithTwoPortApplicationInList_ReturnsErrorObject()
        {
            string[] cmdStrings = { CommandLineParameter, PortApplicationFileName1, PortApplicationFileName2 };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            string portApplication;
            var result = new Printer().CheckAndGetPortApplication(p[_commandLineParameterKey], out portApplication);

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_PARAMETER, result.Code);
        }

        [Test]
        public void PortApplication_WithOnePortApplicationInListButNotWellFormed_ReturnsErrorObject()
        {
            string[] cmdStrings = { CommandLineParameter, "|a|\\:" };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            string portApplication;
            var result = new Printer().CheckAndGetPortApplication(p[_commandLineParameterKey], out portApplication);

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_PARAMETER, result.Code);
        }

        [Test]
        public void PortApplication_WithOnePortApplicationInListButNotARootedPath_ReturnsErrorObject()
        {
            string[] cmdStrings = { CommandLineParameter, "PDFCreator.exe" };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            string portApplication;
            var result = new Printer().CheckAndGetPortApplication(p[_commandLineParameterKey], out portApplication);

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_PARAMETER, result.Code);
        }

        [Test]
        public void PortApplication_WithOnePortApplicationInList_ResultOk()
        {
            string[] cmdStrings = { CommandLineParameter, PortApplicationFileName1 };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            string portApplication;
            var result = new Printer().CheckAndGetPortApplication(p[_commandLineParameterKey], out portApplication);

            Assert.IsNull(result);
            Assert.AreEqual(PortApplicationFileName1, portApplication);
        }
    }
}
