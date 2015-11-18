using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using SystemInterface.IO;
using SystemWrapper.IO;
using NLog;
using pdfforge.PDFCreator.Core.Jobs;
using pdfforge.PDFCreator.Core.Settings;
using pdfforge.PDFCreator.Core.Settings.Enums;
using pdfforge.PDFCreator.Utilities;
using pdfforge.PDFCreator.Utilities.IO;

namespace pdfforge.PDFCreator.Core.Ghostscript.OutputDevices
{
    /// <summary>
    ///     The abstract class OutputDevice holds methods and properties that handle the Ghostscript parameters. The device
    ///     independent elements are defined here.
    ///     Other classes inherit OutputDevice to extend the functionality with device-specific functionality, i.e. to create
    ///     PDF or PNG files.
    ///     Especially the abstract function AddDeviceSpecificParameters has to be implemented to add parameters that are
    ///     required to use a given device.
    /// </summary>
    public abstract class OutputDevice
    {
        protected readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IFormatProvider _numberFormat = CultureInfo.InvariantCulture.NumberFormat;

        /// <summary>
        ///     A list of ps files that will be added *before* the first contect ps file will be added
        /// </summary>
        protected IList<string> AdditionalInputFiles = new List<string>();

        /// <summary>
        ///     A list of Distiller dictionary strings. They will be added after all parameters are set.
        /// </summary>
        protected IList<string> DistillerDictonaries = new List<string>();

        /// <summary>
        ///     A list of output files produced during the conversion
        /// </summary>
        public IList<string> TempOutputFiles = new List<string>(); 

        /// <summary>
        ///     The Job that is converted
        /// </summary>
        public IJob Job { get; set; }

        /// <summary>
        ///     The current conversion profile
        /// </summary>
        public ConversionProfile Profile { get; set; }

        protected readonly IFile FileWrap;
        protected readonly IDirectory DirectoryWrap;

        protected OutputDevice()
        {
            FileWrap = new FileWrap();
            DirectoryWrap = new DirectoryWrap();
        }

        protected OutputDevice(IFile file, IDirectory directory)
        {
            FileWrap = file;
            DirectoryWrap = directory;
        }

        protected OutputDevice(IFile file)
            : this()
        {
            FileWrap = file;
        }

        /// <summary>
        ///     Get the list of Ghostscript Parameters. This List contains of a basic set of parameters together with some
        ///     device-specific
        ///     parameters that will be added by the device implementation
        /// </summary>
        /// <param name="ghostscriptVersion"></param>
        /// <returns>A list of parameters that will be passed to Ghostscript</returns>
        public IList<string> GetGhostScriptParameters(GhostscriptVersion ghostscriptVersion)
        {
            IList<string> parameters = new List<string>();

            parameters.Add("gs");
            //parameters.Add("-q");
            parameters.Add("-I" + ghostscriptVersion.LibPaths);
            parameters.Add("-sFONTPATH=" + OsHelper.GetWindowsFontsFolder());

            parameters.Add("-dNOPAUSE");
            parameters.Add("-dBATCH");

            if (!HasValidExtension(Job.OutputFilenameTemplate, Job.Profile.OutputFormat))
                MakeValidExtension(Job.OutputFilenameTemplate, Job.Profile.OutputFormat);

            SetDeviceSpecificOutputFile(parameters);

            AddDeviceSpecificParameters(parameters);

            // Add user-defined parameters
            if (!String.IsNullOrEmpty(Job.Profile.Ghostscript.AdditionalGsParameters))
            {
                string[] args = FileUtil.CommandLineToArgs(Job.Profile.Ghostscript.AdditionalGsParameters);
                foreach (string s in args)
                    parameters.Add(s);
            }

            //Dictonary-Parameters must be the last Parameters
            if (DistillerDictonaries.Count > 0)
            {
                parameters.Add("-c");
                foreach (string parameter in DistillerDictonaries)
                {
                    parameters.Add(parameter);
                }
            }

            //Don't add further paramters here, since the distiller-parameters should be the last!

            parameters.Add("-f");

            foreach (string file in AdditionalInputFiles)
            {
                parameters.Add(file);
            }

            if (Profile.Stamping.Enabled)
            {
                // Compose name of the stamp file based on the location and name of the inf file
                string stampFileName = Path.Combine(Job.JobTempFolder,
                    Path.GetFileNameWithoutExtension(Job.JobInfo.InfFile) + ".stm");
                CreateStampFile(stampFileName, Profile);
                parameters.Add(stampFileName);
            }

            if (Profile.CoverPage.Enabled)
                parameters.Add(Profile.CoverPage.File);

            foreach (SourceFileInfo sfi in Job.JobInfo.SourceFiles)
            {
                parameters.Add(sfi.Filename);
            }

            if (Profile.AttachmentPage.Enabled)
                parameters.Add(Profile.AttachmentPage.File);

            // Compose name of the pdfmark file based on the location and name of the inf file
            string pdfMarkFileName = Path.Combine(Job.JobTempFolder, "metadata.mtd");
            CreatePdfMarksFile(pdfMarkFileName);

            // Add pdfmark file as input file to set metadata
            parameters.Add(pdfMarkFileName);

            foreach (string s in parameters)
            {
                Console.WriteLine(s);
            }
            return parameters;
        }

