using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using pdfforge.PrinterHelper;
using Rhino.Mocks;

namespace PrinterHelper.Test
{
    [TestFixture]
    public class UnInstallPrinter
    {
        private const string CommandLineParameter = "/UnInstallPrinter";
        private const string MonitorName = "Monitor";
        private const string DriverName = "Driver";
        private const string PrinterName1 = "Printer1";
        private const string PrinterName2 = "Printer2";
        private const string PrinterName3 = "Printer3";

        private readonly string _commandLineParameterKey = CommandLineParameter.Substring(1).ToLower();

        [Test]
        public void UnInstallPrinter_WithPrinterDriver_CallsGetInstalledPrintersFromList()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();

            win32PrinterStub.Stub(x => x.GetLocalPrinters(DriverName)).Return(new List<PRINTER_INFO_2>());

            new Printer().UnInstallPrinter(win32PrinterStub, DriverName, MonitorName, new Logging(LogLevel.NoLogging, ""));

            win32PrinterStub.AssertWasCalled(x => x.GetLocalPrinters(DriverName));
        }

        [Test]
        public void UnInstallPrinter_WithPrinterDriver_CallsDeletePrinter()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();

            var printer1 = new PRINTER_INFO_2 {pPrinterName = PrinterName1};
            var printer2 = new PRINTER_INFO_2 { pPrinterName = PrinterName2 };
            var printer3 = new PRINTER_INFO_2 { pPrinterName = PrinterName3 };
            var printers = new List<PRINTER_INFO_2> {printer1, printer2, printer3};
            win32PrinterStub.Stub(x => x.GetLocalPrinters(DriverName)).Return(printers);
            List<string> notInstalledPrinters;
            List<string> notPrintersWithSpecificDriver;
            var printersList = printers.Select(x => x.pPrinterName).ToList();
            win32PrinterStub.Stub(x => x.ArePrintersWithSpecificDriverDeletable(printersList, DriverName, PrinterEnumFlags.PRINTER_ENUM_LOCAL, out notInstalledPrinters, out notPrintersWithSpecificDriver)).Return(true);

            new Printer().UnInstallPrinter(win32PrinterStub, DriverName, MonitorName, new Logging(LogLevel.NoLogging, ""));

            win32PrinterStub.AssertWasCalled(x => x.DeletePrinter(PrinterName1), options => options.Repeat.Once());
            win32PrinterStub.AssertWasCalled(x => x.DeletePrinter(PrinterName2), options => options.Repeat.Once());
            win32PrinterStub.AssertWasCalled(x => x.DeletePrinter(PrinterName3), options => options.Repeat.Once());
        }

        [Test]
        public void UnInstallPrinter_WithPrinterDriver_CallsDeletePrinterDriver()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();

            var printer1 = new PRINTER_INFO_2 { pPrinterName = PrinterName1 };
            var printers = new List<PRINTER_INFO_2> { printer1 };

            win32PrinterStub.Stub(x => x.GetLocalPrinters(DriverName)).Return(new List<PRINTER_INFO_2> { printer1 });
            win32PrinterStub.Stub(x => x.GetLocalPrinters(DriverName)).Return(printers);
            List<string> notInstalledPrinters;
            List<string> notPrintersWithSpecificDriver;
            var printersList = printers.Select(x => x.pPrinterName).ToList();
            win32PrinterStub.Stub(x => x.ArePrintersWithSpecificDriverDeletable(printersList, DriverName, PrinterEnumFlags.PRINTER_ENUM_LOCAL, out notInstalledPrinters, out notPrintersWithSpecificDriver)).Return(true);
            win32PrinterStub.Stub(x => x.DeletePrinter(PrinterName1)).Return(true);
            win32PrinterStub.Stub(x => x.IsDriverInstalled(PrinterEnvironment.WindowsNtX86, DriverName)).Return(true);
            win32PrinterStub.Stub(x => x.IsDriverInstalled(PrinterEnvironment.WindowsX64, DriverName)).Return(true);

            new Printer().UnInstallPrinter(win32PrinterStub, DriverName, MonitorName, new Logging(LogLevel.NoLogging, ""));

            win32PrinterStub.AssertWasCalled(x => x.DeletePrinterDriver(PrinterEnvironment.WindowsNtX86, DriverName), options => options.Repeat.Once());
            win32PrinterStub.AssertWasCalled(x => x.DeletePrinterDriver(PrinterEnvironment.WindowsX64, DriverName), options => options.Repeat.Once());
        }

        [Test]
        public void UnInstallPrinter_WithPrinterDriver_CallsDeleteMonitor()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();

            var printer1 = new PRINTER_INFO_2 { pPrinterName = PrinterName1 };
            var printers = new List<PRINTER_INFO_2> { printer1 };

            var printerEnvironment = Printer.CurrentPrinterEnvironment;
            
            List<string> notInstalledPrinters;
            List<string> notPrintersWithSpecificDriver;
            var printersList = printers.Select(x => x.pPrinterName).ToList();
            win32PrinterStub.Stub(x => x.ArePrintersWithSpecificDriverDeletable(printersList, DriverName, PrinterEnumFlags.PRINTER_ENUM_LOCAL, out notInstalledPrinters, out notPrintersWithSpecificDriver)).Return(true);
            win32PrinterStub.Stub(x => x.GetLocalPrinters(DriverName)).Return(new List<PRINTER_INFO_2> { printer1 });
            win32PrinterStub.Stub(x => x.DeletePrinter(PrinterName1)).Return(true);
            win32PrinterStub.Stub(x => x.DeletePrinterDriver(printerEnvironment, DriverName)).Return(true);
            win32PrinterStub.Stub(x => x.IsMonitorInstalled(MonitorName)).Return(true);
            win32PrinterStub.Stub(x => x.GetPrinters(PrinterEnumFlags.PRINTER_ENUM_ALL, MonitorName))
                .Return(new List<PRINTER_INFO_2>());
            new Printer().UnInstallPrinter(win32PrinterStub, DriverName, MonitorName, new Logging(LogLevel.NoLogging, ""));

            win32PrinterStub.AssertWasCalled(x => x.DeleteMonitor(MonitorName, true), options => options.Repeat.Once());
        }

        [Test]
        public void UnInstallPrinter_UndeletablePdfcreatorPrinters_ReturnsErrorObject()
        {
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();

            var printer1 = new PRINTER_INFO_2 { pPrinterName = PrinterName1 };
            var printers = new List<PRINTER_INFO_2> { printer1 };
            var printersList = printers.Select(x => x.pPrinterName).ToList();

            win32PrinterStub.Stub(x => x.GetLocalPrinters(DriverName)).Return(new List<PRINTER_INFO_2> { printer1 });
            win32PrinterStub.Stub(x => x.DeletePrinter(PrinterName1)).Return(false);
            List<string> notInstalledPrinters;
            List<string> notPrintersWithSpecificDriver;
            win32PrinterStub.Stub(x => x.ArePrintersWithSpecificDriverDeletable(printersList, DriverName, PrinterEnumFlags.PRINTER_ENUM_LOCAL, out notInstalledPrinters, out notPrintersWithSpecificDriver)).Return(false).OutRef(new List<string>(), printersList);

            var result = new Printer().UnInstallPrinter(win32PrinterStub, DriverName, MonitorName, new Logging(LogLevel.NoLogging, ""));

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_PARAMETER, result.Code);
        }
    }
}
