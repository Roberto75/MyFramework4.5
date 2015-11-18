using System.Collections.Generic;
using NUnit.Framework;
using pdfforge.PrinterHelper;
using Rhino.Mocks;

namespace PrinterHelper.Test
{
    [TestFixture]
    public class InstallPrinter
    {
        private const string CommandLineParameter = "/InstallPrinter";
        private const string PortName = "Port";
        private const string MonitorName = "Monitor";
        private const string MonitorDriverFileName = "pdfcmon.dll";
        private const string PortApplication = "";
        private const string DriverName = "Driver";
        private const string PrinterName1 = "Printer1";
        private const string PrinterName2 = "Printer2";
        private const string PrinterName3 = "Printer3";
        private const string PpdFile = "ppdfile.ppd";

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
                new Printer().InstallPrinter(p[_commandLineParameterKey], win32PrinterStub, MonitorName, PortName, PortApplication, DriverName, PpdFile, new Logging(LogLevel.NoLogging, ""));
            };

            Assert.Throws<KeyNotFoundException>(errorMethod);
        }

        [Test]
        public void InstallPrinter_WithoutPrinterList_ReturnsErrorObject()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            string[] cmdStrings = { CommandLineParameter };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            var result = new Printer().InstallPrinter(p[_commandLineParameterKey], win32PrinterStub, MonitorName, PortName, PortApplication, DriverName, PpdFile, new Logging(LogLevel.NoLogging, ""));

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_PARAMETER, result.Code);
        }

        [Test]
        public void InstallPrinter_WithEmptyPrinterList_ReturnsErrorObject()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            string[] cmdStrings = { CommandLineParameter, "" };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            var result = new Printer().InstallPrinter(p[_commandLineParameterKey], win32PrinterStub, MonitorName, PortName, PortApplication, DriverName, PpdFile, new Logging(LogLevel.NoLogging, ""));

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_PARAMETER, result.Code);
        }

        [Test]
        public void InstallPrinter_WithOneGivenPrinterInPrinterList_CallsGetInstalledPrintersFromList()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            string[] cmdStrings = { CommandLineParameter, PrinterName1 };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            win32PrinterStub.Stub(x => x.GetInstalledPrintersFromList(p[_commandLineParameterKey])).Return(new List<string> { PrinterName1 });
            new Printer().InstallPrinter(p[_commandLineParameterKey], win32PrinterStub, MonitorName, PortName, PortApplication, DriverName, PpdFile, new Logging(LogLevel.NoLogging, ""));
            win32PrinterStub.AssertWasCalled(x => x.GetInstalledPrintersFromList(p[_commandLineParameterKey]), options => options.Repeat.Once());
        }

        [Test]
        public void InstallPrinter_WithOneGivenPrinterInPrinterList_CallsIsMonitorInstalled()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            string[] cmdStrings = { CommandLineParameter, PrinterName1 };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            win32PrinterStub.Stub(x => x.GetInstalledPrintersFromList(p[_commandLineParameterKey])).Return(new List<string>());
            win32PrinterStub.Stub(x => x.IsMonitorInstalled(MonitorName)).Return(true);

            new Printer().InstallPrinter(p[_commandLineParameterKey], win32PrinterStub, MonitorName, PortName, PortApplication, DriverName, PpdFile, new Logging(LogLevel.NoLogging, ""));
            win32PrinterStub.AssertWasCalled(x => x.IsMonitorInstalled(MonitorName), options => options.Repeat.Once());
        }

        [Test]
        public void InstallPrinter_WithOneGivenPrinterInPrinterList_CallsAddPdfcreatorPrinterMonitor()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            string[] cmdStrings = { CommandLineParameter, PrinterName1 };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            win32PrinterStub.Stub(x => x.GetInstalledPrintersFromList(p[_commandLineParameterKey])).Return(new List<string>());

            var printerEnvironment = Printer.CurrentPrinterEnvironment;
            
            new Printer().InstallPrinter(p[_commandLineParameterKey], win32PrinterStub, MonitorName, PortName, PortApplication, DriverName, PpdFile, new Logging(LogLevel.NoLogging, ""));
            win32PrinterStub.AssertWasCalled(x => x.AddPdfcreatorPrinterMonitor(printerEnvironment, MonitorName, PortName, MonitorDriverFileName, PortApplication), options => options.Repeat.Once());
        }

        [Test]
        public void InstallPrinter_WithOneGivenPrinterInPrinterList_CallsIsDriverInstalled()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            string[] cmdStrings = { CommandLineParameter, PrinterName1 };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            var printerEnvironment = Printer.CurrentPrinterEnvironment;
            
            win32PrinterStub.Stub(x => x.GetInstalledPrintersFromList(p[_commandLineParameterKey])).Return(new List<string>());
            win32PrinterStub.Stub(x => x.IsDriverInstalled(printerEnvironment, DriverName)).Return(true);

            new Printer().InstallPrinter(p[_commandLineParameterKey], win32PrinterStub, MonitorName, PortName, PortApplication, DriverName, PpdFile, new Logging(LogLevel.NoLogging, ""));

            win32PrinterStub.AssertWasCalled(x => x.IsDriverInstalled(printerEnvironment, DriverName), options => options.Repeat.Once());
        }

        [Test]
        public void InstallPrinter_WithOneGivenPrinterInPrinterList_CallsAddPdfcreatorPrinterDriverGdi()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            string[] cmdStrings = { CommandLineParameter, PrinterName1 };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            win32PrinterStub.Stub(x => x.GetInstalledPrintersFromList(p[_commandLineParameterKey])).Return(new List<string>());

            var printerEnvironment = Printer.CurrentPrinterEnvironment;
            
            new Printer().InstallPrinter(p[_commandLineParameterKey], win32PrinterStub, MonitorName, PortName, PortApplication, DriverName, PpdFile, new Logging(LogLevel.NoLogging, ""));
            win32PrinterStub.AssertWasCalled(x => x.AddPdfcreatorPrinterDriverGdi(printerEnvironment, DriverName, PrinterDescriptionFileLanguage.CurrentCulture, PpdFile), options => options.Repeat.Once());
        }

        [Test]
        public void InstallPrinter_WithThreeGivenPrinterInPrinterList_CallsAddPrinter()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            string[] cmdStrings = { CommandLineParameter, PrinterName1, PrinterName2, PrinterName3 };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            var printerEnvironment = Printer.CurrentPrinterEnvironment;
            
            win32PrinterStub.Stub(x => x.GetInstalledPrintersFromList(p[_commandLineParameterKey])).Return(new List<string>());
            win32PrinterStub.Stub(x => x.IsPortInstalled(PortName)).Return(true);
            win32PrinterStub.Stub(x => x.IsDriverInstalled(printerEnvironment, DriverName)).Return(false).Repeat.Once();
            win32PrinterStub.Stub(x => x.IsDriverInstalled(printerEnvironment, DriverName)).Return(true).Repeat.Once();
            win32PrinterStub.Stub(x => x.AdaptFreememPrinterSetting(PrinterName1, 32767)).Return(null).Repeat.Once();
            win32PrinterStub.Stub(x => x.AdaptFreememPrinterSetting(PrinterName2, 32767)).Return(null).Repeat.Once();
            win32PrinterStub.Stub(x => x.AdaptFreememPrinterSetting(PrinterName3, 32767)).Return(null).Repeat.Once();

            new Printer().InstallPrinter(p[_commandLineParameterKey], win32PrinterStub, MonitorName, PortName, PortApplication, DriverName, PpdFile, new Logging(LogLevel.NoLogging, ""));

            // ReSharper disable once AccessToModifiedClosure
            for (var i = 0; i < p[_commandLineParameterKey].Count; i++)
                win32PrinterStub.AssertWasCalled(x => x.AddPrinter(p[_commandLineParameterKey][i], PortName, DriverName), options => options.Repeat.Once());
        }
    }
}
