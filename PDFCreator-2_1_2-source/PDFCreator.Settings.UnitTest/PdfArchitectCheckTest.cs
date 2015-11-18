﻿using SystemInterface.IO;
using SystemInterface.Microsoft.Win32;
using NUnit.Framework;
using pdfforge.PDFCreator.Core.Settings;

namespace PDFCreator.Settings.UnitTest
{
    [TestFixture]
    public class PdfArchitectCheckTest
    {
        [Test]
        public void InstallationPath_WithNoPdfArchitectInstalled_ReturnsNull()
        {
            var factory = new PdfArchitectMockRegistryFactory();
            IRegistry registryMock = factory.BuildRegistry();
            IFile fileMock = factory.BuildFile();

            var path = PdfArchitectCheck.InstallationPath(registryMock, fileMock);

            Assert.IsNull(path);
        }

        [Test]
        public void InstallationPath_WithPdfArchitect1Installed_ReturnsCorrectPath()
        {
            var factory = new PdfArchitectMockRegistryFactory();
            factory.AddArchitectVersion("000", "PDF Architect", @"C:\Program Files\PDF Architect", "PDF Architect.exe", false);
            IRegistry registryMock = factory.BuildRegistry();
            IFile fileMock = factory.BuildFile();

            var path = PdfArchitectCheck.InstallationPath(registryMock, fileMock);

            Assert.AreEqual(@"C:\Program Files\PDF Architect\PDF Architect.exe", path);
        }

        [Test]
        public void InstallationPath_WithPdfArchitect1InstalledWow64_ReturnsCorrectPath()
        {
            var factory = new PdfArchitectMockRegistryFactory();
            factory.AddArchitectVersion("000", "PDF Architect", @"C:\Program Files (x86)\PDF Architect", "PDF Architect.exe", true);
            IRegistry registryMock = factory.BuildRegistry();
            IFile fileMock = factory.BuildFile();

            var path = PdfArchitectCheck.InstallationPath(registryMock, fileMock);

            Assert.AreEqual(@"C:\Program Files (x86)\PDF Architect\PDF Architect.exe", path);
        }

        [Test]
        public void InstallationPath_WithPdfArchitect2Installed_ReturnsCorrectPath()
        {
            var factory = new PdfArchitectMockRegistryFactory();
            factory.AddArchitectVersion("000", "PDF Architect 2", @"C:\Program Files\PDF Architect 2", "PDF Architect 2.exe", false);
            IRegistry registryMock = factory.BuildRegistry();
            IFile fileMock = factory.BuildFile();

            var path = PdfArchitectCheck.InstallationPath(registryMock, fileMock);

            Assert.AreEqual(@"C:\Program Files\PDF Architect 2\PDF Architect 2.exe", path);
        }

        [Test]
        public void InstallationPath_WithPdfArchitect2InstalledWow64_ReturnsCorrectPath()
        {
            var factory = new PdfArchitectMockRegistryFactory();
            factory.AddArchitectVersion("000", "PDF Architect 2", @"C:\Program Files (x86)\PDF Architect 2", "PDF Architect 2.exe", true);
            IRegistry registryMock = factory.BuildRegistry();
            IFile fileMock = factory.BuildFile();

            var path = PdfArchitectCheck.InstallationPath(registryMock, fileMock);

            Assert.AreEqual(@"C:\Program Files (x86)\PDF Architect 2\PDF Architect 2.exe", path);
        }

        [Test]
        public void InstallationPath_WithPdfArchitect3Installed_ReturnsCorrectPath()
        {
            var factory = new PdfArchitectMockRegistryFactory();
            factory.AddArchitectVersion("000", "PDF Architect 3", @"C:\Program Files\PDF Architect 3", "PDF Architect 3.exe", false);
            IRegistry registryMock = factory.BuildRegistry();
            IFile fileMock = factory.BuildFile();

            var path = PdfArchitectCheck.InstallationPath(registryMock, fileMock);

            Assert.AreEqual(@"C:\Program Files\PDF Architect 3\PDF Architect 3.exe", path);
        }

        [Test]
        public void InstallationPath_WithPdfArchitect3WithShortExeNameInstalled_ReturnsCorrectPath()
        {
            var factory = new PdfArchitectMockRegistryFactory();
            factory.AddArchitectVersion("000", "PDF Architect 3", @"C:\Program Files\PDF Architect 3", "architect.exe", false);
            IRegistry registryMock = factory.BuildRegistry();
            IFile fileMock = factory.BuildFile();

            var path = PdfArchitectCheck.InstallationPath(registryMock, fileMock);

            Assert.AreEqual(@"C:\Program Files\PDF Architect 3\architect.exe", path);
        }

        [Test]
        public void InstallationPath_WithPdfArchitect3WithInvalidExeName_ReturnsNull()
        {
            var factory = new PdfArchitectMockRegistryFactory();
            factory.AddArchitectVersion("000", "PDF Architect 3", @"C:\Program Files\PDF Architect 3", "architectXAZ.exe", false);
            IRegistry registryMock = factory.BuildRegistry();
            IFile fileMock = factory.BuildFile();

            var path = PdfArchitectCheck.InstallationPath(registryMock, fileMock);

            Assert.IsNull(path);
        }