        /// <summary>
        ///     Create a file with metadata in the pdfmarks format. This file can be passed to Ghostscript to set Metadata of the
        ///     resulting document
        /// </summary>
        /// <param name="filename">Full path and filename of the resulting file</param>
        private void CreatePdfMarksFile(string filename)
        {
            var metadataContent = new StringBuilder();
            metadataContent.Append("/pdfmark where {pop} {userdict /pdfmark /cleartomark load put} ifelse\n[ ");
            metadataContent.Append("\n/Title " + EncodeGhostscriptParametersHex(Job.JobInfo.Metadata.Title));
            metadataContent.Append("\n/Author " + EncodeGhostscriptParametersHex(Job.JobInfo.Metadata.Author));
            metadataContent.Append("\n/Subject " + EncodeGhostscriptParametersHex(Job.JobInfo.Metadata.Subject));
            metadataContent.Append("\n/Keywords " + EncodeGhostscriptParametersHex(Job.JobInfo.Metadata.Keywords));
            metadataContent.Append("\n/Creator " + EncodeGhostscriptParametersHex(Job.JobInfo.Metadata.Producer));
            metadataContent.Append("\n/Producer " + EncodeGhostscriptParametersHex(Job.JobInfo.Metadata.Producer));
            metadataContent.Append("\n/DOCINFO pdfmark");

            AddViewerSettingsToMetadataContent(metadataContent);

            FileWrap.WriteAllText(filename, metadataContent.ToString());
            
            Logger.Debug("Created metadata file \"" + filename + "\"");
        }

        private string RgbToCmykColorString(Color color)
        {
            var red = color.R / 255.0;
            var green = color.G / 255.0;
            var blue = color.B / 255.0;

            var k = Math.Min(1 - red, 1 - green);
            k = Math.Min(k, 1 - blue);
            var c = (1 - red - k)/(1 - k);
            var m = (1 - green - k)/(1 - k);
            var y = (1 - blue - k)/(1 - k);

            return  c.ToString("0.00", _numberFormat) + " " +
                    m.ToString("0.00", _numberFormat) + " " +
                    y.ToString("0.00", _numberFormat) + " " +
                    k.ToString("0.00", _numberFormat);
        }

