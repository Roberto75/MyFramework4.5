using System;
using SystemInterface.IO;
using SystemWrapper.IO;

namespace pdfforge.PDFCreator.Utilities.IO
{
    /// <summary>
    /// Creates a directory that does not exist yet. It takes a path and appends a counting number (_2, _3, etc) to ensure this in a readable way.
    /// </summary>
    public class UniqueDirectory
    {
        private string _path;

        private readonly IDirectory _directoryWrap = new DirectoryWrap();
        private readonly IPath _pathWrap = new PathWrap();

        public UniqueDirectory(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            _path = path;
        }

        public UniqueDirectory(string path, IDirectory directoryWrap)
            : this(path)
        {
            _directoryWrap = directoryWrap;
        }

        /// <summary>
        /// Creates a directory that does not exist yet. It takes a path and appends a counting number (_2, _3, etc) to ensure this in a readable way.
        /// </summary>
        /// <returns>The uniqified directory path</returns>
        public string MakeUniqueDirectory()
        {
            string directory = _pathWrap.GetDirectoryName(_path) ?? "";
            string fileBody = _pathWrap.GetFileName(_path);

            int i = 2;

            while (_directoryWrap.Exists(_path))
            {
                _path = _pathWrap.Combine(directory, fileBody + "_" + i);
                i++;
            }

            return _path;
        }
    }
}
