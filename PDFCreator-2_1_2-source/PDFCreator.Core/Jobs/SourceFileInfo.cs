using System;
using System.Globalization;
using System.IO;
using pdfforge.DataStorage;

namespace pdfforge.PDFCreator.Core.Jobs
{
    /// <summary>
    ///     SourceFileInfo holds data stored about a single source file, like name of the input file, printer name etc.
    /// </summary>
    public class SourceFileInfo
    {
        /// <summary>
        ///     The full path of the source file
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        ///     The Windows Session Id
        /// </summary>
        public int SessionId { get; set; }

        /// <summary>
        ///     The window station the job was created on (i.e. Console)
        /// </summary>
        public string WinStation { get; set; }

        /// <summary>
        ///     The Author of the document
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        ///     Name of the computer on which the job was created
        /// </summary>
        public string ClientComputer { get; set; }

        /// <summary>
        ///     Name of the printer
        /// </summary>
        public string PrinterName { get; set; }

        /// <summary>
        ///     pdfcmon job counter
        /// </summary>
        public int JobCounter { get; set; }

        /// <summary>
        ///     ID of the Job as given from Windows printer
        /// </summary>
        public int JobId { get; set; }

        /// <summary>
        ///     The Title of the document
        /// </summary>
        public string DocumentTitle { get; set; }

        /// <summary>
        ///     Read a single SourceFileInfo record from the given data section
        /// </summary>
        /// <param name="infFilename">full path to the inf file to read</param>
        /// <param name="data">Data set to use</param>
        /// <param name="section">Name of the section to process</param>
        /// <returns>A filled SourceFileInfo or null, if the data is invalid (i.e. no filename)</returns>
        internal static SourceFileInfo ReadSourceFileInfo(string infFilename, Data data, string section)
        {
            if (infFilename == null)
                throw new ArgumentNullException("infFilename");

            var sfi = new SourceFileInfo();

            sfi.DocumentTitle = data.GetValue(section + "DocumentTitle");
            sfi.WinStation = data.GetValue(section + "WinStation");
            sfi.Author = data.GetValue(section + "UserName");
            sfi.ClientComputer = data.GetValue(section + "ClientComputer");
            sfi.Filename = data.GetValue(section + "SpoolFileName");

            if (!Path.IsPathRooted(sfi.Filename))
            {
                sfi.Filename = Path.Combine(Path.GetDirectoryName(infFilename) ?? "", sfi.Filename);
            }

            sfi.PrinterName = data.GetValue(section + "PrinterName");

            try
            {
                sfi.SessionId = Int32.Parse(data.GetValue(section + "SessionId"));
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
                sfi.SessionId = 0;
            }

            try
            {
                sfi.JobCounter = Int32.Parse(data.GetValue(section + "JobCounter"));
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
                sfi.JobCounter = 0;
            }

            try
            {
                sfi.JobId = Int32.Parse(data.GetValue(section + "JobId"));
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
                sfi.JobId = 0;
            }

            if (String.IsNullOrEmpty(sfi.Filename))
                return null;

            return sfi;
        }

        internal void WriteSourceFileInfo(Data data, string section)
        {
            data.SetValue(section + "DocumentTitle", DocumentTitle);
            data.SetValue(section + "WinStation", WinStation);
            data.SetValue(section + "UserName", Author);
            data.SetValue(section + "ClientComputer", ClientComputer);
            data.SetValue(section + "SpoolFileName", Filename);
            data.SetValue(section + "PrinterName", PrinterName);
            data.SetValue(section + "SessionId", SessionId.ToString(CultureInfo.InvariantCulture));
            data.SetValue(section + "JobCounter", JobCounter.ToString(CultureInfo.InvariantCulture));
            data.SetValue(section + "JobId", JobId.ToString(CultureInfo.InvariantCulture));
        }
    }
}