        private void CreateStampFile(string filename, ConversionProfile profile)
        {
            // Create a resource manager to retrieve resources.
            var rm = new ResourceManager(typeof (CoreResources));

            var stampString = rm.GetString("PostScriptStamp");

            if (stampString == null)
                throw new InvalidOperationException("Error while fetching stamp template");

            int outlineWidth = 0;
            string outlineString = "show";

            if (profile.Stamping.FontAsOutline)
            {
                outlineWidth = profile.Stamping.FontOutlineWidth;
                outlineString = "true charpath stroke";
            }

            // Only Latin1 chars are allowed here
            stampString = stampString.Replace("[STAMPSTRING]",
                EncodeGhostscriptParametersOctal(profile.Stamping.StampText));
            stampString = stampString.Replace("[FONTNAME]", profile.Stamping.PostScriptFontName);
            stampString = stampString.Replace("[FONTSIZE]", profile.Stamping.FontSize.ToString(_numberFormat));
            stampString = stampString.Replace("[STAMPOUTLINEFONTTHICKNESS]", outlineWidth.ToString(CultureInfo.InvariantCulture));
            stampString = stampString.Replace("[USEOUTLINEFONT]", outlineString); // true charpath stroke OR show

            if (profile.OutputFormat == OutputFormat.PdfX ||
                profile.PdfSettings.ColorModel == ColorModel.Cmyk)
            {
                var colorString = RgbToCmykColorString(profile.Stamping.Color);
                stampString = stampString.Replace("[FONTCOLOR]", colorString);
                stampString = stampString.Replace("setrgbcolor", "setcmykcolor");
            }
            else
            {
                var colorString = (profile.Stamping.Color.R / 255.0).ToString("0.00", _numberFormat) + " " +
                                 (profile.Stamping.Color.G / 255.0).ToString("0.00", _numberFormat) + " " +
                                 (profile.Stamping.Color.B / 255.0).ToString("0.00", _numberFormat);
                stampString = stampString.Replace("[FONTCOLOR]", colorString);
            }
            
            FileWrap.WriteAllText(filename, stampString);
        }

        /*private string GetPostscriptFontName(string font)
        {
            return font.Replace(" ", "");
        }*/

        protected string EncodeGhostscriptParametersOctal(string String)
        {
            var sb = new StringBuilder();

            foreach (char c in String)
            {
                switch (c)
                {
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '{':
                        sb.Append("\\{");
                        break;
                    case '}':
                        sb.Append("\\}");
                        break;
                    case '[':
                        sb.Append("\\[");
                        break;
                    case ']':
                        sb.Append("\\]");
                        break;
                    case '(':
                        sb.Append("\\(");
                        break;
                    case ')':
                        sb.Append("\\)");
                        break;

                    default:
                        int charCode = c;
                        if (charCode > 127)
                            sb.Append("\\" + Convert.ToString(Math.Min(charCode, 255), 8));
                        else sb.Append(c);
                        break;
                }
            }

            return sb.ToString();
        }

        protected string EncodeGhostscriptParametersHex(string String)
        {
            if (String == null)
                return "()";

            return "<FEFF" + BitConverter.ToString(Encoding.BigEndianUnicode.GetBytes(String)).Replace("-", "") + ">";
        }

        /// <summary>
        ///     This functions is called by inherited classes to add device-specific parameters to the Ghostscript parameter list
        /// </summary>
        /// <param name="parameters">The current list of parameters. This list may be modified in inherited classes.</param>
        protected abstract void AddDeviceSpecificParameters(IList<string> parameters);

        protected abstract void SetDeviceSpecificOutputFile(IList<string> parameters);

        /// <summary>
        ///     Collect all output files for this device from tempFolder, rename and move them to their destination according to
        ///     the FilenameTemplate and store them in the OutputFiles list.
        ///     By default, this inserts the filename template here. This can be overridden, i.e. to collect multiple images with
        ///     enumerator.
        /// </summary>
        public void CollectTemporaryOutputFiles()
        {
            string[] files = DirectoryWrap.GetFiles(Job.JobTempOutputFolder);

            foreach (var file in files)
            {
                TempOutputFiles.Add(file);
            }
        }

