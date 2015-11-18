using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using pdfforge.DataStorage;
using pdfforge.DataStorage.Storage;
using pdfforge.DynamicTranslator;
using pdfforge.PDFCreator.Core.Settings;

namespace pdfforge.PDFCreator.Shared.Helper
{
    /// <summary>
    /// TranslationUtil provides functionality that is used in conjunction with the DynamicTransltor classes.
    /// </summary>
    public class TranslationHelper
    {
        private Translator _tmpTranslator;
        private Translator _translator;
        private Translator _nativeLanguageTranslator;

        private string _translationPath;

        private static TranslationHelper _instance;

        public static TranslationHelper Instance
        {
            get { return _instance ?? (_instance = new TranslationHelper()); }
        }


        /// <summary>
        /// The Translator Singleton that will be used throughout the application
        /// </summary>
        public Translator TranslatorInstance
        {
            get
            {
                if (_translator == null)
                    throw new ArgumentException("Translator has not been initialized and thus cannot be used");

                return _translator;
            }
        }

        
        /// <summary>
        /// Path to where the translation files are located. The default takes into account, that there might be different folders to look at, i.e. when run from Visual Studio
        /// </summary>
        public string TranslationPath
        {
            get
            {
                if (_translationPath != null)
                    return _translationPath;

                var translationPathCandidates = new[] { @"Languages", @"..\..\Languages" };

                string appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                if (appDir == null)
                    return "";

                foreach (string pathCandidate in translationPathCandidates)
                {
                    string path = Path.Combine(appDir, pathCandidate);
                    if (Directory.Exists(path) && (File.Exists(Path.Combine(path, "english.ini"))))
                    {
                        _translationPath = path;
                        return _translationPath;
                    }
                }

                return "";
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _translationPath = value;
            }
        }

        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Hiding constructor as there are only static methods
        /// </summary>
        private TranslationHelper()
        {

        }

        /// <summary>
        /// Translate a single string. If the string is not available in the current translation, the default translation will be used
        /// </summary>
        /// <param name="section">section name of the translation file</param>
        /// <param name="item">name of the item to translate</param>
        /// <returns>The translated value if available or the default translation</returns>
        public string GetTranslation(string section, string item)
        {
            string t = _translator.GetTranslation(section, item);
            if (String.IsNullOrEmpty(t))
                t = _nativeLanguageTranslator.GetTranslation(section, item);

            return t;
        }

        /// <summary>
        /// Translate a single string. If the string is not available in the current translation, the default value will be used
        /// </summary>
        /// <param name="section">section name of the translation file</param>
        /// <param name="item">name of the item to translate</param>
        /// <param name="defaultValue">the default translation of this item</param>
        /// <returns>The translated value if available or the default value</returns>
        public string GetTranslation(string section, string item, string defaultValue)
        {
            string t = _translator.GetTranslation(section, item);
            if (String.IsNullOrEmpty(t))
                t = defaultValue;

            return t;
        }

        /// <summary>
        /// Translate a single string that contains formatting. If the string is not available in the current translation, the default translation will be used.
        /// The formatting will be applied in both cases
        /// </summary>
        /// <param name="section">section name of the translation file</param>
        /// <param name="item">name of the item to translate</param>
        /// <param name="args">an array of values to pass to the formatting function</param>
        /// <returns></returns>
        public string GetFormattedTranslation(string section, string item, params object[] args)
        {
            string t = _translator.GetFormattedTranslation(section, item, args);
            if (String.IsNullOrEmpty(t))
                t = _nativeLanguageTranslator.GetFormattedTranslation(section, item, args);

            return t;
        }

        /// <summary>
        /// Translate a single string that contains formatting while providing a default value. If the string is not available in the current translation, the default value will be used.
        /// The formatting will be applied in both cases
        /// </summary>
        /// <param name="section">section name of the translation file</param>
        /// <param name="item">name of the item to translate</param>
        /// <param name="defaultValue">the default translation of this item</param>
        /// <param name="args">an array of values to pass to the formatting function</param>
        /// <returns></returns>
        public string GetFormattedTranslation(string section, string item, string defaultValue, params object[] args)
        {
            string t = _translator.GetFormattedTranslation(section, item, args);
            if (String.IsNullOrEmpty(t))
                t = String.Format(defaultValue, args);

            return t;
        }

