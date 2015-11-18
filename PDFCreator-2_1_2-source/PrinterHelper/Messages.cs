namespace pdfforge.PrinterHelper
{
    public static class Messages
    {
        public const string FoundInvalidCommands = "Found invalid command(s):";
        public const string FoundInvalidCommandsCombination = "A combination of these commands is not allowed:";
        public const string FoundUnknownCommandsCombination = "A combination of these commands is unknown!";
        public const string FoundNoCommands = "Found no commands!";
        public const string FoundNoValidCommandsCombination = "Found no valid commands!";
        public const string UnknownCommandExecutionError = "An unknown error occured executing command '{0}'!";
        public const string CommandDoesntSupportParameters = "The command '{0}' doesn't support parameters:";
        public const string CommandSupportsOnlyOneParameter = "The command '{0}' supports only one parameters!";
        public const string CommandWrongNumberOfParameters = "Wrong number of parameters for command '{0}'! Please check usage.";

        public const string NoPrinterNameSpecified = "There was no printer name specified!";
        public const string NoValidPrinterNameSpecified = "There was no valid printer name specified!";
        public const string PrintersAlreadyInstalled = "These printers are already installed:";
        public const string InValidCharInPrinterName = "The chars \"!\", \"\\\" and \",\" are not allowed in a printer name!";
        public const string NoPdfcreatorPrinterInstalled = "No PDFCreator printer was found!";
        public const string RestartSpoolerServiceFailed = "A necessary restart of spooler service failed!";
        public const string PrintersUsingPdfcreatorMonitor = "Can't delete the PDFCreator print monitor. These printers are using the PDFCreator print monitor:";
        public const string PrinterFreememSettingFailure = "Could not adapt the freemem setting for printer '{0}'!";
        
        public const string PpdFileNotFound = "The ppd-file '{0}' could not found!";
        public const string NoPpdFileSpecified = "There was no ppd-file specified!";

        public const string NoPortApplicationSpecified = "There was no port application specified!";
        public const string NoValidPortApplicationSpecified = "There was no valid port application specified!";
        public const string OnlyOnePortApplicationAllowed = "You can define only one port application!";
        public const string PathPortApplicationNotValid = "The path for the port application is not valid!";
        public const string PathPortApplicationNotRooted = "The path for the port application must be a rooted path!";
        public const string PrinterMonitorUnDeletable = "The printer monitor is not deletable!";

        public const string InstallPdfCreatorComment = " You have to install the complete 'PDFCreator' printer with the command 'InstallPrinter' before you can add another one.";
        public const string NoPortSpecified = "There was no port name specified!";
        public const string PdfCreatorPortNotInstalled = "PDFCreator port 'pdfcmon' not installed!";
        public const string NoPrinterDriverNameSpecified = "There was no driver name specified!";
        public const string NoPdfCreatorPrinterDriverInstalled = "PDFCreator driver 'PDFCreator' not installed!";
        public const string PrinterDriverUnDeletable = "The printer driver is not deletable!";

        public const string NoNewPrinterName = "Missing the new name for the printer! Check usage.";
        public const string OnlyTwoPrinterNamesAllowedForRenaming = "Only 2 values (old printer name and new printer name) are allowed for command 'RenamePrinter'! Check usage.";
        public const string PrinterNotInstalled = "Printer '{0}' is not installed!";
        public const string NewPrinterNameForRenamingAlreadyExists = "The given new printer '{0}' already exists!";

        public const string PrinterPortAlreadyInstalled = "Printer port already installed. Uninstall the port before you use the command 'AddPrinter'.";
        public const string PrinterDriverAlreadyInstalled = "Printer driver already installed. Uninstall the driver before you use the command 'AddPrinter'.";
        public const string PrintersAreNotInstalled = "These given printers are not installed:";
        public const string PrintersAreNotPdfCreatorPrinters = "These given printers are not PDFCreator printers:";
    }
}