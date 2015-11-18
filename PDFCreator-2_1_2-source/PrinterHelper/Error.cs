using System.Collections.Generic;

namespace pdfforge.PrinterHelper
{
    public class Error : IError
    {
        // ReSharper disable InconsistentNaming
        public const int ERROR_INVALID_COMMAND = 1;
        public const int ERROR_INVALID_PARAMETER = 2;
        public const int ERROR_INTERNAL = 3;
        // ReSharper restore InconsistentNaming

        public int Code { get; private set; }
        public string Message { get; private set; }
        public IList<string> Details { get; private set; }

        public Error(int errorCode, string message) : this(errorCode, message, new List<string>())
        {
        }

        public Error(int errorCode, string message, IList<string> errorItemList)
        {
            Code = errorCode;
            Message = message;
            Details = errorItemList;
        }
    }
}