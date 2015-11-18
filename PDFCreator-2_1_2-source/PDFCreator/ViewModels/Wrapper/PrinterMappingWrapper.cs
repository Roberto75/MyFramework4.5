using System.ComponentModel;
using pdfforge.PDFCreator.Annotations;
using pdfforge.PDFCreator.Core.Settings;
using pdfforge.PDFCreator.Helper;

namespace pdfforge.PDFCreator.ViewModels.Wrapper
{
    public class PrinterMappingWrapper : INotifyPropertyChanged
    {
        private string _printerName;
        private ConversionProfile _conversionProfile;
        private string _primaryPrinter;

        public PrinterMapping PrinterMapping { get; private set; }

        public string PrinterName
        {
            get { return _printerName; }
            set
            {
                _printerName = value;
                PrinterMapping.PrinterName = _printerName;
                OnPropertyChanged("PrinterName");
                OnPropertyChanged("IsPrimaryPrinter");
            }
        }

        public ConversionProfile Profile
        {
            get { return _conversionProfile; }
            set
            {
                _conversionProfile = value;
                PrinterMapping.ProfileGuid = _conversionProfile == null ? "" : _conversionProfile.Guid;
                OnPropertyChanged("Profile");
            }
        }

        public bool IsPrimaryPrinter
        {
            get { return PrinterName != null && PrinterName == PrimaryPrinter; }
        }

        public string PrimaryPrinter
        {
            get { return _primaryPrinter; }
            set
            {
                if (value == _primaryPrinter) return;
                _primaryPrinter = value;
                OnPropertyChanged("PrimaryPrinter");
                OnPropertyChanged("IsPrimaryPrinter");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public PrinterMappingWrapper(PrinterMapping printerMapping)
            : this(printerMapping, SettingsHelper.Settings)
        {
        }

        public PrinterMappingWrapper(PrinterMapping printerMapping, PdfCreatorSettings settings)
        {
            PrinterMapping = printerMapping;
            PrinterName = printerMapping.PrinterName;
            Profile = settings.GetProfileByGuid(printerMapping.ProfileGuid);
        }
    }
}
