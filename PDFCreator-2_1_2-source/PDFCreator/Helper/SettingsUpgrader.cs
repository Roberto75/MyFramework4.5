﻿using System;
using System.Collections.Generic;
using pdfforge.DataStorage;

namespace pdfforge.PDFCreator.Helper
{
    /// <summary>
    /// The SettingsUpgrader class performs updates to the PDFCreator settings.
    /// This is done after loading the raw data and before loading them into the DataStorage classes.
    /// There is one update method from each version to the next (0 to 1, 1 to 2 etc.) and they are called subsequently, if required.
    /// </summary>
    public class SettingsUpgrader : DataUpgrader
    {
        private readonly List<Action> _upgradeMethods = new List<Action>();

        public const string VersionSettingPath = @"ApplicationProperties\SettingsVersion";

        public int SettingsVersion
        {
            get {
                string versionString = Data.GetValue(VersionSettingPath);
                return GetInt(versionString) ?? 0;
            }
        }

        public SettingsUpgrader(Data settingsData)
        {
            Data = settingsData;

            _upgradeMethods.Add(UpgradeV0ToV1);
            _upgradeMethods.Add(UpgradeV1ToV2);
            _upgradeMethods.Add(UpgradeV2ToV3);
        }

        public int NumberOfUpgradeMethods()
        {
            return _upgradeMethods.Count;
        }

        public void Upgrade(int targetVersion)
        {
            for (int i = SettingsVersion; i < Math.Min(targetVersion, _upgradeMethods.Count); i++)
            {
                // Call upgrade methods subsequently, starting with the current version
                var upgradeMethod = _upgradeMethods[i];
                upgradeMethod();
            }
        }

        public bool RequiresUpgrade(int targetVersion)
        {
            return targetVersion > SettingsVersion;
        }

        private void UpgradeV0ToV1()
        {
            MoveSettingInAllProfiles("DefaultFormat", "OutputFormat");
            MapSettingInAllProfiles("PdfSettings\\Security\\" + "EncryptionLevel", MapEncryptionNamesV1);
            ApplyNewSettingInAllProfiles("TitleTemplate", "<PrintJobName>");
            ApplyNewSettingInAllProfiles("AuthorTemplate", "<PrintJobAuthor>");

            Data.SetValue(VersionSettingPath, "1");
        }

        private void UpgradeV1ToV2()
        {
            MoveSettingInAllProfiles(@"CoverPage\AddBackground", @"BackgroundPage\OnCover");
            MoveSettingInAllProfiles(@"AttachmentPage\AddBackground", @"BackgroundPage\OnAttachment");
            MoveValue(@"ApplicationSettings\LastUsedProfilGuid", @"ApplicationSettings\LastUsedProfileGuid");
            Data.SetValue(VersionSettingPath, "2");
        }

        private void UpgradeV2ToV3()
        {
            MapSettingInAllProfiles(@"OutputFormat", MapOutputformatPdfA_V3);
            Data.SetValue(VersionSettingPath, "3");
        }

        private string MapOutputformatPdfA_V3(string s)
        {
            if (s.Equals("PdfA", StringComparison.CurrentCultureIgnoreCase))
                return "PdfA2B";
            return s;
        }

        private string MapEncryptionNamesV1(string s)
        {
            switch (s)
            {
                case "Low40Bit":
                    return "Rc40Bit";
                case "Medium128Bit":
                    return "Rc128Bit";
                case "High128BitAes":
                    return "Aes128Bit";
            }

            return "Rc128Bit";
        }

        

        private void MoveSettingInAllProfiles(string oldPath, string newPath)
        {
            int? numProfiles = GetInt(Data.GetValue(@"ConversionProfiles\numClasses"));

            if (numProfiles != null)
            {
                for (int i = 0; i < numProfiles; i++)
                {
                    string path = string.Format(@"ConversionProfiles\{0}\", i);
                    MoveValue(path + oldPath, path + newPath);
                }
            }
        }

        private void MapSettingInAllProfiles(string path, Func<string,string> mapFunction)
        {
            int? numProfiles = GetInt(Data.GetValue(@"ConversionProfiles\numClasses"));

            if (numProfiles != null)
            {
                for (int i = 0; i < numProfiles; i++)
                {
                    string p = string.Format(@"ConversionProfiles\{0}\" + path, i);
                    MapValue(p, mapFunction);
                }
            }
        }

        private void ApplyNewSettingInAllProfiles(string path, string defaultValue)
        {
            int? numProfiles = GetInt(Data.GetValue(@"ConversionProfiles\numClasses"));

            if (numProfiles != null)
            {
                for (int i = 0; i < numProfiles; i++)
                {
                    string p = string.Format(@"ConversionProfiles\{0}\" + path, i);
                    Data.SetValue(p, defaultValue);
                }
            }
        }

        private int? GetInt(string s)
        {
            int i;

            if (!int.TryParse(s, out i))
                return null;
            return i;
        }
    }
}
