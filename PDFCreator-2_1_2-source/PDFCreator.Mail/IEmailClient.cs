using System;
using System.Linq;
using System.Text;

namespace pdfforge.PDFCreator.Mail
{
    public interface IEmailClient
    {
        bool ShowEmailClient(Email email);
        bool IsClientInstalled { get; }
    }
}
