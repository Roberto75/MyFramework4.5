using System;
using System.IO;
using pdfforge.PDFCreator.Core.Settings.Enums;

namespace pdfforge.PDFCreator.Core
{
    public class OutputFormatHelper
    {
        public static bool HasValidExtension(string file, OutputFormat outputFormat)
        {
            string extension = Path.GetExtension(file);

            if (extension == null)
                return false;

            switch (outputFormat)
            {
                case OutputFormat.Pdf:
                case OutputFormat.PdfA1B:
                case OutputFormat.PdfA2B:
                case OutputFormat.PdfX:
                    return extension.Equals(".pdf", StringComparison.InvariantCultureIgnoreCase);

                case OutputFormat.Jpeg:
                    return extension.Equals(".jpg", StringComparison.InvariantCultureIgnoreCase) || extension.Equals(".jpeg", StringComparison.InvariantCultureIgnoreCase);

                case OutputFormat.Png:
                    return extension.Equals(".png", StringComparison.InvariantCultureIgnoreCase);

                case OutputFormat.Tif:
                    return extension.Equals(".tif", StringComparison.InvariantCultureIgnoreCase) || extension.Equals(".tiff", StringComparison.InvariantCultureIgnoreCase);
            }

            return false;
        }

        public static string AddValidExtension(string file, OutputFormat outputFormat)
        {
            if (HasValidExtension(file, outputFormat))
                return file;

            switch (outputFormat)
            {
                case OutputFormat.Pdf:
                case OutputFormat.PdfA1B:
                case OutputFormat.PdfA2B:
                case OutputFormat.PdfX:
                    return Path.ChangeExtension(file, ".pdf");

                case OutputFormat.Jpeg:
                    return Path.ChangeExtension(file, ".jpg");

                case OutputFormat.Png:
                    return Path.ChangeExtension(file, ".png");

                case OutputFormat.Tif:
                    return Path.ChangeExtension(file, ".tif");
            }

            return file;
        }
    }
}
