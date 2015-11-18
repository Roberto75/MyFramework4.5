
using System.Collections.Generic;
using System.ComponentModel;
using NUnit.Framework;
using pdfforge.DynamicTranslator;
using pdfforge.PDFCreator.ViewModels.UserControls;
using PDFCreator.Test.ViewModels.Helper;
using Rhino.Mocks;

namespace PDFCreator.Test.ViewModels.UserControls
{
    [TestFixture]
    class GeneralTabViewModelTest
    {
        [Test]
        public void EmptyViewModel_SettingLanguages_RaisesPropertyChanged()
        {
            var eventStub = MockRepository.GenerateStub<IEventHandler<PropertyChangedEventArgs>>();
            var generalTabViewModel = new GeneralTabViewModel();
            generalTabViewModel.PropertyChanged += eventStub.OnEventRaised;
            var propertyListener = new PropertyChangedListenerMock(generalTabViewModel, "Languages");

            generalTabViewModel.Languages = new List<Language>();
            Assert.IsTrue(propertyListener.WasCalled);
        }
    }
}