        /// <summary>
        /// Determine the translation file to use based on specified language. If the language is invalid, the English file will be used.
        /// </summary>
        /// <param name="language">the language to use</param>
        /// <returns>The path to a translation file or null</returns>
        public string GetTranslationFile(string language)
        {
            string file = GetTranslationFileIfExists(language);

            if (file != null)
                return file;

            return GetTranslationFileIfExists("english");
        }

        /// <summary>
        /// Determine the translation file to use based on the file name.
        /// </summary>
        /// <param name="languageName">Name of the language file</param>
        /// <returns>The path to a translation file or null, if it does not exist</returns>
        public string GetTranslationFileIfExists(string languageName)
        {
            string path = Path.Combine(TranslationPath, languageName + ".ini");

            if (File.Exists(path))
                return path;

            return null;
        }

        /// <summary>
        /// Initialize the Translator for later use in the application
        /// </summary>
        /// <param name="languageName">Language to use for initialization</param>
        public void InitTranslator(string languageName)
        {
            string appDir = "";
            try
            {
                appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
            catch (NullReferenceException) { }
            InitTranslator(appDir, languageName);
        }

        /// <summary>
        /// Initialize the Translator for later use in the application
        /// </summary>
        /// <param name="applicationDir">Application directory where the help files are stored</param>
        /// <param name="languageName">Language to use for initialization</param>
        public void InitTranslator(string applicationDir, string languageName)
        {
            IsInitialized = true;

            string translationFile = GetTranslationFile(languageName);

            _translator = BuildLanguageTranslator(translationFile);
            _nativeLanguageTranslator = BuildLanguageTranslator(Path.Combine(TranslationPath, "english.ini"));

            string helpFile = null;

            if (languageName.Equals("german", StringComparison.InvariantCultureIgnoreCase))
                helpFile = Path.Combine(applicationDir, "PDFCreator_german.chm");
            else if (languageName.Equals("french", StringComparison.InvariantCultureIgnoreCase))
                helpFile = Path.Combine(applicationDir, "PDFCreator_french.chm");

            if ((helpFile != null) && File.Exists(helpFile))
                UserGuideHelper.HelpFile = helpFile;
            else
                UserGuideHelper.HelpFile = Path.Combine(applicationDir, "PDFCreator_english.chm");
        }

        public void InitTranslator(Stream stream, string languageName)
        {
            IsInitialized = true;

            var data = Data.CreateDataStorage();

            IniStorage ini = new IniStorage();
            ini.SetData(data);
            ini.ReadData(stream, true);

            _translator = new BasicTranslator(languageName, data);
            _nativeLanguageTranslator = _translator;
        }

        private Translator BuildLanguageTranslator(string translationFile)
        {
            if ((translationFile == null) || !File.Exists(translationFile))
            {
                return new BasicTranslator("empty", Data.CreateDataStorage());
            }

            return new BasicTranslator(translationFile);
        }


        public void TranslateComboBox<T>(string section, ComboBox cmb)
        {
            for(int i = 0; i<cmb.Items.Count; i++)
            {
                var item = ((T) (object) i);

                var enumName = item.GetType().Name;
                var name = enumName + "." + item;

                string translation = GetTranslation(section, name);
                if (!String.IsNullOrEmpty(translation))
                    cmb.Items[i] = translation;
            }
        }

        public void TranslateComboBoxWpf<T>(string section, System.Windows.Controls.ComboBox cmb)
        {
            for (int i = 0; i < cmb.Items.Count; i++)
            {
                var item = ((T)(object)i);

                var enumName = item.GetType().Name;
                var name = enumName + "." + item;

                string translation = GetTranslation(section, name);
                if (!String.IsNullOrEmpty(translation))
                    cmb.Items[i] = translation;
            }
        }

        public void TranslateComboBox<T>(ComboBox cmb)
        {
            TranslateComboBox<T>("Enums", cmb);
        }

        /// <summary>
        /// Translates a profile list by searching for predefined translations based on their GUID and apply the translated name to them
        /// </summary>
        /// <param name="profiles">The profile list</param>
        public void TranslateProfileList(IList<ConversionProfile> profiles)
        {
            foreach(ConversionProfile p in profiles)
            {
                try
                {
                    string translation = GetTranslation("ProfileNameByGuid", p.Guid);
                    if (!String.IsNullOrEmpty(translation))
                        p.Name = translation;
                }
                catch (ArgumentException)
                { 
                    //do nothing, profile must not be renamed 
                }
            }
        }

        /// <summary>
        /// Temporarily sets a translation while storing the old translator for later use. Use RevertTemporaryTranslation to revert to the initial translator.
        /// </summary>
        /// <param name="languageFileName">filename of the translation file, i.e. "english.ini"</param>
        /// <returns>true, if the file was successfully loaded</returns>
        public bool SetTemporaryTranslation(string languageFileName)
        {
            string languageFile = Path.Combine(TranslationPath, languageFileName);

            if (!File.Exists(languageFile))
                return false;

            if (_tmpTranslator == null)
            {
                _tmpTranslator = _translator;
            }

            _translator = new BasicTranslator(languageFile);

            return true;
        }

        /// <summary>
        /// Temporarily sets a translation while storing the old translator for later use. Use RevertTemporaryTranslation to revert to the initial translator.
        /// </summary>
        /// <param name="language">The language definition to use</param>
        /// <returns>true, if the translation was successfully loaded</returns>
        public bool SetTemporaryTranslation(Language language)
        {
            return SetTemporaryTranslation(language.FileName);
        }

        /// <summary>
        /// Reverts a temporarily set translation to it's original. If no temporary translation has been set, nothing will be reverted.
        /// </summary>
        public void RevertTemporaryTranslation()
        {
            if (_tmpTranslator != null)
            {
                _translator = _tmpTranslator;
                _tmpTranslator = null;
            }
        }

        public IEnumerable<Language> GetAvailableLanguages()
        {
            return Translator.FindTranslations(TranslationPath);
        }

        public Language FindBestLanguage(CultureInfo cultureInfo)
        {
            return FindBestLanguage(cultureInfo, GetAvailableLanguages());
        }

        public Language FindBestLanguage(CultureInfo cultureInfo, IEnumerable<Language> languages)
        {
            Language english = null;
            
            // try using LCID
            foreach (var language in languages)
            {
                try
                {
                    int lcid = Convert.ToInt32(language.LanguageId, 16);

                    if (lcid == cultureInfo.LCID)
                        return language;

                    if (lcid == 1033)
                        english = language;
                }
                catch (FormatException)
                {
                }
                catch (ArgumentOutOfRangeException)
                {
                }
            }

            // try using ISO2
            foreach (var language in languages)
            {
                if (string.IsNullOrWhiteSpace(language.Iso2))
                    continue;

                if (String.Equals(language.Iso2, cultureInfo.TwoLetterISOLanguageName, StringComparison.CurrentCultureIgnoreCase))
                    return language;
            }

            return english;
        }

        /// <summary>
        /// Populate a List with EnumValue objects that map enum values to their translation
        /// </summary>
        /// <typeparam name="T">Enum that will be used to fetch values</typeparam>
        /// <returns></returns>
        public IEnumerable<EnumValue<T>> GetEnumTranslation<T>() where T : struct, IConvertible
        {
            var values = new List<EnumValue<T>>();
            var type = typeof (T);

            foreach (var value in Enum.GetValues(type).Cast<T>())
            {
                string translation = GetTranslation("Enums", type.Name + "." + value);
                if (String.IsNullOrEmpty(translation))
                    translation = value.ToString();

                values.Add(new EnumValue<T>
                {
                    Name = translation,
                    Value = value
                });
            }

            return values;
        }
    }
}
