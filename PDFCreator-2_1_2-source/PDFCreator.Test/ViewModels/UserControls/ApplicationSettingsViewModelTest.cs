using System.ComponentModel;
using NUnit.Framework;
using pdfforge.PDFCreator.Core.Settings;
using pdfforge.PDFCreator.ViewModels.UserControls;
using PDFCreator.Test.ViewModels.Helper;
using Rhino.Mocks;

namespace PDFCreator.Test.ViewModels.UserControls
{
    [TestFixture]
    class ApplicationSettingsViewModelTest
    {
        [Test]
        public void EmptyViewModel_SettingApplicationSettings_RaisesPropertyChanged()
        {
            var eventStub = MockRepository.GenerateStub<IEventHandler<PropertyChangedEventArgs>>();
            var asViewModel = new ApplicationSettingsViewModel();
            asViewModel.PropertyChanged += eventStub.OnEventRaised;
            var propertyListener = new PropertyChangedListenerMock(asViewModel, "ApplicationSettings");

            asViewModel.ApplicationSettings = new ApplicationSettings();

            Assert.IsTrue(propertyListener.WasCalled);
        }

        [Test]
        public void ApplicationSettings_OnSet_TriggersOnSettingsChangeEvent()
        {
            var viewModel = new ApplicationSettingsViewModel();
            var settingsChanged = false;
            viewModel.SettingsChanged += (sender, args) => settingsChanged = true;

            viewModel.ApplicationSettings = new ApplicationSettings();

            Assert.IsTrue(settingsChanged);
        }

        

        

        
    }
}
