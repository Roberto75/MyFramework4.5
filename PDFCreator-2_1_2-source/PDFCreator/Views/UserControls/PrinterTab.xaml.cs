using System.Windows.Controls;
using pdfforge.PDFCreator.Core.Settings;
using pdfforge.PDFCreator.Helper;
using pdfforge.PDFCreator.Shared.Assistants;
using pdfforge.PDFCreator.Shared.Helper;
using pdfforge.PDFCreator.ViewModels.UserControls;
using pdfforge.PDFCreator.ViewModels.Wrapper;

namespace pdfforge.PDFCreator.Views.UserControls
{
    public partial class PrinterTab : UserControl
    {
        private static readonly TranslationHelper TranslationHelper = TranslationHelper.Instance;
        private Shared.Helper.PrinterHelper _printerHelper = new Shared.Helper.PrinterHelper();

        public PrinterTab()
        {
            InitializeComponent();
            if (TranslationHelper.IsInitialized)
            {
                TranslationHelper.TranslatorInstance.Translate(this);
            }
            ViewModel.AddPrinterAction = AddPrinterAction;
            ViewModel.RenamePrinterAction = RenamePrinterAction;
            ViewModel.DeletePrinterAction = DeletePrinterAction;
        }

        public PrinterTabViewModel ViewModel
        {
            get { return (PrinterTabViewModel) DataContext; }
            set { DataContext = value; }
        }

        private void PrimaryPrinterBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel == null)
                return;

            ViewModel.UpdatePrimaryPrinter(ViewModel.ApplicationSettings.PrimaryPrinter);
        }

        private void DataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            e.Cancel = true;
        }

        private void AddPrinterAction()
        {
            var helper = new PrinterActionsAssistant();

            string printerName;
            var success = helper.AddPrinter(out printerName);

            if (success)
            {
                ViewModel.PrinterMappings.Add(
                    new PrinterMappingWrapper(new PrinterMapping(printerName, SettingsHelper.LAST_USED_PROFILE_GUID)));
            }
        }

        private void RenamePrinterAction(PrinterMappingWrapper printerMapping)
        {
            var helper = new PrinterActionsAssistant();
            string newPrinterName;
            var success = helper.RenamePrinter(printerMapping.PrinterName, out newPrinterName);

            if (success)
            {
                ViewModel.PdfCreatorPrinters = _printerHelper.GetPDFCreatorPrinters();
                printerMapping.PrinterName = newPrinterName;
                PrimaryPrinterBox.SelectedValue = ViewModel.PrimaryPrinter;
            }
        }

        private void DeletePrinterAction(PrinterMappingWrapper printerMapping)
        {
            var helper = new PrinterActionsAssistant();
            var success = helper.DeletePrinter(printerMapping.PrinterName, ViewModel.PdfCreatorPrinters.Count);

            if (success)
            {
                ViewModel.PrinterMappings.Remove(printerMapping);
                ViewModel.PdfCreatorPrinters = _printerHelper.GetPDFCreatorPrinters();
                PrimaryPrinterBox.SelectedValue = ViewModel.PrimaryPrinter;
            }
        }

        public void UpdateProfilesList()
        {
            ViewModel.RefreshPrinterMappings();
        }
    }
}