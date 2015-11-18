using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using pdfforge.PrinterHelper;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;

namespace PrinterHelper.Test
{
    [TestFixture]
    public class Commands
    {
        private const string PortName = "Port1";
        private const string DriverName = "Driver";
        private const string PrinterName1 = "Printer1";
        private const string PrinterName2 = "Printer2";

        [Test]
        public void CheckCommands_WithNoCommandLineList_ReturnsErrorObject()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var result = commands.CheckCommands(null, null);

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_COMMAND, result.Code);
        }
        
        [Test]
        public void CheckCommands_WithEmptyCommandLineList_ReturnsErrorObject()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var result = commands.CheckCommands(new Dictionary<string, IList<string>>(), null);

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_COMMAND, result.Code);
        }

        [Test]
        public void CheckCommands_WithNoValidCommandList_ReturnsErrorObject()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var result = commands.CheckCommands(new Dictionary<string, IList<string>>(), null);

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_COMMAND, result.Code);
        }

        [Test]
        public void CheckCommands_WithEmptyValidCommandList_ReturnsErrorObject()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var result = commands.CheckCommands(new Dictionary<string, IList<string>>(), new List<string>());

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_COMMAND, result.Code);
        }

        [Test]
        public void CheckCommands_WithNotValidCommandListForCommandAddPrinter_ReturnsErrorObject()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var result = commands.CheckCommands(new Dictionary<string, IList<string>> { { "Hello", new List<string>() } }, pdfforge.PrinterHelper.Commands.ValidCommands);

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_COMMAND, result.Code);
        }

        [Test]
        public void CheckCommands_OnlyWithCommandLog_ReturnsErrorObject()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var result = commands.CheckCommands(new Dictionary<string, IList<string>> { { "log", new List<string>() } }, pdfforge.PrinterHelper.Commands.ValidCommands);

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_COMMAND, result.Code);
        }

        [Test]
        public void CheckCommands_WithCommandAddPrinter_ReturnsNull()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var result = commands.CheckCommands(new Dictionary<string, IList<string>> { { "addprinter", new List<string>() } }, pdfforge.PrinterHelper.Commands.ValidCommands);

            Assert.IsNull(result);
        }

        [Test]
        public void CheckCommands_WithCommandAddPrinterAndCommandLog_ReturnsNull()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var commandsDictionary = new Dictionary<string, IList<string>>
            {
                {"addprinter", new List<string>()},
                {"log", new List<string>()}
            };
            var result = commands.CheckCommands(commandsDictionary, pdfforge.PrinterHelper.Commands.ValidCommands);

            Assert.IsNull(result);
        }

        [Test]
        public void CheckCommands_WithCommandAddPrinterAndCommandRenamePrinter_ReturnsErrorObject()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var commandsDictionary = new Dictionary<string, IList<string>>
            {
                {"addprinter", new List<string>()},
                {"renameprinter", new List<string>()},
            };
            var result = commands.CheckCommands(commandsDictionary, pdfforge.PrinterHelper.Commands.ValidCommands);

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_COMMAND, result.Code);
        }

        [Test]
        public void CheckCommands_WithCommandAddPrinterAndCommandLogAndCommandRenamePrinter_ReturnsErrorObject()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var commandsDictionary = new Dictionary<string, IList<string>>
            {
                {"addprinter", new List<string>()},
                {"renameprinter", new List<string>()},
                {"log", new List<string>()},
            };
            var result = commands.CheckCommands(commandsDictionary, pdfforge.PrinterHelper.Commands.ValidCommands);

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_COMMAND, result.Code);
        }

        [Test]
        public void CheckCommands_WithNotValidCommandListForCommandRenamePrinter_ReturnsErrorObject()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var result = commands.CheckCommands(new Dictionary<string, IList<string>> { { "Hello", new List<string>() } }, pdfforge.PrinterHelper.Commands.ValidCommands);

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_COMMAND, result.Code);
        }

        [Test]
        public void CheckCommands_WithCommandRenamePrinter_ReturnsNull()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var result = commands.CheckCommands(new Dictionary<string, IList<string>> { { "renameprinter", new List<string>() } }, pdfforge.PrinterHelper.Commands.ValidCommands);

            Assert.IsNull(result);
        }

        [Test]
        public void CheckCommands_WithCommandRenamePrinterAndCommandLog_ReturnsNull()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var commandsDictionary = new Dictionary<string, IList<string>>
            {
                {"renameprinter", new List<string>()},
                {"log", new List<string>()}
            };
            var result = commands.CheckCommands(commandsDictionary, pdfforge.PrinterHelper.Commands.ValidCommands);

            Assert.IsNull(result);
        }

        [Test]
        public void CheckCommands_WithCommandRenamePrinterAndCommandDeletePrinter_ReturnsErrorObject()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var commandsDictionary = new Dictionary<string, IList<string>>
            {
                {"renameprinter", new List<string>()},
                {"deleteprinter", new List<string>()},
            };
            var result = commands.CheckCommands(commandsDictionary, pdfforge.PrinterHelper.Commands.ValidCommands);

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_COMMAND, result.Code);
        }

        [Test]
        public void CheckCommands_WithCommandRenamePrinterAndCommandLogAndCommandDeletePrinter_ReturnsErrorObject()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var commandsDictionary = new Dictionary<string, IList<string>>
            {
                {"renameprinter", new List<string>()},
                {"deleteprinter", new List<string>()},
                {"log", new List<string>()},
            };
            var result = commands.CheckCommands(commandsDictionary, pdfforge.PrinterHelper.Commands.ValidCommands);

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_COMMAND, result.Code);
        }

        [Test]
        public void CheckCommands_WithNotValidCommandListForCommandDeletePrinter_ReturnsErrorObject()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var result = commands.CheckCommands(new Dictionary<string, IList<string>> { { "Hello", new List<string>() } }, pdfforge.PrinterHelper.Commands.ValidCommands);

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_COMMAND, result.Code);
        }

        [Test]
        public void CheckCommands_WithCommandDeletePrinter_ReturnsNull()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var result = commands.CheckCommands(new Dictionary<string, IList<string>> { { "deleteprinter", new List<string>() } }, pdfforge.PrinterHelper.Commands.ValidCommands);

            Assert.IsNull(result);
        }

        [Test]
        public void CheckCommands_WithCommandDeletePrinterAndCommandLog_ReturnsNull()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var commandsDictionary = new Dictionary<string, IList<string>>
            {
                {"deleteprinter", new List<string>()},
                {"log", new List<string>()}
            };
            var result = commands.CheckCommands(commandsDictionary, pdfforge.PrinterHelper.Commands.ValidCommands);

            Assert.IsNull(result);
        }

        [Test]
        public void CheckCommands_WithCommandDeletePrinterAndCommandAddPrinter_ReturnsErrorObject()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var commandsDictionary = new Dictionary<string, IList<string>>
            {
                {"deleteprinter", new List<string>()},
                {"addprinter", new List<string>()},
            };
            var result = commands.CheckCommands(commandsDictionary, pdfforge.PrinterHelper.Commands.ValidCommands);

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_COMMAND, result.Code);
        }

        [Test]
        public void CheckCommands_WithCommandDeletePrinterAndCommandLogAndCommandAddPrinter_ReturnsErrorObject()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var commandsDictionary = new Dictionary<string, IList<string>>
            {
                {"deleteprinter", new List<string>()},
                {"addprinter", new List<string>()},
                {"log", new List<string>()},
            };
            var result = commands.CheckCommands(commandsDictionary, pdfforge.PrinterHelper.Commands.ValidCommands);

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_COMMAND, result.Code);
        }

        [Test]
        public void CheckCommands_WithNotValidCommandListForCommandInstallPrinter_ReturnsErrorObject()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var result = commands.CheckCommands(new Dictionary<string, IList<string>> { { "Hello", new List<string>() } }, pdfforge.PrinterHelper.Commands.ValidCommands);

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_COMMAND, result.Code);
        }

        [Test]
        public void CheckCommands_WithCommandInstallPrinter_ReturnsErrorObject()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var result = commands.CheckCommands(new Dictionary<string, IList<string>> { { "installprinter", new List<string>() } }, pdfforge.PrinterHelper.Commands.ValidCommands);

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_COMMAND, result.Code);
        }

        [Test]
        public void CheckCommands_WithCommandInstallPrinterAndCommandPortApplication_ReturnsNull()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var commandsDictionary = new Dictionary<string, IList<string>>
            {
                {"installprinter", new List<string>()},
                {"portapplication", new List<string>()},
            };
            var result = commands.CheckCommands(commandsDictionary, pdfforge.PrinterHelper.Commands.ValidCommands);

            Assert.IsNull(result);
        }

        [Test]
        public void CheckCommands_WithCommandInstallPrinterAndCommandPortApplicationAndCommandLog_ReturnsNull()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var commandsDictionary = new Dictionary<string, IList<string>>
            {
                {"installprinter", new List<string>()},
                {"portapplication", new List<string>()},
                {"log", new List<string>()},
            };
            var result = commands.CheckCommands(commandsDictionary, pdfforge.PrinterHelper.Commands.ValidCommands);

            Assert.IsNull(result);
        }

        [Test]
        public void CheckCommands_WithCommandInstallPrinterAndCommandPpdFileAndCommandLog_ReturnsErrorObject()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var commandsDictionary = new Dictionary<string, IList<string>>
            {
                {"installprinter", new List<string>()},
                {"ppdfile", new List<string>()},
                {"log", new List<string>()},
            };
            var result = commands.CheckCommands(commandsDictionary, pdfforge.PrinterHelper.Commands.ValidCommands);

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_COMMAND, result.Code);
        }
        
        [Test]
        public void CheckCommands_WithCommandInstallPrinterAndCommandPortApplicationAndCommandPpdFileAndCommandLog_ReturnsNull()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var commandsDictionary = new Dictionary<string, IList<string>>
            {
                {"installprinter", new List<string>()},
                {"portapplication", new List<string>()},
                {"ppdfile", new List<string>()},
                {"log", new List<string>()},
            };
            var result = commands.CheckCommands(commandsDictionary, pdfforge.PrinterHelper.Commands.ValidCommands);

            Assert.IsNull(result);
        }

        [Test]
        public void CheckCommands_WithCommandInstallPrinterAndCommandPortApplicationAndCommandAddPrinter_ReturnsErrorObject()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var commandsDictionary = new Dictionary<string, IList<string>>
            {
                {"installprinter", new List<string>()},
                {"portapplication", new List<string>()},
                {"addprinter", new List<string>()},
            };
            var result = commands.CheckCommands(commandsDictionary, pdfforge.PrinterHelper.Commands.ValidCommands);

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_COMMAND, result.Code);
        }

        [Test]
        public void CheckCommands_WithCommandInstallPrinterAndCommandAddPrinter_ReturnsErrorObject()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var commandsDictionary = new Dictionary<string, IList<string>>
            {
                {"installprinter", new List<string>()},
                {"addprinter", new List<string>()},
            };
            var result = commands.CheckCommands(commandsDictionary, pdfforge.PrinterHelper.Commands.ValidCommands);

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_COMMAND, result.Code);
        }

        [Test]
        public void CheckCommands_WithNotValidCommandListForCommandUnInstallPrinter_ReturnsErrorObject()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var result = commands.CheckCommands(new Dictionary<string, IList<string>> { { "Hello", new List<string>() } }, pdfforge.PrinterHelper.Commands.ValidCommands);

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_COMMAND, result.Code);
        }

        [Test]
        public void CheckCommands_WithCommandUninstallPrinter_ReturnsNull()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var result = commands.CheckCommands(new Dictionary<string, IList<string>> { { "deleteprinter", new List<string>() } }, pdfforge.PrinterHelper.Commands.ValidCommands);

            Assert.IsNull(result);
        }

        [Test]
        public void CheckCommands_WithCommandUninstallPrinterAndCommandLog_ReturnsNull()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var commandsDictionary = new Dictionary<string, IList<string>>
            {
                {"uninstallprinter", new List<string>()},
                {"log", new List<string>()}
            };
            var result = commands.CheckCommands(commandsDictionary, pdfforge.PrinterHelper.Commands.ValidCommands);

            Assert.IsNull(result);
        }

        [Test]
        public void CheckCommands_WithCommandUninstallPrinterAndCommandAddPrinter_ReturnsErrorObject()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var commandsDictionary = new Dictionary<string, IList<string>>
            {
                {"deleteprinter", new List<string>()},
                {"addprinter", new List<string>()},
            };
            var result = commands.CheckCommands(commandsDictionary, pdfforge.PrinterHelper.Commands.ValidCommands);

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_COMMAND, result.Code);
        }

        [Test]
        public void CheckCommands_WithCommandUninstallPrinterAndCommandLogAndCommandAddPrinter_ReturnsErrorObject()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var commandsDictionary = new Dictionary<string, IList<string>>
            {
                {"uninstallprinter", new List<string>()},
                {"addprinter", new List<string>()},
                {"log", new List<string>()},
            };
            var result = commands.CheckCommands(commandsDictionary, pdfforge.PrinterHelper.Commands.ValidCommands);

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_COMMAND, result.Code);
        }

        [Test]
        public void CheckCommandsCount_WithOneCommandsAndCommandsIsOneAndFirstCommandIsNotCommandLog_ReturnsNull()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var commandsDictionary = new Dictionary<string, IList<string>>
            {
                {"addprinter", new List<string>()},
            };
            var result = commands.CheckCommandsCount(commandsDictionary, 2);

            Assert.IsNull(result);
        }

        [Test]
        public void CheckCommandsCount_WithTwoCommandsAndCommandsCountIsTwoAndSecondCommandIsNotCommandLog_ReturnsErrorObject()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var commandsDictionary = new Dictionary<string, IList<string>>
            {
                {"addprinter", new List<string>()},
                {"uninstallprinter", new List<string>()},
            };
            var result = commands.CheckCommandsCount(commandsDictionary, 2);

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_COMMAND, result.Code);
        }

        [Test]
        public void CheckCommandsCount_WithTwoCommandsAndCommandsCountIsTwoAndSecondCommandIsCommandLog_ReturnsNull()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var commandsDictionary = new Dictionary<string, IList<string>>
            {
                {"uninstallprinter", new List<string>()},
                {"log", new List<string>()},
            };
            var result = commands.CheckCommandsCount(commandsDictionary, 2);

            Assert.IsNull(result);
        }

        [Test]
        public void CheckCommandsCount_WithThreeCommandsAndCommandsCountIsThreeAndThirdCommandIsNotCommandLog_ReturnsErrorObject()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var commandsDictionary = new Dictionary<string, IList<string>>
            {
                {"addprinter", new List<string>()},
                {"installprinter", new List<string>()},
                {"uninstallprinter", new List<string>()},
            };
            var result = commands.CheckCommandsCount(commandsDictionary, 3);

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_COMMAND, result.Code);
        }

        [Test]
        public void CheckCommandsCount_WithThreeCommandsAndCommandsCountIsThreeAndThirdCommandIsCommandLog_ReturnsNull()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var commandsDictionary = new Dictionary<string, IList<string>>
            {
                {"installprinter", new List<string>()},
                {"portapplication", new List<string>()},
                {"log", new List<string>()},
            };
            var result = commands.CheckCommandsCount(commandsDictionary, 3);

            Assert.IsNull(result);
        }

        [Test]
        public void CheckLogging_WithNoCommands_ReturnsNullAndLoggingObjectWithNoLogging()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var commandsDictionary = new Dictionary<string, IList<string>>();

            ILogging logging;
            var result = commands.CheckLogging(commandsDictionary, out logging);

            Assert.IsNull(result);
            Assert.AreEqual(logging.LogLevel, LogLevel.NoLogging);
        }

        [Test]
        public void CheckLogging_WithNoLogCommand_ReturnsNullAndLoggingObjectWithNoLogging()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var commandsDictionary = new Dictionary<string, IList<string>>
            {
                {"installprinter", new List<string>()},
                {"portapplication", new List<string>()},
            };

            ILogging logging;
            var result = commands.CheckLogging(commandsDictionary, out logging);

            Assert.IsNull(result);
            Assert.AreEqual(logging.LogLevel, LogLevel.NoLogging);
        }

        [Test]
        public void CheckLogging_WithLogCommandWithoutLogParameter_ReturnsNullAndLoggingObjectWithLogToConsole()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var commandsDictionary = new Dictionary<string, IList<string>>
            {
                {"installprinter", new List<string>()},
                {"portapplication", new List<string>()},
                {"log", new List<string>()},
            };

            ILogging logging;
            var result = commands.CheckLogging(commandsDictionary, out logging);

            Assert.IsNull(result);
            Assert.AreEqual(logging.LogLevel, LogLevel.LogToConsole);
        }

        [Test]
        public void CheckLogging_WithLogCommandAndValidFilenameAsLogParameter_ReturnsNullAndLoggingObjectWithLogToFile()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var commandsDictionary = new Dictionary<string, IList<string>>
            {
                {"installprinter", new List<string>()},
                {"portapplication", new List<string>()},
                {"log", new List<string>{ Path.Combine(Path.GetTempPath(), "LogFile.txt") }},
            };

            var loggingStub = MockRepository.GenerateStub<ILogging>();

            var result = commands.CheckLogging(commandsDictionary, out loggingStub);

            Assert.IsNull(result);
            Assert.AreEqual(loggingStub.LogLevel, LogLevel.LogToFile);
        }
        
        [Test]
        public void CheckLogging_WithLogCommandAndInvalidFilenameAsLogParameter_ReturnsErrorObject()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            var commandsDictionary = new Dictionary<string, IList<string>>
            {
                {"installprinter", new List<string>()},
                {"portapplication", new List<string>()},
                {"log", new List<string>{ Path.Combine(Path.GetTempPath(), ":LogFile.txt") }},
            };

            ILogging loggingStub;

            var result = commands.CheckLogging(commandsDictionary, out loggingStub);

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_PARAMETER, result.Code);
        }
        
        [Test]
        public void CheckCommandAddPrinterAndRun_WithEmptyArgumentList_ReturnsNull()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            string[] cmdStrings = { "" };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            var loggingStub = MockRepository.GenerateStub<ILogging>();

            var result = commands.CheckCommandAddPrinterAndExecute(p, win32PrinterStub, PrinterEnvironment.WindowsX64, PortName, DriverName, loggingStub);

            Assert.IsNull(result);
        }

        [Test]
        public void CheckCommandAddPrinterAndRun_WithoutCommandAddPrinter_ReturnsNull()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            string[] cmdStrings = { "/RenamePrinter" };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            var loggingStub = MockRepository.GenerateStub<ILogging>();

            var result = commands.CheckCommandAddPrinterAndExecute(p, win32PrinterStub, PrinterEnvironment.WindowsX64, PortName, DriverName, loggingStub);

            Assert.IsNull(result);
        }

        [Test]
        public void CheckCommandAddPrinterAndRun_WithCommandAddPrinterAndNoPrinternames_ReturnsErrorObject()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            string[] cmdStrings = { "/AddPrinter" };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            var loggingStub = MockRepository.GenerateStub<ILogging>();
            win32PrinterStub.Stub(x => x.GetInstalledPrintersFromList(null)).Return(new List<string> {PrinterName1});

            var result = commands.CheckCommandAddPrinterAndExecute(p, win32PrinterStub, PrinterEnvironment.WindowsX64, PortName, DriverName, loggingStub);

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_PARAMETER, result.Code);
        }

        [Test]
        public void CheckCommandAddPrinterAndRun_WithCommandAddPrinterAndOnePrinternameAndPrinterIsAlreadyInstalled_ReturnsErrorObject()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            string[] cmdStrings = { "/AddPrinter", PrinterName1 };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            var loggingStub = MockRepository.GenerateStub<ILogging>();
            win32PrinterStub.Stub(x => x.GetInstalledPrintersFromList(p["addprinter"])).Return(new List<string> { PrinterName1 });

            var result = commands.CheckCommandAddPrinterAndExecute(p, win32PrinterStub, PrinterEnvironment.WindowsX64, PortName, DriverName, loggingStub);

            Assert.IsNotNull(result);
            Assert.AreEqual(Error.ERROR_INVALID_PARAMETER, result.Code);
        }

        [Test]
        public void CheckCommandAddPrinterAndRun_WithCommandAddPrinterCorrectUsing_ReturnsNull()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            string[] cmdStrings = { "/AddPrinter", PrinterName1, "/PortApplication", @"C:\Test.exe" };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            var loggingStub = MockRepository.GenerateStub<ILogging>();
            win32PrinterStub.Stub(x => x.GetInstalledPrintersFromList(p["addprinter"])).Return(new List<string>()).Repeat.Once();
            win32PrinterStub.Stub(x => x.GetInstalledPrintersFromList(p["addprinter"])).Return(new List<string> { PrinterName1 }).Repeat.Once();
            win32PrinterStub.Stub(x => x.IsPortInstalled(PortName)).Return(true);
            win32PrinterStub.Stub(x => x.IsDriverInstalled(Printer.CurrentPrinterEnvironment, DriverName)).Return(true);
            win32PrinterStub.Stub(x => x.AdaptFreememPrinterSetting(PrinterName1, 32767)).Return(null).Repeat.Once();

            var result = commands.CheckCommandAddPrinterAndExecute(p, win32PrinterStub, PrinterEnvironment.WindowsX64, PortName, DriverName, loggingStub);

            Assert.IsNull(result);
        }

        [Test]
        public void CheckCommandAddPrinterAndRun_WithTwoCommandAddPrinter_ReturnsNull()
        {
            var commands = new pdfforge.PrinterHelper.Commands();
            string[] cmdStrings = { "/AddPrinter", PrinterName1, "/AddPrinter", PrinterName2 };
            var p = CommandLineParser.ParseCommandLine(cmdStrings, true, true);

            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();
            var loggingStub = MockRepository.GenerateStub<ILogging>();
            win32PrinterStub.Stub(x => x.GetInstalledPrintersFromList(p["addprinter"])).Return(new List<string>()).Repeat.Once();
            win32PrinterStub.Stub(x => x.GetInstalledPrintersFromList(p["addprinter"])).Return(new List<string> { PrinterName1, PrinterName2 }).Repeat.Once();
            win32PrinterStub.Stub(x => x.IsPortInstalled(PortName)).Return(true);
            win32PrinterStub.Stub(x => x.IsDriverInstalled(Printer.CurrentPrinterEnvironment, DriverName)).Return(true);
            win32PrinterStub.Stub(x => x.AdaptFreememPrinterSetting(PrinterName1, 32767)).Return(null).Repeat.Once();
            win32PrinterStub.Stub(x => x.AdaptFreememPrinterSetting(PrinterName2, 32767)).Return(null).Repeat.Once();

            var result = commands.CheckCommandAddPrinterAndExecute(p, win32PrinterStub, PrinterEnvironment.WindowsX64, PortName, DriverName, loggingStub);

            Assert.IsNull(result);
        }
    }
}
