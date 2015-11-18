using System.Collections.Generic;
using NUnit.Framework;
using pdfforge.PrinterHelper;
using Rhino.Mocks;

namespace PrinterHelper.Test
{
    [TestFixture]
    public class RenamePrinter
    {
        private const string CommandLineParameter = "/RenamePrinter";
        private const string PrinterName1 = "Printer1";
        private const string PrinterName2 = "Printer2";
        private const string PrinterName3 = "Printer3";

        private readonly string _commandLineParameterKey = CommandLineParameter.Substring(1).ToLower();

        [Test]
        public void RenamePrinter_WithEmptyArgumentList_ThrowsKeyNotFoundException()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            string[] cmdStrings = { "" };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            // ReSharper disable once ConvertToLambdaExpression
            TestDelegate errorMethod = delegate
            {
                new Printer().RenamePrinter(p[_commandLineParameterKey], win32PrinterStub, new Logging(LogLevel.NoLogging, ""));
            };

            Assert.Throws<KeyNotFoundException>(errorMethod);
        }

        [Test]
        public void RenamePrinter_WithoutPrinterList_ReturnsErrorObject()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            string[] cmdStrings = { CommandLineParameter };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            var result = new Printer().RenamePrinter(p[_commandLineParameterKey], win32PrinterStub, new Logging(LogLevel.NoLogging, ""));

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_PARAMETER, result.Code);
        }
        
        [Test]
        public void RenamePrinter_WithEmptyPrinterList_ReturnsErrorObject()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            string[] cmdStrings = { CommandLineParameter, PrinterName1 };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            var result = new Printer().RenamePrinter(p[_commandLineParameterKey], win32PrinterStub, new Logging(LogLevel.NoLogging, ""));

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_PARAMETER, result.Code);
        }

        [Test]
        public void RenamePrinter_WithOnePrinterInPrinterList_ReturnsErrorObject()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            string[] cmdStrings = { CommandLineParameter, PrinterName1 };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            var result = new Printer().RenamePrinter(p[_commandLineParameterKey], win32PrinterStub, new Logging(LogLevel.NoLogging, ""));

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_PARAMETER, result.Code);
        }

        [Test]
        public void RenamePrinter_WithThreePrinterInPrinterList_ReturnsErrorObject()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            string[] cmdStrings = { CommandLineParameter, PrinterName1, PrinterName2, PrinterName3 };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            var result = new Printer().RenamePrinter(p[_commandLineParameterKey], win32PrinterStub, new Logging(LogLevel.NoLogging, ""));

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_PARAMETER, result.Code);
        }

        [Test]
        public void RenamePrinter_CheckPrinterName1_CallsIsPrinterInstalled()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            string[] cmdStrings = { CommandLineParameter, PrinterName1, PrinterName2 };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            new Printer().RenamePrinter(p[_commandLineParameterKey], win32PrinterStub, new Logging(LogLevel.NoLogging, ""));

            win32PrinterStub.AssertWasCalled(x => x.IsPrinterInstalled(PrinterName1));
        }

        [Test]
        public void RenamePrinter_CheckPrinterName2_CallsIsPrinterInstalled()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            string[] cmdStrings = { CommandLineParameter, PrinterName1, PrinterName2 };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            win32PrinterStub.Stub(x => x.IsPrinterInstalled(p[_commandLineParameterKey][0])).Return(true);

            new Printer().RenamePrinter(p[_commandLineParameterKey], win32PrinterStub, new Logging(LogLevel.NoLogging, ""));

            win32PrinterStub.AssertWasCalled(x => x.IsPrinterInstalled(PrinterName2));
        }

        [Test]
        public void RenamePrinter_WithCorrectPrinterNames_CallsRenamePrinter()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            string[] cmdStrings = { CommandLineParameter, PrinterName1, PrinterName2 };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            win32PrinterStub.Stub(x => x.IsPrinterInstalled(p[_commandLineParameterKey][0])).Return(true);
            win32PrinterStub.Stub(x => x.IsPrinterInstalled(p[_commandLineParameterKey][1])).Return(false);

            new Printer().RenamePrinter(p[_commandLineParameterKey], win32PrinterStub, new Logging(LogLevel.NoLogging, ""));

            win32PrinterStub.AssertWasCalled(x => x.RenamePrinter(p[_commandLineParameterKey][0], p[_commandLineParameterKey][1]));
        }
    }
}
