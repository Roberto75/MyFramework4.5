using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using NUnit.Framework;
using pdfforge.DataStorage;
using pdfforge.DynamicTranslator;
using pdfforge.PDFCreator.Core.Settings.Enums;
using pdfforge.DataStorage.Storage;
using pdfforge.PDFCreator.Core.Settings;
using pdfforge.PDFCreator.Helper;
using pdfforge.PDFCreator.Shared.Helper;
using pdfforge.PDFCreator.Shared.Views;
using pdfforge.PDFCreator.Shared.Views.ActionControls;
using pdfforge.PDFCreator.Views;
using pdfforge.PDFCreator.Views.ActionControls;

namespace PDFCreator.Test
{
    [TestFixture]
    class TranslationTest
    {
        string _languagePath;
        List<Language> _translations;
        PdfCreatorSettings _settings;

        [SetUp]
        public void SetUp()
        {
            Assembly a = Assembly.GetExecutingAssembly();
            string appDir = Path.GetDirectoryName(a.CodeBase.Replace(@"file:///", ""));

            if (appDir == null)
                throw new InvalidDataException("The app dir may never be null");

            _languagePath = FindTranslationFolder();

            Assert.True(Directory.Exists(_languagePath), "Could not find language path: " + _languagePath);

            _translations = Translator.FindTranslations(_languagePath);

            _settings = new PdfCreatorSettings(new IniStorage());
            _settings.LoadData("settings.ini");

            TranslationHelper.Instance.TranslationPath = _languagePath;
            TranslationHelper.Instance.InitTranslator(appDir, _settings.ApplicationSettings.Language);
        }

        private static string FindTranslationFolder()
        {
            var candidates = new[] { @"..\..\..\PDFCreator\Languages", @"..\PDFCreator\Languages", @"PDFCreator\Languages", @"Languages" };

            foreach (string dir in candidates)
            {
                Console.Write(@"Translation folder candidate: {0}: ", dir);
                if (File.Exists(Path.Combine(dir, "english.ini")))
                {
                    Console.WriteLine(@"OK");
                    return dir;
                }

                Console.WriteLine(@"FAIL");
            }

            throw new IOException("Could not find test file folder");
        }

        [Test]
        [RequiresSTA]
        public void AllFormsAndWindows_WhenTranslatedWithAllLanguages_DoNotThrowExceptions()
        {
            foreach (Language lang in _translations)
            {
                TestLanguage(lang);
            }
        }

        private void TestLanguage(Language lang)
        {
            Data translationData = Data.CreateDataStorage();
            IniStorage iniStorage = new IniStorage();
            iniStorage.SetData(translationData);
            iniStorage.ReadData(Path.Combine(_languagePath, lang.FileName), true);

            Translator trans = new BasicTranslator(lang.CommonName, translationData);

            trans.TranslationError += trans_TranslationError;

            // Do some work to include all resources as we do not have our WPF app context here
            EnsureApplicationResources();

            var args = new Dictionary<Type, object[]>();

            // Everything with the type "Window" is tested automatically. If special parameters are needed for a type, they can be declared here
            args.Add(typeof(EditEmailTextWindow), new object[] { false });
            args.Add(typeof(EncryptionPasswordsWindow), new object[] {EncryptionPasswordMiddleButton.Remove, true, true});
            args.Add(typeof(FtpPasswordWindow), new object[] { FtpPasswordMiddleButton.Remove });
            args.Add(typeof(MessageWindow), new object[] { "", "", MessageWindowButtons.YesLaterNo, MessageWindowIcon.PDFCreator });
            args.Add(typeof(ProfileSettingsWindow), new object[] { new PdfCreatorSettings(new IniStorage()) });
            args.Add(typeof(RecommendPdfArchitectWindow), new object[] { false });
            args.Add(typeof(SignaturePasswordWindow), new object[] { PasswordMiddleButton.Skip, null });
            args.Add(typeof(SmtpPasswordWindow), new object[] { SmtpPasswordMiddleButton.Remove });
            args.Add(typeof(UpdateDownloadWindow), new object[] { null });

            TestWindows(trans, lang, args);
            TestActionControls(trans, lang);

            Assert.IsEmpty(trans.TranslationErrors, "There were errors while translating the forms");
        }

