﻿using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using pdfforge.PDFCreator.Core.Actions;
using pdfforge.PDFCreator.Shared.Helper;
using pdfforge.PDFCreator.Utilities.Tokens;

namespace pdfforge.PDFCreator.Shared.Views.ActionControls
{
    public partial class ScriptActionControl : ActionControl
    {
        private readonly TokenReplacer _tokenReplacer;
        public List<string> Tokens { get; private set; }

        public ScriptActionControl()
        {
            _tokenReplacer = TokenHelper.TokenReplacerWithPlaceHolders;
            Tokens = new List<string>(_tokenReplacer.GetTokenNames(true));

            InitializeComponent();

            DisplayName = TranslationHelper.Instance.GetTranslation("ScriptActionSettings", "DisplayName", "Run script");
            Description = TranslationHelper.Instance.GetTranslation("ScriptActionSettings", "Description", "The script action runs a custom script or application after the conversion. This allows you to further process the output.");

            TranslationHelper.Instance.TranslatorInstance.Translate(this);
        }

        public override bool IsActionEnabled
        {
            get
            {
                if (CurrentProfile == null)
                    return false;
                return CurrentProfile.Scripting.Enabled;
            }
            set { CurrentProfile.Scripting.Enabled = value; }
        }


        private void BrowseScriptButton_OnClick(object sender, RoutedEventArgs e)
        {
            var title = TranslationHelper.Instance.GetTranslation("ScriptActionSettings", "SelectScriptTitle", "Select script file");
            var filter = TranslationHelper.Instance.GetTranslation("ScriptActionSettings", "ExecutableFiles", "Executable files")
                + @" (*.exe, *.bat, *.cmd)|*.exe;*.bat;*.cmd|"
                + TranslationHelper.Instance.GetTranslation("ScriptActionSettings", "AllFiles", "All files")
                + @"(*.*)|*.*";
            
            FileDialogHelper.ShowSelectFileDialog(ScriptFileTextBox, title, filter);
        }

        private void ScriptFileTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ScriptCallPreviewTextBox.Text = ComposeSampleCommand(ScriptFileTextBox.Text, AdditionalParametersTextBox.Text);
        }

        private string ComposeSampleCommand(string scriptPath, string additionalParams)
        {
            if ((string.IsNullOrEmpty(scriptPath)) || (scriptPath.Trim().Length == 0))
                return "";

            //TokenReplacer tokenReplacer = TokenHelper.TokenReplacerWithPlaceHolders;

            string scriptCall = Path.GetFileName(ScriptAction.ComposeScriptPath(scriptPath, _tokenReplacer));

            if (!string.IsNullOrEmpty(additionalParams))
            {
                scriptCall += " " + ScriptAction.ComposeScriptParameters(additionalParams, new[] { @"C:\File1.pdf", @"C:\File2.pdf" }, _tokenReplacer);
            }
            else
            {
                scriptCall += @" C:\File1.pdf C:\File2.pdf";
            }

            return scriptCall;
        }

        private void AddParameterTokenComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TextBoxHelper.InsertText(AdditionalParametersTextBox, AddParameterTokenComboBox.Items[AddParameterTokenComboBox.SelectedIndex] as string);
        }

        private void AddScriptTokenComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TextBoxHelper.InsertText(ScriptFileTextBox, AddScriptTokenComboBox.Items[AddScriptTokenComboBox.SelectedIndex] as string);
        }
    }
}
