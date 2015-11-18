﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using pdfforge.PDFCreator.Core.Actions;
using pdfforge.PDFCreator.Helper;
using pdfforge.PDFCreator.Shared.Helper;

namespace pdfforge.PDFCreator.Views
{
    public partial class DefectiveProfilesWindow : Window
    {
        public DefectiveProfilesWindow()
        {
            InitializeComponent();
        }

        private DefectiveProfilesWindow(ActionResultDict actionResultDict)
            : this()
        {
            var errors = new List<ProfileError>();

            TranslationHelper.Instance.TranslatorInstance.Translate(this);

            if (actionResultDict.Count() > 1)
                DefectiveProfilesText.Text = TranslationHelper.Instance.GetTranslation(
                    "DefectiveProfilesWindow", "DefectiveProfiles", "Defective profiles:");
            else
                DefectiveProfilesText.Text = TranslationHelper.Instance.GetTranslation(
                    "DefectiveProfilesWindow", "DefectiveProfile", "Defective profile:");

            foreach (var profileNameActionResult in actionResultDict)
            {
                foreach (int error in profileNameActionResult.Value)
                {
                    string errorText = ErrorCodeInterpreter.GetErrorText(error, false);
                    errors.Add(new ProfileError(profileNameActionResult.Key, errorText));
                }
            }

            ProfileList.ItemsSource = errors;

            var view = (CollectionView)CollectionViewSource.GetDefaultView(ProfileList.ItemsSource);
            var groupDescription = new PropertyGroupDescription("Profile");
            view.GroupDescriptions.Add(groupDescription);
        }

        public static bool ShowDialogTopMost(ActionResultDict actionResultDict)
        {
            var window = new DefectiveProfilesWindow(actionResultDict);

            return TopMostHelper.ShowDialogTopMost(window, true) == true;
        }

        private void IgnoreButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CommandBinding_CopyExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            IList errorList;
            if (ProfileList.SelectedItems.Count > 0)
                errorList = ProfileList.SelectedItems;
            else
                errorList = ProfileList.Items;


            var text = new StringBuilder();
            string previousProfile = "";
            foreach (ProfileError profileError in errorList)
            {
                if (previousProfile != profileError.Profile)
                {
                    text.AppendLine(profileError.Profile);
                    previousProfile = profileError.Profile;
                }

                text.AppendLine("- " + profileError.Error);
            }

            Clipboard.SetText(text.ToString());
        }
    }

    class ProfileError
    {
        public ProfileError(string profile, string error)
        {
            Profile = profile;
            Error = error;
        }

        public string Error { get; set; }
        public string Profile    { get; set; }
    }
}
