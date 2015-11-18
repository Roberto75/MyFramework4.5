using System.Windows.Controls;
using pdfforge.PDFCreator.Shared.Helper;
using pdfforge.PDFCreator.ViewModels.UserControls;

namespace pdfforge.PDFCreator.Views.UserControls
{
    public partial class TitleTab : UserControl
    {
        private static readonly TranslationHelper TranslationHelper = TranslationHelper.Instance;

        public TitleTab()
        {
            InitializeComponent();
            if (TranslationHelper.IsInitialized)
            {
                TranslationHelper.TranslatorInstance.Translate(this);
            }
        }

        public TitleTabViewModel ViewModel
        {
            get { return (TitleTabViewModel) DataContext; }
        }

        private void DataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
