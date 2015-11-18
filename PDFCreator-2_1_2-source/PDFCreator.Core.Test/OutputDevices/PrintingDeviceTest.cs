using System;
using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;
using pdfforge.PDFCreator.Core.Ghostscript;
using pdfforge.PDFCreator.Core.Ghostscript.OutputDevices;
using pdfforge.PDFCreator.Core.Settings;
using pdfforge.PDFCreator.Core.Settings.Enums;
using pdfforge.PDFCreator.Utilities;
using PDFCreator.TestUtilities;
using Rhino.Mocks;

namespace PDFCreator.Core.Test.OutputDevices
{
    [TestFixture]
    class PrintingDeviceTest
    {
        private TestHelper _th;
        
        private PrintingDevice _printingDevice;
        private PrinterWrapper _printerStub ; 
        private Collection<string> _parameterStrings;
        private GhostscriptVersion _ghostscriptVersion;
        private const string TestFile = "testfile1.pdf";
        private const string PrinterName = "PrinterTestName";

        [SetUp]
        public void SetUp()
        {
            _th = new TestHelper("PrintingDeviceTest");
            _printerStub = MockRepository.GenerateStub<PrinterWrapper>();
            _printingDevice = new PrintingDevice(_printerStub);
            _th.GenerateGsJob(PSfiles.ThreePDFCreatorTestpages, OutputFormat.Pdf);
            _printingDevice.Job = _th.Job;
            _printingDevice.Job.OutputFiles.Add(TestFile);
            
            _printingDevice.Profile = new ConversionProfile();

            _ghostscriptVersion = new GhostscriptDiscovery().GetBestGhostscriptInstance(TestHelper.MinGhostscriptVersion);  
        }

        [TearDown]
        public void CleanUp()
        {
            _th.CleanUp();
        }

        public string GetMarkString(Collection<string> parameters)
        {
            var markString = parameters.First(x => x.StartsWith("mark "));
            Assert.IsNotNull(markString, "Missing mark parameter string.");
            return markString;
        }

        [Test]
        public void CheckDeviceIndependentDefaultParameters()
        {
            _parameterStrings = new Collection<string>(_printingDevice.GetGhostScriptParameters(_ghostscriptVersion));
            DevicesGeneralTests.CheckDefaultParameters(_parameterStrings);
        }

