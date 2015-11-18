using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using pdfforge.PDFCreator.Helper;
using pdfforge.PDFCreator.Shared.Helper;

namespace pdfforge.PDFCreator.Views
{
    public partial class WelcomeWindow : Window
    {
        public WelcomeWindow()
        {
            InitializeComponent();
        }

        private void FacebookButton_OnClick(object sender, RoutedEventArgs e)
        {
            ShowUrlInBrowser(Urls.Facebook);
        }

        private void WhatsNewButton_OnClick(object sender, RoutedEventArgs e)
        {
            UserGuideHelper.ShowHelp(null, HelpTopic.WhatsNew);
        }

        private void GooglePlusButton_OnClick(object sender, RoutedEventArgs e)
        {
            ShowUrlInBrowser(Urls.GooglePlus);
        }

        private void ShowUrlInBrowser(string url)
        {
            try
            {
                System.Diagnostics.Process.Start(url);
            }
            catch (Win32Exception)
            {
            }
            catch (FileNotFoundException)
            {
            }
        }

        public static void ShowTopMost()
        {
            var welcomeWindow = new WelcomeWindow();
            TopMostHelper.ShowTopMost(welcomeWindow, true);
        }

        public static void ShowDialogTopMost()
        {
            var welcomeWindow = new WelcomeWindow();
            TopMostHelper.ShowDialogTopMost(welcomeWindow, true);
        }

        private void WelcomeWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            TranslationHelper.Instance.TranslatorInstance.Translate(this);
        }
    }
}