        public virtual void MoveOutputFiles()
        {
            /*
            if (!FileUtil.CheckWritability(Job.OutputFilenameTemplate))
            {   
                throw new DeviceException("Not enough permissions to write in the outfilenameTemplate directory \"" + Job.OutputFilenameTemplate + "\".", 1);
            }
            */
            /*
            var uniqueFileNameContinuance = false;
            
            if (Job.Profile.AutoSave.Enabled && Job.Profile.AutoSave.EnsureUniqueFilenames)
            {
                _logger.Debug("Ensuring unique filename for: " + Job.OutputFilenameTemplate);
                var oldTemplate = Job.OutputFilenameTemplate;
                Job.OutputFilenameTemplate = new UniqueFilename(Job.OutputFilenameTemplate).MakeUniqueFilename();
                uniqueFileNameContinuance = oldTemplate != Job.OutputFilenameTemplate;
                _logger.Debug("Unique filename result: " + Job.OutputFilenameTemplate);
            }
            else
            {
                try
                {
                    File.Delete(Job.OutputFilenameTemplate);
                }
                catch (IOException)
                {
                    string oldFile = Job.OutputFilenameTemplate;
                    Job.OutputFilenameTemplate = new UniqueFilename(Job.OutputFilenameTemplate).MakeUniqueFilename();
                    uniqueFileNameContinuance = true;
                    _logger.Warn("Could not write to output file: {0}, using {1} instead", oldFile,
                        Job.OutputFilenameTemplate);
                }
            }
            _logger.Debug("Moving output file: {0}", Job.OutputFilenameTemplate);

            string outputFile = OutputFiles[0];

            try//quick fix for multithreading
            {//
                if (File.Exists(outputFile))
                {
                    File.Move(outputFile, Job.OutputFilenameTemplate);
                    Job.OutputFiles.Add(Job.OutputFilenameTemplate);
                }
            }//
            catch (Exception)//
            {//
                _logger.Warn("Could not move outputfile to \"" +Job.OutputFilenameTemplate +"\" starting new attempt");//
                MoveFile(outputFile, uniqueFileNameContinuance);//
            }//
            
            */
            var uniqueFileNameContinuance = false;

            if (Job.Profile.AutoSave.Enabled && Job.Profile.AutoSave.EnsureUniqueFilenames)
            {
                var oldTemplate = Job.OutputFilenameTemplate;
                Job.OutputFilenameTemplate = EnsureUniqueFilename(Job.OutputFilenameTemplate, false);
                uniqueFileNameContinuance = oldTemplate != Job.OutputFilenameTemplate;
            }

            if (!CopyFile(TempOutputFiles[0], Job.OutputFilenameTemplate))
            {
                Job.OutputFilenameTemplate = EnsureUniqueFilename(Job.OutputFilenameTemplate, uniqueFileNameContinuance);

                if (!CopyFile(TempOutputFiles[0], Job.OutputFilenameTemplate))
                    //Throw exception affter second attempt to copy failes.
                    throw new DeviceException("Error while copying to target file in second attempt. Process gets canceled.", 2);
            }
            DeleteFile(TempOutputFiles[0]);
            Job.OutputFiles.Add(Job.OutputFilenameTemplate);
        }

        protected bool CopyFile(string tempFile, string outputFile)
        {
            try
            {
                FileWrap.Copy(tempFile, outputFile, true);
                Logger.Debug("Copied output file \"{0}\" \r\nto \"{1}\"", tempFile, outputFile);
                return true;
            }
            catch (IOException ioException)
            {
                Logger.Warn("Error while copying to target file.\r\nfrom\"{0}\" \r\nto \"{1}\"\r\n{2}" , tempFile, outputFile, ioException.Message);
            }
            return false;
        }

