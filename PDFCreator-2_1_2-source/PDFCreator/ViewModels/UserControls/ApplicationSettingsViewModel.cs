using System;
using pdfforge.PDFCreator.Core.Settings;
using pdfforge.PDFCreator.Shared.ViewModels;

namespace pdfforge.PDFCreator.ViewModels.UserControls
{
    public class ApplicationSettingsViewModel : ViewModelBase
    {
        private ApplicationSettings _applicationSettings;
        public EventHandler SettingsChanged;

        public ApplicationSettings ApplicationSettings
        {
            get { return _applicationSettings; }
            set
            {
                _applicationSettings = value;
                OnSettingsChanged(new EventArgs());
            }
        }

        private void OnSettingsChanged(EventArgs e)
        {
            RaisePropertyChanged("ApplicationSettings");
            var handler = SettingsChanged;
            if (handler != null) handler(this, e);
        }
    }
}