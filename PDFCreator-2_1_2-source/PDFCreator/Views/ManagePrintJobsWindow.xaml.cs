using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using pdfforge.PDFCreator.Assistants;
using pdfforge.PDFCreator.Shared.Helper;
using pdfforge.PDFCreator.ViewModels;

namespace pdfforge.PDFCreator.Views
{
    public partial class ManagePrintJobsWindow : Window
    {
        public ManagePrintJobsWindow()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            TranslationHelper.Instance.TranslatorInstance.Translate(this);
            var view = (GridView) JobList.View;
            view.Columns[0].Header = TranslationHelper.Instance.GetTranslation("ManagePrintJobsWindow", "TitleColoumn", "Title");
            view.Columns[1].Header = TranslationHelper.Instance.GetTranslation("ManagePrintJobsWindow", "FilesColoumn", "Files");
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            if (((string[])e.Data.GetData(DataFormats.FileDrop)).Length == 0)
                e.Effects = DragDropEffects.None;
            else
                e.Effects = DragDropEffects.Copy;
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            var printFile = new PrintFileAssistant();
            if (printFile.AddFiles((string[])e.Data.GetData(DataFormats.FileDrop, false)))
                printFile.PrintAll();
        }

        private void JobList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = (ManagePrintJobsViewModel)DataContext;
            vm.DeleteJobCommand.RaiseCanExecuteChanged();
            vm.MergeJobsCommand.RaiseCanExecuteChanged();
        }

        private void OnActivated(object sender, EventArgs e)
        {
            ((ManagePrintJobsViewModel)DataContext).RaiseRefreshView();
        }

        private void ManagePrintJobsWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }
    }
}
