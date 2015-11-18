using System.Collections.Generic;
using NUnit.Framework;
using pdfforge.PrinterHelper;
using Rhino.Mocks;

namespace PrinterHelper.Test
{
    [TestFixture]
    public class AddPrinter
    {
        private const string CommandLineParameter = "/AddPrinter";
        private const string PortName = "Port1";
        private const string DriverName = "Driver";
        private const string PrinterName1 = "Printer1";
        private const string PrinterName2 = "Printer2";
        private const string PrinterName3 = "Printer3";

        private readonly string _commandLineParameterKey = CommandLineParameter.Substring(1).ToLower();

        [Test]
        public void AddPrinter_WithEmptyArgumentList_ThrowsKeyNotFoundException()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            string[] cmdStrings = {""};
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            // ReSharper disable once ConvertToLambdaExpression
            TestDelegate errorMethod = delegate
            {
                new Printer().AddPrinter(PrinterEnvironment.WindowsX64, p[_commandLineParameterKey], win32PrinterStub, "", "", new Logging(LogLevel.NoLogging, ""));
            };

            Assert.Throws<KeyNotFoundException>(errorMethod);
        }

        [Test]
        public void AddPrinter_WithoutPrinterList_ReturnsErrorObject()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            string[] cmdStrings = { CommandLineParameter };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            var result = new Printer().AddPrinter(PrinterEnvironment.WindowsX64, p[_commandLineParameterKey], win32PrinterStub, "", "", new Logging(LogLevel.NoLogging, ""));

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_PARAMETER, result.Code);
        }

        [Test]
        public void AddPrinter_WithEmptyPrinterList_ReturnsErrorObject()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            string[] cmdStrings = { CommandLineParameter, "" };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            var result = new Printer().AddPrinter(PrinterEnvironment.WindowsX64, p[_commandLineParameterKey], win32PrinterStub, "", "", new Logging(LogLevel.NoLogging, ""));

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_PARAMETER, result.Code);
        }

        [Test]
        public void AddPrinter_WithPrinterName_CallsGetInstalledPrintersFromList()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            string[] cmdStrings = { CommandLineParameter, PrinterName1 };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            win32PrinterStub.Stub(x => x.GetInstalledPrintersFromList(p[_commandLineParameterKey])).Return(new List<string>());

            new Printer().AddPrinter(PrinterEnvironment.WindowsX64, p[_commandLineParameterKey], win32PrinterStub, "", "", new Logging(LogLevel.NoLogging, ""));

            win32PrinterStub.AssertWasCalled(x => x.GetInstalledPrintersFromList(p[_commandLineParameterKey]), options => options.Repeat.Once());
        }

        [Test]
        public void AddPrinter_WithPrinterAlreadyInstalled_ReturnsErrorObject()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            string[] cmdStrings = { CommandLineParameter, PrinterName1 };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);
            
            var tmpList = new List<string>(new[]{PrinterName1});
            win32PrinterStub.Stub(x => x.GetInstalledPrintersFromList(p[_commandLineParameterKey])).Return(tmpList);

            var result = new Printer().AddPrinter(PrinterEnvironment.WindowsX64, p[_commandLineParameterKey], win32PrinterStub, "", "", new Logging(LogLevel.NoLogging, ""));
            
            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_PARAMETER, result.Code);
        }

        [Test]
        public void AddPrinter_WithEmptyPortName_ReturnsErrorObject()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            string[] cmdStrings = { CommandLineParameter, PrinterName1 };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            win32PrinterStub.Stub(x => x.GetInstalledPrintersFromList(p[_commandLineParameterKey])).Return(new List<string>());

            var result = new Printer().AddPrinter(PrinterEnvironment.WindowsX64, p[_commandLineParameterKey], win32PrinterStub, "", "", new Logging(LogLevel.NoLogging, ""));
            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_COMMAND, result.Code);
        }

        [Test]
        public void AddPrinter_WithPortName_CallsIsPortInstalled()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            string[] cmdStrings = { CommandLineParameter, PrinterName1 };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            win32PrinterStub.Stub(x => x.GetInstalledPrintersFromList(p[_commandLineParameterKey])).Return(new List<string>());

            new Printer().AddPrinter(PrinterEnvironment.WindowsX64, p[_commandLineParameterKey], win32PrinterStub, PortName, "", new Logging(LogLevel.NoLogging, ""));

            win32PrinterStub.AssertWasCalled(x => x.IsPortInstalled(PortName), options => options.Repeat.Once());
        }

        [Test]
        public void AddPrinter_WithPortAlreadyInstalled_ReturnsErrorObject()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            string[] cmdStrings = { CommandLineParameter, PrinterName1 };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            win32PrinterStub.Stub(x => x.GetInstalledPrintersFromList(p[_commandLineParameterKey])).Return(new List<string>());
            win32PrinterStub.Stub(x => x.IsPortInstalled(PortName)).Return(true);

            var result = new Printer().AddPrinter(PrinterEnvironment.WindowsX64, p[_commandLineParameterKey], win32PrinterStub, PortName, "", new Logging(LogLevel.NoLogging, ""));

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_COMMAND, result.Code);
        }

        [Test]
        public void AddPrinter_WithEmptyDriverName_ReturnsErrorObject()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            string[] cmdStrings = { CommandLineParameter, PrinterName1 };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            win32PrinterStub.Stub(x => x.GetInstalledPrintersFromList(p[_commandLineParameterKey])).Return(new List<string>());

            var result = new Printer().AddPrinter(PrinterEnvironment.WindowsX64, p[_commandLineParameterKey], win32PrinterStub, PortName, "", new Logging(LogLevel.NoLogging, ""));
            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_COMMAND, result.Code);
        }

        [Test]
        public void AddPrinter_WithDriverName_CallsIsDriverInstalled()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            string[] cmdStrings = { CommandLineParameter, PrinterName1 };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            win32PrinterStub.Stub(x => x.GetInstalledPrintersFromList(p[_commandLineParameterKey])).Return(new List<string>());
            win32PrinterStub.Stub(x => x.IsPortInstalled(PortName)).Return(true);

            new Printer().AddPrinter(PrinterEnvironment.WindowsX64, p[_commandLineParameterKey], win32PrinterStub, PortName, DriverName, new Logging(LogLevel.NoLogging, ""));

            win32PrinterStub.AssertWasCalled(x => x.IsDriverInstalled(PrinterEnvironment.WindowsX64, DriverName), options => options.Repeat.Once());
        }

        [Test]
        public void AddPrinter_WithDriverAlreadyInstalled_ReturnsErrorObject()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            string[] cmdStrings = { CommandLineParameter, PrinterName1 };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            win32PrinterStub.Stub(x => x.GetInstalledPrintersFromList(p[_commandLineParameterKey])).Return(new List<string>());
            win32PrinterStub.Stub(x => x.IsDriverInstalled(PrinterEnvironment.WindowsX64, DriverName)).Return(true);

            var result = new Printer().AddPrinter(PrinterEnvironment.WindowsX64, p[_commandLineParameterKey], win32PrinterStub, PortName, DriverName, new Logging(LogLevel.NoLogging, ""));

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_COMMAND, result.Code);
        }
        
        [Test]
        public void AddPrinter_WithThreeGivenPrinterInPrinterList_CallsAddPrinter()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            string[] cmdStrings = { CommandLineParameter, PrinterName1, PrinterName2, PrinterName3 };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            win32PrinterStub.Stub(x => x.GetInstalledPrintersFromList(p[_commandLineParameterKey])).Return(new List<string>());
            win32PrinterStub.Stub(x => x.IsPortInstalled(PortName)).Return(true);
            win32PrinterStub.Stub(x => x.IsDriverInstalled(PrinterEnvironment.WindowsX64, DriverName)).Return(true);
            win32PrinterStub.Stub(x => x.AdaptFreememPrinterSetting(PrinterName1, 32767)).Return(null).Repeat.Once();
            win32PrinterStub.Stub(x => x.AdaptFreememPrinterSetting(PrinterName2, 32767)).Return(null).Repeat.Once();
            win32PrinterStub.Stub(x => x.AdaptFreememPrinterSetting(PrinterName3, 32767)).Return(null).Repeat.Once();

            new Printer().AddPrinter(PrinterEnvironment.WindowsX64, p[_commandLineParameterKey], win32PrinterStub, PortName, DriverName, new Logging(LogLevel.NoLogging, ""));

            for (var i = 0; i < p[_commandLineParameterKey].Count; i++)
                win32PrinterStub.AssertWasCalled(x => x.AddPrinter(p[_commandLineParameterKey][i], PortName, DriverName), options => options.Repeat.Once());
        }
    }
}
