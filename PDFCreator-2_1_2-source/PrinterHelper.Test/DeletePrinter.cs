using System.Collections.Generic;
using NUnit.Framework;
using pdfforge.PrinterHelper;
using Rhino.Mocks;

namespace PrinterHelper.Test
{
    [TestFixture]
    public class DeletePrinter
    {
        private const string CommandLineParameter = "/DeletePrinter";
        private const string DriverName = "Driver";
        private const string PrinterName1 = "Printer1";
        private const string PrinterName2 = "Printer2";
        private const string PrinterName3 = "Printer3";

        private readonly string _commandLineParameterKey = CommandLineParameter.Substring(1).ToLower();

        [Test]
        public void InstallPrinter_WithEmptyArgumentList_ThrowsKeyNotFoundException()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            string[] cmdStrings = { "" };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            // ReSharper disable once ConvertToLambdaExpression
            TestDelegate errorMethod = delegate
            {
                new Printer().DeletePrinter(p[_commandLineParameterKey], win32PrinterStub, DriverName, new Logging(LogLevel.NoLogging, ""));
            };

            Assert.Throws<KeyNotFoundException>(errorMethod);
        }

        [Test]
        public void DeletePrinter_WithoutPrinterList_ReturnsErrorObject()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            string[] cmdStrings = { CommandLineParameter };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            var result = new Printer().DeletePrinter(p[_commandLineParameterKey], win32PrinterStub, DriverName, new Logging(LogLevel.NoLogging, ""));

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_PARAMETER, result.Code);
        }

        [Test]
        public void DeletePrinter_WithEmptyPrinterList_ReturnsErrorObject()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            string[] cmdStrings = { CommandLineParameter, "" };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            var result = new Printer().DeletePrinter(p[_commandLineParameterKey], win32PrinterStub, DriverName, new Logging(LogLevel.NoLogging, ""));

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_PARAMETER, result.Code);
        }

        [Test]
        public void DeletePrinter_WithPrinterList_CallsArePrintersWithSpecificDriverDeletable()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            string[] cmdStrings = { CommandLineParameter, PrinterName1 };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            var notInstalledPrinters = new List<string> {PrinterName1};
            var notPrintersWithSpecificDriver = new List<string>();
            win32PrinterStub.Stub(
                f => f.ArePrintersWithSpecificDriverDeletable(p[_commandLineParameterKey], DriverName, PrinterEnumFlags.PRINTER_ENUM_LOCAL, out notInstalledPrinters, out notPrintersWithSpecificDriver))
                .Return(false).OutRef(notInstalledPrinters, notPrintersWithSpecificDriver);

            var result = new Printer().DeletePrinter(p[_commandLineParameterKey], win32PrinterStub, DriverName, new Logging(LogLevel.NoLogging, ""));

            win32PrinterStub.AssertWasCalled(x => x.ArePrintersWithSpecificDriverDeletable(p[_commandLineParameterKey], DriverName, PrinterEnumFlags.PRINTER_ENUM_LOCAL, out notInstalledPrinters, out notPrintersWithSpecificDriver));

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_PARAMETER, result.Code);
        }

        [Test]
        public void DeletePrinter_WithThreeGivenPrinterInPrinterList_CallsDeletePrinter()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            string[] cmdStrings = { CommandLineParameter, PrinterName1, PrinterName2, PrinterName3 };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            List<string> notInstalledPrinters;
            List<string> notPrintersWithSpecificDriver;

            win32PrinterStub.Stub(
                f => f.ArePrintersWithSpecificDriverDeletable(p[_commandLineParameterKey], DriverName, PrinterEnumFlags.PRINTER_ENUM_LOCAL, out notInstalledPrinters, out notPrintersWithSpecificDriver))
                .Return(true).OutRef(new List<string>(), new List<string>());

            new Printer().DeletePrinter(p[_commandLineParameterKey], win32PrinterStub, DriverName, new Logging(LogLevel.NoLogging, ""));

            for (int i = 0; i < p[_commandLineParameterKey].Count; i++)
                win32PrinterStub.AssertWasCalled(x => x.DeletePrinter(p[_commandLineParameterKey][i]), options => options.Repeat.Once());
        }
    }
}
