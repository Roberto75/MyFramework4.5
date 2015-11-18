using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pdfforge.PDFCreator.Mail
{
    public interface IEmailClientFactory
    {
        IEmailClient CreateEmailClient();
    }
}
