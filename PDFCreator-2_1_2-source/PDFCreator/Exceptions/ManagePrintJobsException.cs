﻿using System;

namespace pdfforge.PDFCreator.Exceptions
{
    /// <summary>
    /// The sole purpose of the exception is to signal that the user wants to manage the print jobs and that the processing shall be suspended during that time
    /// </summary>
    class ManagePrintJobsException : Exception
    {
    }
}
