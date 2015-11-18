using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NLog;
using pdfforge.PDFCreator.Core.Settings.Enums;
using pdfforge.PDFCreator.Utilities;

namespace pdfforge.PDFCreator.Core.Ghostscript.OutputDevices
{
    /// <summary>
    ///     Extends OutputDevice for Printing with installed Windowsprinters
    /// </summary>
    public class PrintingDevice : OutputDevice
    {
        protected static Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly PrinterWrapper _printer;

        public PrintingDevice()
        {
            _printer = new PrinterWrapper();
        }

        public PrintingDevice(PrinterWrapper printerWrapper)
        {
            _printer = printerWrapper;
        }

        protected override void AddDeviceSpecificParameters(IList<string> parameters)
        {
            parameters.Add("-c");
            var markstring = new StringBuilder();
            markstring.Append("mark ");
            markstring.Append("/NoCancel false ");
            markstring.Append("/BitsPerPixel 24 "); //random = true color
            //var _printer = new PrinterSettings();
            switch (Profile.Printing.SelectPrinter)
            {
                case SelectPrinter.DefaultPrinter:
                    //printer.PrinterName returns default printer
                    if (!_printer.IsValid)
                    {
                        Logger.Error("The default printer (" + Profile.Printing.PrinterName + ") is invalid!");
                        throw new Exception("100");
                    }
                    markstring.Append("/OutputFile (\\\\\\\\spool\\\\" + _printer.PrinterName.Replace("\\", "\\\\") +
                                      ") ");
                    break;

                case SelectPrinter.SelectedPrinter:
                    _printer.PrinterName = Profile.Printing.PrinterName;
                    //Hint: setting PrinterName, does not change the systems default
                    if (!_printer.IsValid)
                    {
                        Logger.Error("The selected printer (" + Profile.Printing.PrinterName + ") is invalid!");
                        throw new Exception("101");
                    }
                    markstring.Append("/OutputFile (\\\\\\\\spool\\\\" + _printer.PrinterName.Replace("\\", "\\\\") +
                                      ") ");
                    break;

                case SelectPrinter.ShowDialog:
                default:
                    //add nothing to trigger the Windows-Printing-Dialog
                    break;
            }
            markstring.Append("/UserSettings ");
            markstring.Append("1 dict ");
            markstring.Append("dup /DocumentName (" + Path.GetFileName(Job.OutputFiles[0]) + ") put ");
            markstring.Append("(mswinpr2) finddevice ");
            markstring.Append("putdeviceprops ");
            markstring.Append("setdevice");
            parameters.Add(markstring.ToString());

            //No duplex settings for PrinterDialog
            if (Profile.Printing.SelectPrinter == SelectPrinter.ShowDialog)
                return;

            switch (Profile.Printing.Duplex)
            {
                case DuplexPrint.LongEdge: //Book
                    if (_printer.CanDuplex)
                    {
                        parameters.Add("<< /Duplex true /Tumble false >> setpagedevice ");
                    }
                    break;
                case DuplexPrint.ShortEdge: //Calendar
                    if (_printer.CanDuplex)
                    {
                        parameters.Add("<< /Duplex true /Tumble true >> setpagedevice ");
                    }
                    break;
                case DuplexPrint.Disable:
                default:
                    //Nothing
                    break;
            }
        }

        protected override void SetDeviceSpecificOutputFile(IList<string> parameters)
        {
            //nothing here
            //The OutputFile (respectively printer) gets set in the specific mark section  
        }
    }
}