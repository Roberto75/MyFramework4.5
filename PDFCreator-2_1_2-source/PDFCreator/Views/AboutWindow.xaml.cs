using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using pdfforge.PDFCreator.Helper;
using pdfforge.PDFCreator.Shared.Helper;

namespace pdfforge.PDFCreator.Views
{
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            ApplyCustomization();
        }

        private void ApplyCustomization()
        {
            if (Properties.Licensing.ApplyLicensing.Equals("true", StringComparison.CurrentCultureIgnoreCase))
            {
                CustomizationPanel.Visibility = Visibility.Visible;
                CustomText.Text = Properties.Licensing.AboutDialog;

                try
                {
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri("pack://application:,,,/PDFCreator;component/Resources/customlogo.png");
                    logo.EndInit();

                    CustomImage.Source = logo;
                }
                catch (Exception)
                {
                    CustomImage.Source = null;
                }
            }
            else
            {
                CustomizationPanel.Visibility = Visibility.Hidden;
            }
        }

        private void ManualButton_OnClick(object sender, RoutedEventArgs e)
        {
            UserGuideHelper.ShowHelp(null, HelpTopic.General);
        }

        private void LicenseButton_OnClick(object sender, RoutedEventArgs e)
        {
            UserGuideHelper.ShowHelp(null, HelpTopic.License);
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

        private void DonateButton_OnClick(object sender, RoutedEventArgs e)
        {
            ShowUrlInBrowser(Urls.DonateUrl);
        }

        private void CompanyButton_OnClick(object sender, RoutedEventArgs e)
        {
            ShowUrlInBrowser(Urls.PdfforgeWebsiteUrl);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            TranslationHelper.Instance.TranslatorInstance.Translate(this);

            VersionText.Text = "PDFCreator " + VersionHelper.GetCurrentApplicationVersion_WithBuildnumber();
        }

        private void FacebookButton_OnClick(object sender, RoutedEventArgs e)
        {
            ShowUrlInBrowser(Urls.Facebook);
        }

        private void GooglePlusButton_OnClick(object sender, RoutedEventArgs e)
        {
            ShowUrlInBrowser(Urls.GooglePlus);
        }
    }
}
