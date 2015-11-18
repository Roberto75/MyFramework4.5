using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using NUnit.Framework;
using pdfforge.PDFCreator.Core.Settings;
using pdfforge.PDFCreator.ViewModels.UserControls;
using PDFCreator.Test.ViewModels.Helper;
using Rhino.Mocks;

namespace PDFCreator.Test.ViewModels.UserControls
{
    [TestFixture]
    internal class PrinterTabViewModelTest
    {
        private const string ExistingPrinter = "PDFCreator";
        private const string UnknownPrinter = "UnknownPrinter";
        private ApplicationSettings _applicationSettings;
        private ConversionProfile _profile1;
        private ConversionProfile _profile2;

        [SetUp]
        public void SetUp()
        {
            _profile1 = new ConversionProfile
            {
                Name = "Profile1",
                Guid = "Profile1Guid"
            };

            _profile2 = new ConversionProfile
            {
                Name = "Profile1",
                Guid = "Profile1Guid"
            };

            _applicationSettings = new ApplicationSettings();
            _applicationSettings.PrinterMappings = new[]
            {new PrinterMapping(ExistingPrinter, _profile1.Guid), new PrinterMapping(UnknownPrinter, _profile2.Guid)};
        }

        [Test]
        public void EmptyViewModel_SettingPdfCreatorPrinters_RaisesPropertyChanged()
        {
            var eventStub = MockRepository.GenerateStub<IEventHandler<PropertyChangedEventArgs>>();
            var asViewModel = new PrinterTabViewModel();
            asViewModel.PropertyChanged += eventStub.OnEventRaised;
            var propertyListener = new PropertyChangedListenerMock(asViewModel, "PdfCreatorPrinters");

            asViewModel.PdfCreatorPrinters = new List<string>();

            Assert.IsTrue(propertyListener.WasCalled);
        }

        [Test]
        public void RenamePrinterCommand_WithNonexistingPrinter_NotExecutable()
        {
            var asViewModel = new PrinterTabViewModel(_applicationSettings, new List<ConversionProfile>(),
                () => new[] {ExistingPrinter, "Something"});

            var view = CollectionViewSource.GetDefaultView(asViewModel.PrinterMappings);
            view.MoveCurrentToLast();

            Assert.IsFalse(asViewModel.RenamePrinterCommand.CanExecute(null));
        }

        [Test]
        public void RenamePrinterCommand_WithValidPrinter_IsExecutable()
        {
            var asViewModel = new PrinterTabViewModel(_applicationSettings, new List<ConversionProfile>(),
                () => new[] {ExistingPrinter, "Something"});

            var view = CollectionViewSource.GetDefaultView(asViewModel.PrinterMappings);
            view.MoveCurrentToFirst();

            Assert.IsTrue(asViewModel.RenamePrinterCommand.CanExecute(null));
        }

        [Test]
        public void DeletePrinterCommand_WithNonexistingPrinter_NotExecutable()
        {
            var asViewModel = new PrinterTabViewModel(_applicationSettings, new List<ConversionProfile>(),
                () => new[] {ExistingPrinter, "Something"});

            var view = CollectionViewSource.GetDefaultView(asViewModel.PrinterMappings);
            view.MoveCurrentToLast();

            Assert.IsFalse(asViewModel.DeletePrinterCommand.CanExecute(null));
        }

        [Test]
        public void DeletePrinterCommand_WithValidPrinter_IsExecutable()
        {
            var asViewModel = new PrinterTabViewModel(_applicationSettings, new List<ConversionProfile>(),
                () => new[] {ExistingPrinter, "Something"});

            var view = CollectionViewSource.GetDefaultView(asViewModel.PrinterMappings);
            view.MoveCurrentToFirst();

            Assert.IsTrue(asViewModel.DeletePrinterCommand.CanExecute(null));
        }

        [Test]
        public void EmptyViewModel_WithEmptyProfileList_ContainsLastUsedMappingProfile()
        {
            var asViewModel = new PrinterTabViewModel(new ApplicationSettings(), new ConversionProfile[] {},
                () => new string[] {});

            Assert.AreEqual("", asViewModel.PrinterMappingProfiles.First().Guid);
        }
    }
}