﻿using System.Text.RegularExpressions;
using SystemInterface.Microsoft.Win32;
using NUnit.Framework;
using pdfforge.PDFCreator.Helper;
using Rhino.Mocks;

namespace PDFCreator.Test
{
    [TestFixture]
    class WelcomeVersionTest
    {
        [Test]
        public void FirstRunTest_WelcomeVersionIsTheCurrentVersion_IsNotFirstRun()
        {
            IRegistry registryMock = MockRepository.GenerateMock<IRegistry>();

            var welcomeSettingsHelper = new WelcomeSettingsHelper(registryMock);
            var currentVersion = VersionHelper.GetCurrentApplicationVersion_WithBuildnumber();

            registryMock.Stub(x => x.GetValue(WelcomeSettingsHelper.RegistryKeyForWelcomeSettings, WelcomeSettingsHelper.RegistryValueForWelcomeVersion, null)).Return(currentVersion);

            Assert.IsFalse(welcomeSettingsHelper.IsFirstRun(), "Current Version as WelcomeVersion detected as FirstRun.");

            registryMock.AssertWasCalled(
                x => x.GetValue(WelcomeSettingsHelper.RegistryKeyForWelcomeSettings,
                        WelcomeSettingsHelper.RegistryValueForWelcomeVersion, null), options => options.Repeat.Once());
        }
        
        [Test]
        public void FirstRunTest_NoWelcomeVersionSet_IsFirstRun()
        {
            IRegistry registryMock = MockRepository.GenerateMock<IRegistry>();
            registryMock.Stub(x => x.GetValue(WelcomeSettingsHelper.RegistryKeyForWelcomeSettings, WelcomeSettingsHelper.RegistryValueForWelcomeVersion, null)).Return("");

            var welcomeSettingsHelper = new WelcomeSettingsHelper(registryMock);

            Assert.IsTrue(welcomeSettingsHelper.IsFirstRun(), "Empty registry value for WelcomeVersion not detected as FirstRun.");

            registryMock.AssertWasCalled(
                x => x.GetValue(WelcomeSettingsHelper.RegistryKeyForWelcomeSettings,
                        WelcomeSettingsHelper.RegistryValueForWelcomeVersion, null), options => options.Repeat.Once());
        }

        [Test]
        public void FirstRunTest_WelcomeVersionIsSmallerThanCurrentVersion_IsFirstRun()
        {
            IRegistry registryMock = MockRepository.GenerateMock<IRegistry>();
            registryMock.Stub(x => x.GetValue(WelcomeSettingsHelper.RegistryKeyForWelcomeSettings, WelcomeSettingsHelper.RegistryValueForWelcomeVersion, null)).Return("0.0.0");

            var welcomeSettingsHelper = new WelcomeSettingsHelper(registryMock);

            Assert.IsTrue(welcomeSettingsHelper.IsFirstRun(), "Smaller WelcomeVersion in registry not detected as FirstRun.");

            registryMock.AssertWasCalled(
                x => x.GetValue(WelcomeSettingsHelper.RegistryKeyForWelcomeSettings,
                        WelcomeSettingsHelper.RegistryValueForWelcomeVersion, null), options => options.Repeat.Once());
        }

        [Test]
        public void FirstRunTest_WelcomeVersionIsBiggerThanCurrentVersion_IsFirstRun()
        {
            IRegistry registryMock = MockRepository.GenerateMock<IRegistry>();
            registryMock.Stub(x => x.GetValue(WelcomeSettingsHelper.RegistryKeyForWelcomeSettings, WelcomeSettingsHelper.RegistryValueForWelcomeVersion, null)).Return("999999999.9.9");

            var welcomeSettingsHelper = new WelcomeSettingsHelper(registryMock);

            Assert.IsTrue(welcomeSettingsHelper.IsFirstRun(), "Bigger WelcomeVersion not detected as FirstRun.");

            registryMock.AssertWasCalled(
                x => x.GetValue(WelcomeSettingsHelper.RegistryKeyForWelcomeSettings,
                        WelcomeSettingsHelper.RegistryValueForWelcomeVersion, null), options => options.Repeat.Once());
        }

        [Test]
        public void SetCurrentVersionAsWelcomeVersionTest_RegistrySetValueGetsCalled()
        {
            IRegistry registryMock = MockRepository.GenerateMock<IRegistry>();

            var welcomeSettingsHelper = new WelcomeSettingsHelper(registryMock);
            var currentVersion = VersionHelper.GetCurrentApplicationVersion_WithBuildnumber();

            welcomeSettingsHelper.SetCurrentApplicationVersionAsWelcomeVersionInRegistry();

            registryMock.AssertWasCalled(
                x => x.SetValue(WelcomeSettingsHelper.RegistryKeyForWelcomeSettings,
                    WelcomeSettingsHelper.RegistryValueForWelcomeVersion, currentVersion),
                options => options.Repeat.Once());
        }
    }
}
