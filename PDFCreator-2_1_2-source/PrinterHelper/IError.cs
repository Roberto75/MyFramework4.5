using System.Collections.Generic;

namespace pdfforge.PrinterHelper
{
    public interface IError
    {
        int Code { get; }
        string Message { get; }
        IList<string> Details { get; }
    }
}