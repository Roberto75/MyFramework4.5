using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using NLog;

namespace pdfforge.PDFCreator.Shared.Helper
{
    public static class UserGuideHelper
    {
        public static string HelpFile { get; set; }

        private static readonly Dictionary<HelpTopic, StringValueAttribute> StringValues = new Dictionary<HelpTopic, StringValueAttribute>();
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void ShowHelp(Control parent, HelpTopic topic)
        {
            string topicText = GetTopic(topic);

            if (topicText == null)
            {
                Logger.Warn("There is no help topic for {0}", topic);
                return;
            }

            ShowHelp(parent, topicText + ".html");
        }

        public static void ShowHelp(HelpTopic topic)
        {
            ShowHelp(null, topic);
        }

        private static void ShowHelp(Control parent, string topicId)
        {
            if (!File.Exists(HelpFile))
                return;

            Help.ShowHelp(parent, HelpFile, HelpNavigator.Topic, topicId);
        }

        public static string GetTopic(HelpTopic value)
        {
            string output = null;
            Type type = value.GetType();

            //Check first in our cached results...
            if (StringValues.ContainsKey(value))
                return StringValues[value].Value;
            
            //Look for our 'StringValueAttribute' 
            //in the field's custom attributes
            FieldInfo fi = type.GetField(value.ToString());
            var attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];

            if (attrs != null && attrs.Length > 0)
            {
                StringValues.Add(value, attrs[0]);
                output = attrs[0].Value;
            }

            return output;
        }
    }

    internal class StringValueAttribute : Attribute
    {

        private readonly string _value;

        public StringValueAttribute(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }

    }

    public enum HelpTopic
    {
        [StringValue("index")]
        General,

        [StringValue("introduction/whats-new")]
        WhatsNew,

        [StringValue("using-pdfcreator/create-a-pdf")]
        CreatingPdf,

        [StringValue("pdfcreator-settings/application-settings/index")]
        AppSettings,

        // Application settings
        [StringValue("pdfcreator-settings/application-settings/general")]
        AppGeneral,

        [StringValue("pdfcreator-settings/application-settings/printers")]
        AppPrinters,

        [StringValue("pdfcreator-settings/application-settings/title")]
        AppTitle,

        [StringValue("pdfcreator-settings/application-settings/api-services")]
        AppApiServices,

        [StringValue("pdfcreator-settings/application-settings/debug")]
        AppDebug,

        // Profile settings
        [StringValue("pdfcreator-settings/profile-settings/index")]
        ProfileSettings,

        [StringValue("pdfcreator-settings/profile-settings/document")]
        ProfileDocument,

        [StringValue("pdfcreator-settings/profile-settings/save")]
        ProfileSave,

        [StringValue("pdfcreator-settings/profile-settings/actions/index")]
        ProfileActions,

        [StringValue("pdfcreator-settings/profile-settings/actions/add-cover")]
        Cover,

        [StringValue("pdfcreator-settings/profile-settings/actions/add-attachment")]
        Attachment,

        [StringValue("pdfcreator-settings/profile-settings/actions/print-document")]
        PrintDocument,

        [StringValue("pdfcreator-settings/profile-settings/actions/open-email-client")]
        OpenEmailClient,

        [StringValue("pdfcreator-settings/profile-settings/actions/send-email-over-smtp")]
        SendEmailOverSmtp,

        [StringValue("pdfcreator-settings/profile-settings/actions/run-script")]
        RunScript,

        [StringValue("pdfcreator-settings/profile-settings/actions/upload-with-ftp")]
        UploadWithFtp,

        [StringValue("pdfcreator-settings/profile-settings/image-formats")]
        ProfileImageFormats,

        [StringValue("pdfcreator-settings/profile-settings/pdf/index")]
        ProfilePdf,

        [StringValue("pdfcreator-settings/profile-settings/pdf/general")]
        PdfGeneral,

        [StringValue("pdfcreator-settings/profile-settings/pdf/compression")]
        PdfCompression,

        [StringValue("pdfcreator-settings/profile-settings/pdf/security")]
        PdfSecurity,

        [StringValue("pdfcreator-settings/profile-settings/pdf/signature")]
        PdfSignature,

        [StringValue("license/index")]
        License,
    }
}
