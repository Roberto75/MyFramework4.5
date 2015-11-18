using System;
using SystemInterface.IO;
using SystemWrapper.IO;

namespace pdfforge.PDFCreator.Utilities.IO
{
    /// <summary>
    /// Creates a file path for a file that does not exist yet. It takes a path and appends a counting number (_2, _3, etc) to ensure this in a readable way.
    /// </summary>
    public class UniqueFilename
    {
        private string _filename;

        private readonly IFile _fileWrap = new FileWrap();
        private readonly IPath _pathWrap = new PathWrap();

        public UniqueFilename(string filename)
        {
            if (filename == null)
                throw new ArgumentNullException("filename");

            _filename = filename;
        }

        public UniqueFilename(string filename, IFile fileWrap)
            :this(filename)
        {
            _fileWrap = fileWrap;
        }

        /// <summary>
        /// Creates a file path for a file that does not exist yet. It takes a path and appends a counting number (_2, _3, etc) to ensure this in a readable way.
        /// </summary>
        /// <param name="continueCounting">Set true to continue with the numerical value of the appendix (i.e. _4)</param>
        /// <returns>The uniqified filename</returns>
        public string MakeUniqueFilename(bool continueCounting = false)
        {
            string directory = _pathWrap.GetDirectoryName(_filename) ?? "";
            string fileBody = _pathWrap.GetFileNameWithoutExtension(_filename);
            string extension = _pathWrap.GetExtension(_filename);

            string uniqueFilename = _filename;

            int i = 2;
            if (continueCounting)
            {
                var appendixIndex = fileBody.LastIndexOf("_", StringComparison.InvariantCulture);
                try
                {
                    i = Convert.ToInt32(fileBody.Substring(appendixIndex + 1));
                    fileBody = fileBody.Substring(0, appendixIndex);
                    i++;
                }
                catch (Exception)
                {
                    i = 2;
                }
            }

            while (_fileWrap.Exists(uniqueFilename))
            {
                uniqueFilename = _pathWrap.Combine(directory, fileBody + "_" + i + extension);
                i++;
            }

            return uniqueFilename;
        }
    }
}
