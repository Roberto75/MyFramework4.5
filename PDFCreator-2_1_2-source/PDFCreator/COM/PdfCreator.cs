using System;
using System.IO;
using System.Runtime.InteropServices;
using pdfforge.PDFCreator.Assistants;
using pdfforge.PDFCreator.Threading;
using pdfforge.PDFCreator.Utilities.Communication;

namespace pdfforge.PDFCreator.COM
{
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    [Guid("A1F6647E-8C19-4A3E-89DF-7FDFAD2A0C30")]
    public interface IPDFCreator
    {
        Printers GetPDFCreatorPrinters { get; }
        bool IsInstanceRunning { get; }
        void PrintFile(string path);
    }

    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("69189C58-70C4-4DF2-B94D-5D786E9AD513")]
    [ProgId("PDFCreatorBeta.PDFCreatorObj")]
    public class PdfCreatorObj : IPDFCreator
    {
        /// <summary>
        /// Returns a new PrinterDevices object
        /// </summary>
        public Printers GetPDFCreatorPrinters
        {
            get { return new Printers(); }
        }

        
        /// <summary>
        /// Checks if PDFCreator is running
        /// </summary>
        public bool IsInstanceRunning
        {
            get
            {
                return PipeServer.SessionServerInstanceRunning(ThreadManager.PipeName);
            }
        }

        /// <summary>
        /// Prints a file to the PDFCreator printer
        /// </summary>
        /// <param name="path">Path of the file to be printed</param>
        public void PrintFile(string path)
        {
            if(String.IsNullOrEmpty(path))
                throw new COMException("The specified path must not be empty or uninitiliazed.");

            if (!File.Exists(path))
                throw new COMException("File with such a path doesn't exist. Please check if the specified path is correct.");

            var printAssistant = new PrintFileAssistant();
            printAssistant.AddFile(path);
            printAssistant.PrintAll();
        }
    }
}
