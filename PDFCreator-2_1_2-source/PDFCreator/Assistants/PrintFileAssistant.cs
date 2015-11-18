using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NLog;
using pdfforge.PDFCreator.Helper;
using pdfforge.PDFCreator.PrintFile;
using pdfforge.PDFCreator.Shared.Helper;
using pdfforge.PDFCreator.Shared.ViewModels;
using pdfforge.PDFCreator.Shared.Views;

namespace pdfforge.PDFCreator.Assistants
{
    /// <summary>
    /// The PrintFileAssistant class provides a reusable way to manage user interaction (ask to change default printer, show responses for invalid files) while printing files
    /// </summary>
    class PrintFileAssistant
    {
        private readonly PrintCommandGroup _printCommands = new PrintCommandGroup();

        private readonly string _pdfCreatorPrinter;

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly TranslationHelper _translationHelper = TranslationHelper.Instance;

        public PrintFileAssistant()
        {
            var printerHelper =  new Shared.Helper.PrinterHelper();
            _pdfCreatorPrinter = printerHelper.GetApplicablePDFCreatorPrinter(SettingsHelper.Settings.ApplicationSettings.PrimaryPrinter, printerHelper.GetDefaultPrinter());
        }

        /// <summary>
        /// The default printer that will be used if no printer is specified
        /// </summary>
        public string DefaultPrinter { get { return _pdfCreatorPrinter; } }

        /// <summary>
        /// Add a single file. The file is checked and dialogs are presented to the user, if there are problems.
        /// </summary>
        /// <param name="file">A single file. If this is the path of a directory or an unprintable file, an error message will be shown.</param>
        /// <returns>true, if all files are printable</returns>
        public bool AddFile(string file)
        {
            return AddFiles(new []{file});
        }

        /// <summary>
        /// Add a single file. The file is checked and dialogs are presented to the user, if there are problems.
        /// </summary>
        /// <param name="file">A single file. If this is the path of a directory or an unprintable file, an error message will be shown.</param>
        /// <param name="printerName">Name of the printer that will be used</param>
        /// <returns>true, if all files are printable</returns>
        public bool AddFile(string file, string printerName)
        {
            return AddFiles(new[] { file }, printerName);
        }

        /// <summary>
        /// Add multiple files. The files are checked and dialogs are presented to the user, if there are problems.
        /// </summary>
        /// <param name="files">A list of files. If this contains a directory or files are not printable, an error message will be shown.</param>
        /// <returns>true, if all files are printable</returns>
        public bool AddFiles(IEnumerable<string> files)
        {
            return AddFiles(files, _pdfCreatorPrinter);
        }

        /// <summary>
        /// Add multiple files. The files are checked and dialogs are presented to the user, if there are problems.
        /// </summary>
        /// <param name="files">A list of files. If this contains a directory or files are not printable, an error message will be shown.</param>
        /// <param name="printerName">Name of the printer that will be used</param>
        /// <returns>true, if all files are printable</returns>
        public bool AddFiles(IEnumerable<string> files, string printerName)
        {
            foreach (var f in files)
            {
                _printCommands.Add(new PrintCommand(f, printerName));
            }

            var directories = _printCommands.FindAll(p => Directory.Exists(p.Filename));

            if (directories.Count > 0)
            {
                const string caption = "PDFCreator";
                string message = _translationHelper.GetTranslation("PrintFiles", "DirectoriesNotSupported", "You have tried to convert directories here. This is currently not supported.");
                MessageWindow.ShowTopMost(message, caption, MessageWindowButtons.OK, MessageWindowIcon.Warning);
                return false;
            }

            var unprintable = _printCommands.FindAll(p => !p.IsPrintable);

            if (unprintable.Any())
            {
                var fileList = new List<string>(unprintable.Select(p => Path.GetFileName(p.Filename)).Take(Math.Min(3, unprintable.Count)));
                const string caption = "PDFCreator";
                string message = _translationHelper.GetTranslation("PrintFiles", "NotPrintableFiles", "The following files can't be converted:") + "\r\n";

                message += string.Join("\r\n", fileList.ToArray());

                if (fileList.Count < unprintable.Count)
                    message += "\r\n" + _translationHelper.GetFormattedTranslation("PrintFiles", "AndXMore", "and {0} more.",
                        unprintable.Count - fileList.Count);

                MessageWindow.ShowTopMost(message, caption, MessageWindowButtons.OK, MessageWindowIcon.Warning);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Prints all files in the list.
        /// </summary>
        /// <returns>true, if all files could be printed</returns>
        public bool PrintAll()
        {
            if (String.IsNullOrEmpty(_pdfCreatorPrinter))
            {
                _logger.Error("No PDFCreator is installed.");
                return false;
            }

            var printerHelper = new Shared.Helper.PrinterHelper();

            bool requiresDefaultPrinter = _printCommands.RequiresDefaultPrinter;
            string defaultPrinter = printerHelper.GetDefaultPrinter();
            
            try
            {
                if (requiresDefaultPrinter)
                {
                    if (SettingsHelper.Settings.ApplicationSettings.AskSwitchDefaultPrinter)
                    {
                        var message = _translationHelper.GetTranslation("PrintFileHelper", "AskSwitchDefaultPrinter",
                            "PDFCreator needs to temporarily change the default printer to be able to convert the file. Do you want to proceed?");
                        const string caption = "PDFCreator";
                        if (
                            MessageWindow.ShowTopMost(message, caption, MessageWindowButtons.YesNo,
                                MessageWindowIcon.Question) != MessageWindowResponse.Yes)
                            return false;
                    }

                    Shared.Helper.PrinterHelper.SetDefaultPrinter(_pdfCreatorPrinter);
                }

                return _printCommands.PrintAll();
            }
            finally
            {
                if (requiresDefaultPrinter)
                    Shared.Helper.PrinterHelper.SetDefaultPrinter(defaultPrinter);
            }
        }
    }
}