        [Test]
        public void CheckDeviceSpecificDefaultParameters()
        {
            _parameterStrings = new Collection<string>(_printingDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-c", _parameterStrings, "Missing device default parameter.");
            var markString = GetMarkString(_parameterStrings);

            var cIndex = _parameterStrings.IndexOf("-c");
            var markIndex = _parameterStrings.IndexOf(markString);
            Assert.Less(cIndex, markIndex, "-c must be set in front of the mark parameters.");

            Assert.IsTrue(markString.Contains("mark /NoCancel false /BitsPerPixel 24 "), "Missing mark parameter: \"mark /NoCancel false /BitsPerPixel 24 \"");
            
            const string markParameter = "/UserSettings 1 dict dup /DocumentName (" + TestFile + ") put (mswinpr2) finddevice putdeviceprops setdevice";
            Assert.IsTrue(markString.Contains(markParameter), "Missing mark parameter: " + markParameter);
        }

        [Test]
        public void PrintingDevice_ParametersTest_DefaultPrinter_IsValid()
        {
            _printingDevice.Profile.Printing.Enabled = true;
            _printingDevice.Profile.Printing.SelectPrinter = SelectPrinter.DefaultPrinter;
            _printingDevice.Profile.Printing.PrinterName = "Some different PrinterName";

            _printerStub.Stub(x => x.IsValid).Return(true);
            _printerStub.PrinterName = PrinterName; 

            _parameterStrings = new Collection<string>(_printingDevice.GetGhostScriptParameters(_ghostscriptVersion));

            var markString = GetMarkString(_parameterStrings);
            Assert.IsTrue(markString.Contains("/OutputFile (\\\\\\\\spool\\\\" + PrinterName + ")"));
        }

        [Test]
        public void PrintingDevice_ParametersTest_DefaultPrinter_IsNotValid()
        {
            _printingDevice.Profile.Printing.Enabled = true;
            _printingDevice.Profile.Printing.SelectPrinter = SelectPrinter.DefaultPrinter;
            _printingDevice.Profile.Printing.PrinterName = "Some different PrinterName";

            _printerStub.Stub(x => x.IsValid).Return(false);
            _printerStub.PrinterName = PrinterName;

            var exception = Assert.Throws<Exception>(
                () => { _parameterStrings = new Collection<string>(_printingDevice.GetGhostScriptParameters(_ghostscriptVersion)); });
            Assert.AreEqual("100", exception.Message, "Wrong errorcode in exception.");
        }

        [Test]
        public void PrintingDevice_ParametersTest_SelectedPrinter_IsValid()
        {
            _printingDevice.Profile.Printing.Enabled = true;
            _printingDevice.Profile.Printing.SelectPrinter = SelectPrinter.SelectedPrinter;
            _printingDevice.Profile.Printing.PrinterName = PrinterName;

            _printerStub.Stub(x => x.IsValid).Return(true);
            _printerStub.PrinterName = "Some different PrinterName";

            _parameterStrings = new Collection<string>(_printingDevice.GetGhostScriptParameters(_ghostscriptVersion));

            var markString = GetMarkString(_parameterStrings);
            Assert.IsTrue(markString.Contains("/OutputFile (\\\\\\\\spool\\\\" + PrinterName + ")"));
        }

        [Test]
        public void PrintingDevice_ParametersTest_SelectedPrinter_IsNotValid()
        {
            _printingDevice.Profile.Printing.Enabled = true;
            _printingDevice.Profile.Printing.SelectPrinter = SelectPrinter.SelectedPrinter;
            _printingDevice.Profile.Printing.PrinterName = PrinterName;

            _printerStub.Stub(x => x.IsValid).Return(false);
            _printerStub.PrinterName = "Some different PrinterName";

            var exception = Assert.Throws<Exception>(
                () => { _parameterStrings = new Collection<string>(_printingDevice.GetGhostScriptParameters(_ghostscriptVersion)); });
            Assert.AreEqual("101", exception.Message, "Wrong errorcode in exception.");
        }

        [Test]
        public void PrintingDevice_ParametersTest_PrinterDialog()
        {
            _printingDevice.Profile.Printing.Enabled = true;
            _printingDevice.Profile.Printing.SelectPrinter = SelectPrinter.ShowDialog;
            _printingDevice.Profile.Printing.PrinterName = PrinterName;

            _printerStub.Stub(x => x.IsValid).Return(true);
            _printerStub.PrinterName = "Some different PrinterName";

            _parameterStrings = new Collection<string>(_printingDevice.GetGhostScriptParameters(_ghostscriptVersion));

            var markString = GetMarkString(_parameterStrings);
            Assert.IsFalse(markString.Contains("/OutputFile (\\\\\\\\spool\\\\"));
        }

        [Test]
        public void PrintingDevice_ParametersTest_DuplexLongEdge_PrinterCanDuplex()
        {
            _printingDevice.Profile.Printing.Enabled = true;
            _printingDevice.Profile.Printing.SelectPrinter = SelectPrinter.DefaultPrinter;
            _printingDevice.Profile.Printing.Duplex = DuplexPrint.LongEdge;
            
            _printerStub.Stub(x => x.IsValid).Return(true);
            _printerStub.PrinterName = PrinterName;
            _printerStub.Stub(x => x.CanDuplex).Return(true);

            _parameterStrings = new Collection<string>(_printingDevice.GetGhostScriptParameters(_ghostscriptVersion));
            
            Assert.Contains("<< /Duplex true /Tumble false >> setpagedevice ", _parameterStrings, "Missing parameter.");
        }

        [Test]
        public void PrintingDevice_ParametersTest_DuplexLongEdge_PrinterCanDuplexIsFalse()
        {
            _printingDevice.Profile.Printing.Enabled = true;
            _printingDevice.Profile.Printing.SelectPrinter = SelectPrinter.DefaultPrinter;
            _printingDevice.Profile.Printing.Duplex = DuplexPrint.LongEdge;

            _printerStub.Stub(x => x.IsValid).Return(true);
            _printerStub.PrinterName = PrinterName;
            _printerStub.Stub(x => x.CanDuplex).Return(false);

            _parameterStrings = new Collection<string>(_printingDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.IsNull(_parameterStrings.FirstOrDefault(x => x.StartsWith("<< /Duplex")), "Falsely set duplex parameter.");
        }

        [Test]
        public void PrintingDevice_ParametersTest_DuplexShortEdge_PrinterCanDuplex()
        {
            _printingDevice.Profile.Printing.Enabled = true;
            _printingDevice.Profile.Printing.SelectPrinter = SelectPrinter.DefaultPrinter;
            _printingDevice.Profile.Printing.Duplex = DuplexPrint.ShortEdge;

            _printerStub.Stub(x => x.IsValid).Return(true);
            _printerStub.PrinterName = PrinterName;
            _printerStub.Stub(x => x.CanDuplex).Return(true);

            _parameterStrings = new Collection<string>(_printingDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("<< /Duplex true /Tumble true >> setpagedevice ", _parameterStrings, "Missing parameter.");
        }

        [Test]
        public void PrintingDevice_ParametersTest_DuplexShortEdge_PrinterCanDuplexIsFalse()
        {
            _printingDevice.Profile.Printing.Enabled = true;
            _printingDevice.Profile.Printing.SelectPrinter = SelectPrinter.DefaultPrinter;
            _printingDevice.Profile.Printing.Duplex = DuplexPrint.ShortEdge;

            _printerStub.Stub(x => x.IsValid).Return(true);
            _printerStub.PrinterName = PrinterName;
            _printerStub.Stub(x => x.CanDuplex).Return(false);

            _parameterStrings = new Collection<string>(_printingDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.IsNull(_parameterStrings.FirstOrDefault(x => x.StartsWith("<< /Duplex")), "Falsely set duplex parameter.");
        }

        [Test]
        public void PrintingDevice_ParametersTest_DuplexDisabled_PrinterCanDuplex()
        {
            _printingDevice.Profile.Printing.Enabled = true;
            _printingDevice.Profile.Printing.SelectPrinter = SelectPrinter.DefaultPrinter;
            _printingDevice.Profile.Printing.Duplex = DuplexPrint.Disable;

            _printerStub.Stub(x => x.IsValid).Return(true);
            _printerStub.PrinterName = PrinterName;
            _printerStub.Stub(x => x.CanDuplex).Return(true);

            _parameterStrings = new Collection<string>(_printingDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.IsNull(_parameterStrings.FirstOrDefault(x => x.StartsWith("<< /Duplex")), "Falsely set duplex parameter.");
        }
    }
}