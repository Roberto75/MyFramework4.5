﻿using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using pdfforge.PDFCreator.Shared.Helper;
using pdfforge.PDFCreator.Shared.ViewModels;

namespace pdfforge.PDFCreator.Shared.Views
{
    public partial class MessageWindow : Window
    {
        public MessageWindowResponse DialogResponse 
        {
            get { return _messageWindowVM.Response; }
        }

        private readonly MessageWindowViewModel _messageWindowVM;

        public MessageWindow(string message, string caption, MessageWindowButtons buttons, MessageWindowIcon icon)
        {
            InitializeComponent();
            MessageText.Text = message;
            Title = caption;
            SetButtons(buttons);
            SetIcon(icon);
            _messageWindowVM = new MessageWindowViewModel(buttons);
            _messageWindowVM.CloseViewAction = delegate(bool? result) { DialogResult = result; };
            DataContext = _messageWindowVM;
        }
        
        public static MessageWindowResponse ShowTopMost(string message, string caption, MessageWindowButtons buttons, MessageWindowIcon icon)
        {
            var messageWindow = new MessageWindow(message, caption, buttons, icon);
            TopMostHelper.ShowDialogTopMost(messageWindow, false);
            return messageWindow.DialogResponse;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            TranslationHelper.Instance.TranslatorInstance.Translate(this);
        }

        private void SetButtons(MessageWindowButtons buttons)
        {
            switch (buttons)
            {
                case MessageWindowButtons.OK:
                    RightButton.Visibility = Visibility.Visible;
                    RightButton.Content = TranslationHelper.Instance.GetTranslation("MessageWindow", "Ok", "OK");
                    RightButton.IsDefault = true;
                    break;
                case MessageWindowButtons.OKCancel:
                    LeftButton.Visibility = Visibility.Visible;
                    LeftButton.Content = TranslationHelper.Instance.GetTranslation("MessageWindow", "Ok", "OK");
                    LeftButton.IsDefault = true;

                    MiddleButton.Visibility = Visibility.Collapsed;
                    
                    RightButton.Visibility = Visibility.Visible;
                    RightButton.Content = TranslationHelper.Instance.GetTranslation("MessageWindow", "Cancel", "Cancel");
                    RightButton.Tag = MessageWindowResponse.Cancel;
                    break;
                case MessageWindowButtons.RetryCancel:
                    LeftButton.Visibility = Visibility.Visible;
                    LeftButton.Content = TranslationHelper.Instance.GetTranslation("MessageWindow", "Retry", "Retry");
                    LeftButton.IsDefault = true;

                    MiddleButton.Visibility = Visibility.Collapsed;
                    
                    RightButton.Visibility = Visibility.Visible;
                    RightButton.Content = TranslationHelper.Instance.GetTranslation("MessageWindow", "Cancel", "Cancel");
                    RightButton.Tag = MessageWindowResponse.Cancel;
                    break;
                case MessageWindowButtons.YesLaterNo:
                    LeftButton.Visibility = Visibility.Visible;
                    LeftButton.Content = TranslationHelper.Instance.GetTranslation("MessageWindow", "Yes", "Yes");
                    LeftButton.IsDefault = true;

                    MiddleButton.Visibility = Visibility.Visible;
                    MiddleButton.Content = TranslationHelper.Instance.GetTranslation("MessageWindow", "Later", "Remind me");

                    RightButton.Visibility = Visibility.Visible;
                    RightButton.Content = TranslationHelper.Instance.GetTranslation("MessageWindow", "No", "No");
                    break;
                case MessageWindowButtons.YesNo:
                    LeftButton.Visibility = Visibility.Visible;
                    LeftButton.Content = TranslationHelper.Instance.GetTranslation("MessageWindow", "Yes", "Yes");
                    LeftButton.IsDefault = true;

                    MiddleButton.Visibility = Visibility.Collapsed;
                    
                    RightButton.Visibility = Visibility.Visible;
                    RightButton.Content = TranslationHelper.Instance.GetTranslation("MessageWindow", "No", "No");
                    break;
            }
        }

        private void SetIcon(MessageWindowIcon icon)
        {
            IconBox.Visibility = Visibility.Visible;
            IconBox.Width = 32;
            IconBox.Height = 32;
            var img = new System.Windows.Controls.Image();
            
            switch (icon)
            {
                case MessageWindowIcon.Error:
                    img.Source = ConvertBitmap(SystemIcons.Error.ToBitmap());
                    System.Media.SystemSounds.Hand.Play();
                    IconBox.Content = img;
                    break;
                case MessageWindowIcon.Exclamation:
                    img.Source = ConvertBitmap(SystemIcons.Exclamation.ToBitmap());
                    System.Media.SystemSounds.Exclamation.Play();
                    IconBox.Content = img;
                    break;
                case MessageWindowIcon.Info:
                    img.Source = ConvertBitmap(SystemIcons.Information.ToBitmap());
                    IconBox.Content = img;
                    System.Media.SystemSounds.Asterisk.Play();
                    break;
                case MessageWindowIcon.Question:
                    img.Source = ConvertBitmap(SystemIcons.Question.ToBitmap());
                    IconBox.Content = img;
                    System.Media.SystemSounds.Question.Play();
                    break;
                case MessageWindowIcon.Warning:
                    img.Source = ConvertBitmap(SystemIcons.Warning.ToBitmap());
                    IconBox.Content = img;
                    System.Media.SystemSounds.Exclamation.Play();
                    break;
                case MessageWindowIcon.PDFCreator:
                    IconBox.Width = 45;
                    IconBox.Height = 45;
                    IconBox.Content = FindResource("PDFCreatorLogo");
                    break;
                case MessageWindowIcon.PDFForge:
                    IconBox.Width = 45;
                    IconBox.Height = 45;
                    IconBox.Content = FindResource("RedFlame");
                    break;
                case MessageWindowIcon.None:
                    IconBox.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private BitmapImage ConvertBitmap(Bitmap value)
        {
            var ms = new MemoryStream();
            value.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            var image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            return image;
        }

        private void CommandBinding_CopyExecuted(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            Clipboard.SetText(MessageText.Text);
        }
    }

    public enum MessageWindowButtons
    {
        OK,
        OKCancel,
        RetryCancel,
        YesNo,
        YesLaterNo,
    }

    public enum MessageWindowIcon
    {
        PDFCreator,
        PDFForge,
        Warning,
        Error,
        Exclamation,
        Question,
        Info,
        None
    }
}