        private void TestActionControls(Translator trans, Language lang)
        {
            var type = typeof(UserControl);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => type.IsAssignableFrom(t) && t.Namespace.StartsWith("pdfforge")).ToList();

            AttachmentActionControl x = new AttachmentActionControl();
            UserControl y = x;

            foreach (var t in types)
            {
                try
                {
                    if (!t.IsAbstract)
                    {
                        var userControl = (UserControl) Activator.CreateInstance(t);
                        trans.Translate(userControl);
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Error while translating " + t.Name + " with " + lang.FileName, ex);
                }
            }
        }

        private void TestWindows(Translator trans, Language lang, Dictionary<Type, object[]> argsDictionary)
        {
            var type = typeof(Window);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => type.IsAssignableFrom(t) && t.Namespace.StartsWith("pdfforge")).ToList();

            foreach (var t in types)
            {
                try
                {
                    if (!t.IsAbstract)
                    {
                        Window window;

                        if (argsDictionary.ContainsKey(t))
                        {
                            window = (Window) Activator.CreateInstance(t, argsDictionary[t]);
                        }
                        else
                        {
                            window = (Window) Activator.CreateInstance(t);
                        }
                        trans.Translate(window);
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Error while translating " + t.Name + " with " + lang.FileName, ex);
                }
            }
        }

        void trans_TranslationError(object sender, TranslationErrorEventArgs e)
        {
            throw new ArgumentException(String.Format("Error while translating {0} in {1} (Language: {2})", e.Item, e.Description, e.Language));
        }

        private static void EnsureApplicationResources()
        {
            if (System.Windows.Application.Current == null)
            {
                // create the Application object
                var app = new System.Windows.Application();

                // merge in your application resources
                app.Resources.MergedDictionaries.Add(
                    System.Windows.Application.LoadComponent(
                        new Uri("PDFCreator;component/Resources/AllResources.xaml",
                        UriKind.Relative)) as ResourceDictionary);
            }
        }

        [Test]
        public void TestEnumTranslationsInEnglish()
        {
            Language lang = _translations.FirstOrDefault(l => l.CommonName.Equals("English"));
            Assert.IsNotNull(lang, "Could not load english translation");
            Translator trans = new BasicTranslator(Path.Combine(_languagePath, lang.FileName));
            
            var enumsWithoutTranslation = new List<Type>();
            enumsWithoutTranslation.Add(typeof(ApiProvider));
            enumsWithoutTranslation.Add(typeof(EncryptionLevel)); ////Associated radio buttons get translated manually
            enumsWithoutTranslation.Add(typeof(LoggingLevel));

            EnsureApplicationResources();

            //Get assembly which defines the settings
            var assembly = Assembly.GetAssembly(typeof (PdfCreatorSettings));
            //Get all enumTypes in "pdfforge.PDFCreator.Core.Settings.Enums"
            var enumTypes = assembly.GetTypes().Where(t => String.Equals(t.Namespace, "pdfforge.PDFCreator.Core.Settings.Enums", StringComparison.Ordinal));
            //Remove all enumtypes without translation
            enumTypes = enumTypes.Where(e => !enumsWithoutTranslation.Contains(e));

            foreach (var e in enumTypes)
            {
                var values = Enum.GetValues(e);
                foreach (var v in values)
                {
                    string enumValue = e.Name +"."+ v;
                    Assert.IsNotNullOrEmpty(trans.GetTranslation("Enums", enumValue), enumValue + " has no translation in " + lang.CommonName);
                }
            }
        }
    }
}