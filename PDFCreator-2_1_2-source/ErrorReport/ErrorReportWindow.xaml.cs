using System;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Navigation;

namespace pdfforge.PDFCreator.ErrorReport
{
    public partial class ErrorReportWindow : Window
    {
        public ErrorReportWindow()
        {
            InitializeComponent();
        }

        public ErrorReportWindow(string errorText, bool allowTerminateApplication)
            : this()
        {
            ErrorDescriptionText.Text = errorText;

            if (!allowTerminateApplication)
                QuitButton.Visibility = Visibility.Hidden;
        }

        private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(e.Uri.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void QuitButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