        /// <summary>
        /// Ensure unique filename.
        /// </summary>
        /// <param name="outputfile">targeted file path</param>
        /// <param name="uniqueFileNameContinuance">set true to proceed with prior appendix</param>
        /// <returns>unique outputfilename</returns>
        protected string EnsureUniqueFilename(string outputfile, bool uniqueFileNameContinuance)
        {
            Logger.Debug("Starting second attempt by creating unique filename.");
            Logger.Debug("Ensuring unique filename for: " + outputfile);
            outputfile = new UniqueFilename(outputfile, FileWrap).MakeUniqueFilename(uniqueFileNameContinuance);
            Logger.Debug("Unique filename result: " + outputfile);

            return outputfile;
        }

        protected void DeleteFile(string tempfile)
        {
            try
            {
                FileWrap.Delete(tempfile);
            }
            catch (IOException)
            {
                Logger.Warn("Could not delete temporary file \"" + tempfile + "\"");
            }
        }

        private void AddViewerSettingsToMetadataContent(StringBuilder metadataContent)
        {
            metadataContent.Append("\n[\n/PageLayout ");

            switch (Profile.PdfSettings.PageView)
            {
                case PageView.OneColumn:
                    metadataContent.Append("/OneColumn");
                    break;
                case PageView.TwoColumnsOddLeft:
                    metadataContent.Append("/TwoColumnLeft");
                    break;
                case PageView.TwoColumnsOddRight:
                    metadataContent.Append("/TwoColumnRight");
                    break;
                case PageView.TwoPagesOddLeft:
                    metadataContent.Append("/TwoPageLeft");
                    break;
                case PageView.TwoPagesOddRight:
                    metadataContent.Append("/TwoPageRight");
                    break;
                case PageView.OnePage:
                default:
                    metadataContent.Append("/SinglePage");
                    break;
            }

            metadataContent.Append("\n/PageMode ");
            switch (Profile.PdfSettings.DocumentView)
            {
                case DocumentView.AttachmentsPanel:
                    metadataContent.Append("/UseAttachments");
                    break;
                case DocumentView.ContentGroupPanel:
                    metadataContent.Append("/UseOC");
                    break;
                case DocumentView.FullScreen:
                    metadataContent.Append("/FullScreen");
                    break;
                case DocumentView.Outline:
                    metadataContent.Append("/UseOutlines");
                    break;
                case DocumentView.ThumbnailImages:
                    metadataContent.Append("/UseThumbs");
                    break;
                case DocumentView.NoOutLineNoThumbnailImages:
                default:
                    metadataContent.Append("/UseNone");
                    break;
            }

            if (Profile.PdfSettings.ViewerStartsOnPage > Job.PageCount)
                metadataContent.Append(" /Page " + Job.PageCount);
            else if (Profile.PdfSettings.ViewerStartsOnPage <= 0)
                metadataContent.Append(" /Page 1");
            else
                metadataContent.Append(" /Page " + Profile.PdfSettings.ViewerStartsOnPage);

            metadataContent.Append("\n/DOCVIEW pdfmark");
        }

        public string[] GetValidExtensions(OutputFormat format)
        {
            string[] validExtensions;

            switch (format)
            {
                case OutputFormat.Jpeg:
                    validExtensions = new[] { ".jpg", ".jpeg" };
                    break;

                case OutputFormat.Tif:
                    validExtensions = new[] { ".tif", ".tiff" };
                    break;

                case OutputFormat.Pdf:
                case OutputFormat.PdfA1B:
                case OutputFormat.PdfA2B:
                case OutputFormat.PdfX:
                    validExtensions = new[] { ".pdf" };
                    break;

                default: validExtensions = new[] { "." + format.ToString().ToLower() };
                    break;
            }

            return validExtensions;
        }

        public bool HasValidExtension(string filename, OutputFormat format)
        {
            string[] validExtensions = GetValidExtensions(format);
            string ext = Path.GetExtension(filename).ToLower();

            return validExtensions.Contains(ext);
        }

        public string MakeValidExtension(string filename, OutputFormat format)
        {
            string[] validExtensions = GetValidExtensions(format);
            return Path.ChangeExtension(filename, validExtensions[0]);
        }
    }


}