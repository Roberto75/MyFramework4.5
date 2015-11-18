using NUnit.Framework;
using pdfforge.PrinterHelper;
using Rhino.Mocks;

namespace PrinterHelper.Test
{
    [TestFixture]
    public class Program
    {
        readonly string[] _args = { "/Test" };

        [Test]
        public void Run_WithCommands_CallsCheckCommands()
        {
            var commandsStub = MockRepository.GenerateStub<ICommands>();
            var p = CommandLineParser.ParseCommandLine(_args, true, true);

            commandsStub.Stub(x => x.CheckCommands(p, pdfforge.PrinterHelper.Commands.ValidCommands)).Return(new Error(Error.ERROR_INVALID_COMMAND, ""));
            new pdfforge.PrinterHelper.Program().Run(p, commandsStub, new Win32Printer(), new Logging(LogLevel.NoLogging, ""));

            commandsStub.AssertWasCalled(x => x.CheckCommands(p, pdfforge.PrinterHelper.Commands.ValidCommands), options => options.Repeat.Once());
        }

        [Test]
        public void Run_WithCommands_CallsCheckLogging()
        {
            var commandsStub = MockRepository.GenerateStub<ICommands>();
            var p = CommandLineParser.ParseCommandLine(_args, true, true);

            commandsStub.Stub(x => x.CheckCommands(p, pdfforge.PrinterHelper.Commands.ValidCommands)).Return(null);
            ILogging logging = new Logging(LogLevel.NoLogging, "");
            commandsStub.Stub(x => x.CheckLogging(p, out logging)).Return(new Error(Error.ERROR_INVALID_COMMAND, ""));
            new pdfforge.PrinterHelper.Program().Run(p, commandsStub, new Win32Printer(), logging);

            commandsStub.AssertWasCalled(x => x.CheckCommands(p, pdfforge.PrinterHelper.Commands.ValidCommands), options => options.Repeat.Once());
        }

        [Test]
        public void Run_WithCommands_CallsLogAllPrinterInfos()
        {
            var loggingStub = MockRepository.GenerateStub<ILogging>();
            var commandsStub = MockRepository.GenerateStub<ICommands>();
            var p = CommandLineParser.ParseCommandLine(_args, true, true);

            commandsStub.Stub(x => x.CheckCommands(p, pdfforge.PrinterHelper.Commands.ValidCommands)).Return(null);
            ILogging logging;
            commandsStub.Stub(x => x.CheckLogging(p, out logging)).Return(null);
            new pdfforge.PrinterHelper.Program().Run(p, commandsStub, new Win32Printer(), loggingStub);

            loggingStub.AssertWasCalled(x => x.LogAllPrinterInfos(), options => options.Repeat.Twice());
        }

        [Test]
        public void Run_WithCommands_CallsCheckCommandAddPrinterAndRun()
        {
            var loggingStub = MockRepository.GenerateStub<ILogging>();
            var commandsStub = MockRepository.GenerateStub<ICommands>();
            var p = CommandLineParser.ParseCommandLine(_args, true, true);
            var printerEnvironment = Printer.CurrentPrinterEnvironment;
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();

            commandsStub.Stub(x => x.CheckCommands(p, pdfforge.PrinterHelper.Commands.ValidCommands)).Return(null);
            new pdfforge.PrinterHelper.Program().Run(p, commandsStub, win32PrinterStub, loggingStub);

            commandsStub.AssertWasCalled(x => x.CheckCommandAddPrinterAndExecute(p, win32PrinterStub, printerEnvironment, pdfforge.PrinterHelper.Program.PortName, pdfforge.PrinterHelper.Program.DriverName, loggingStub), options => options.Repeat.Once());
        }

        [Test]
        public void Run_WithCommands_CallsCheckCommandRenamePrinterAndRun()
        {
            var loggingStub = MockRepository.GenerateStub<ILogging>();
            var commandsStub = MockRepository.GenerateStub<ICommands>();
            var p = CommandLineParser.ParseCommandLine(_args, true, true);
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();

            commandsStub.Stub(x => x.CheckCommands(p, pdfforge.PrinterHelper.Commands.ValidCommands)).Return(null);
            new pdfforge.PrinterHelper.Program().Run(p, commandsStub, win32PrinterStub, loggingStub);

            commandsStub.AssertWasCalled(x => x.CheckCommandRenamePrinterAndExecute(p, win32PrinterStub, loggingStub), options => options.Repeat.Once());
        }

        [Test]
        public void Run_WithCommands_CallsCheckCommandDeletePrinterAndRun()
        {
            var loggingStub = MockRepository.GenerateStub<ILogging>();
            var commandsStub = MockRepository.GenerateStub<ICommands>();
            var p = CommandLineParser.ParseCommandLine(_args, true, true);
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();

            commandsStub.Stub(x => x.CheckCommands(p, pdfforge.PrinterHelper.Commands.ValidCommands)).Return(null);
            new pdfforge.PrinterHelper.Program().Run(p, commandsStub, win32PrinterStub, loggingStub);

            commandsStub.AssertWasCalled(x => x.CheckCommandDeletePrinterAndExecute(p, win32PrinterStub, pdfforge.PrinterHelper.Program.DriverName, loggingStub), options => options.Repeat.Once());
        }

        [Test]
        public void Run_WithCommands_CallsCheckCommandInstallPrinterAndRun()
        {
            var loggingStub = MockRepository.GenerateStub<ILogging>();
            var commandsStub = MockRepository.GenerateStub<ICommands>();
            var p = CommandLineParser.ParseCommandLine(_args, true, true);
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();

            commandsStub.Stub(x => x.CheckCommands(p, pdfforge.PrinterHelper.Commands.ValidCommands)).Return(null);
            new pdfforge.PrinterHelper.Program().Run(p, commandsStub, win32PrinterStub, loggingStub);

            commandsStub.AssertWasCalled(x => x.CheckCommandInstallPrinterAndExecute(p, win32PrinterStub, pdfforge.PrinterHelper.Program.MonitorName, pdfforge.PrinterHelper.Program.PortName, pdfforge.PrinterHelper.Program.DriverName, loggingStub), options => options.Repeat.Once());
        }

        [Test]
        public void Run_WithCommands_CallsCheckCommandUnInstallPrinterAndRun()
        {
            var loggingStub = MockRepository.GenerateStub<ILogging>();
            var commandsStub = MockRepository.GenerateStub<ICommands>();
            var p = CommandLineParser.ParseCommandLine(_args, true, true);
            var win32PrinterStub = MockRepository.GenerateStub<IWin32Printer>();

            commandsStub.Stub(x => x.CheckCommands(p, pdfforge.PrinterHelper.Commands.ValidCommands)).Return(null);
            new pdfforge.PrinterHelper.Program().Run(p, commandsStub, win32PrinterStub, loggingStub);

            commandsStub.AssertWasCalled(x => x.CheckCommandUnInstallPrinterAndExecute(p, win32PrinterStub, pdfforge.PrinterHelper.Program.DriverName, pdfforge.PrinterHelper.Program.MonitorName, loggingStub), options => options.Repeat.Once());
        }
    }
}