        [Test]
        public void InstallationPath_WithPdfArchitect3InstalledWow64_ReturnsCorrectPath()
        {
            var factory = new PdfArchitectMockRegistryFactory();
            factory.AddArchitectVersion("000", "PDF Architect 3", @"C:\Program Files (x86)\PDF Architect 3", "PDF Architect 3.exe", true);
            IRegistry registryMock = factory.BuildRegistry();
            IFile fileMock = factory.BuildFile();

            var path = PdfArchitectCheck.InstallationPath(registryMock, fileMock);

            Assert.AreEqual(@"C:\Program Files (x86)\PDF Architect 3\PDF Architect 3.exe", path);
        }

        [Test]
        public void InstallationPath_WithPdfArchitect2And3Installed_ReturnsArchitect3Path()
        {
            var factory = new PdfArchitectMockRegistryFactory();
            factory.AddArchitectVersion("000", "PDF Architect 2", @"C:\Program Files\PDF Architect 2", "PDF Architect 2.exe", true);
            factory.AddArchitectVersion("001", "PDF Architect 3", @"C:\Program Files\PDF Architect 3", "PDF Architect 3.exe", true);
            IRegistry registryMock = factory.BuildRegistry();
            IFile fileMock = factory.BuildFile();

            var path = PdfArchitectCheck.InstallationPath(registryMock, fileMock);

            Assert.AreEqual(@"C:\Program Files\PDF Architect 3\PDF Architect 3.exe", path);
        }

        [Test]
        public void InstallationPath_WithPdfArchitect2WowAnd3Installed_ReturnsArchitect3Path()
        {
            var factory = new PdfArchitectMockRegistryFactory();
            factory.AddArchitectVersion("000", "PDF Architect 2", @"C:\Program Files\PDF Architect 2", "PDF Architect 2.exe", true);
            factory.AddArchitectVersion("001", "PDF Architect 3", @"C:\Program Files\PDF Architect 3", "PDF Architect 3.exe", false);
            IRegistry registryMock = factory.BuildRegistry();
            IFile fileMock = factory.BuildFile();

            var path = PdfArchitectCheck.InstallationPath(registryMock, fileMock);

            Assert.AreEqual(@"C:\Program Files\PDF Architect 3\PDF Architect 3.exe", path);
        }

        [Test]
        public void InstallationPath_WithPdfArchitect2And3WowInstalled_ReturnsArchitect3Path()
        {
            var factory = new PdfArchitectMockRegistryFactory();
            factory.AddArchitectVersion("000", "PDF Architect 2", @"C:\Program Files\PDF Architect 2", "PDF Architect 2.exe", false);
            factory.AddArchitectVersion("001", "PDF Architect 3", @"C:\Program Files\PDF Architect 3", "PDF Architect 3.exe", true);
            IRegistry registryMock = factory.BuildRegistry();
            IFile fileMock = factory.BuildFile();

            var path = PdfArchitectCheck.InstallationPath(registryMock, fileMock);

            Assert.AreEqual(@"C:\Program Files\PDF Architect 3\PDF Architect 3.exe", path);
        }

        [Test]
        public void InstallationPath_WithJustManagementConsoleInstalled_ReturnsNull()
        {
            var factory = new PdfArchitectMockRegistryFactory();
            factory.AddArchitectVersion("000", "PDF Architect Enterprise Server", @"C:\Program Files\PDF Architect Enterprise", "PDF Architect.exe", false);
            IRegistry registryMock = factory.BuildRegistry();
            IFile fileMock = factory.BuildFile();

            var path = PdfArchitectCheck.InstallationPath(registryMock, fileMock);

            Assert.IsNull(path);
        }

        [Test]
        public void Installed_WithNotPdfArchitect_ReturnsFalse()
        {
            var factory = new PdfArchitectMockRegistryFactory();
            
            IRegistry registryMock = factory.BuildRegistry();
            IFile fileMock = factory.BuildFile();

            var isInstalled = PdfArchitectCheck.Installed(registryMock, fileMock);

            Assert.IsFalse(isInstalled);
        }

        [Test]
        public void Installed_WithPdfArchitect3WithInvalidExeName_ReturnsFalse()
        {
            var factory = new PdfArchitectMockRegistryFactory();
            factory.AddArchitectVersion("000", "PDF Architect 3", @"C:\Program Files\PDF Architect 3", "architectXAZ.exe", false);
            IRegistry registryMock = factory.BuildRegistry();
            IFile fileMock = factory.BuildFile();

            var isInstalled = PdfArchitectCheck.Installed(registryMock, fileMock);

            Assert.IsFalse(isInstalled);
        }

        [Test]
        public void Installed_WithPdfArchitect3InstalledWow64_ReturnsTrue()
        {
            var factory = new PdfArchitectMockRegistryFactory();
            factory.AddArchitectVersion("000", "PDF Architect 3", @"C:\Program Files (x86)\PDF Architect 3", "PDF Architect 3.exe", true);
            IRegistry registryMock = factory.BuildRegistry();
            IFile fileMock = factory.BuildFile();

            var isInstalled = PdfArchitectCheck.Installed(registryMock, fileMock);

            Assert.IsTrue(isInstalled);
        }
    }
}
