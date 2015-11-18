using SystemInterface.Microsoft.Win32;
using NUnit.Framework;
using pdfforge.PDFCreator.Licensing;
using Rhino.Mocks;

namespace PDFCreator.Test.Licensing
{
    [TestFixture]
    class LicenseValidatorTest
    {
        [Test]
        public void Constructor_AppliesMachineId()
        {
            var machine = new MachineId(12345, "12-34-56");

            var licenseValidator = new LicenseValidator(machine, MockRepository.GenerateStub<IRegistry>());

            Assert.AreEqual(machine, licenseValidator.MachineId);
        }

        [Test]
        public void LicenseKeyProperty_WithEmptyRegistry_ReturnsNull()
        {
            IRegistry registry = MockRepository.GenerateStub<IRegistry>();
            registry.Stub(x => x.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\PDFCreator.net", "License", null)).IgnoreArguments().Return(null);

            var machine = new MachineId(12345, "12-34-56");

            var licenseValidator = new LicenseValidator(machine, registry);

            Assert.IsNull(licenseValidator.LicenseKey);
        }

        [Test]
        public void LicenseKeyProperty_WithEmptyRegistryValue_ReturnsNull()
        {
            IRegistry registry = MockRepository.GenerateStub<IRegistry>();
            registry.Stub(x => x.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\PDFCreator.net", "License", null)).Return("");

            var machine = new MachineId(12345, "12-34-56");

            var licenseValidator = new LicenseValidator(machine, registry);

            Assert.IsNull(licenseValidator.LicenseKey);
        }

        [Test]
        public void LicenseKeyProperty_WithRegistryValue_ReturnsNull()
        {
            const string licenseKey = "AAAAA-BBBBB-CCCCC-DDDDD-EEEEE-FFFFF";

            IRegistry registry = MockRepository.GenerateStub<IRegistry>();
            registry.Stub(x => x.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\PDFCreator.net", "License", null)).Return(licenseKey);

            var machine = new MachineId(12345, "12-34-56");

            var licenseValidator = new LicenseValidator(machine, registry);

            Assert.AreEqual(licenseKey, licenseValidator.LicenseKey);
        }

        [Test]
        public void HasValidLicenseProperty_WithoutLicenseKey_ReturnsFalse()
        {
            IRegistry registry = MockRepository.GenerateStub<IRegistry>();
            registry.Stub(x => x.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\PDFCreator.net", "License", null)).Return("");

            var machine = new MachineId(12345, "12-34-56");

            var licenseValidator = new LicenseValidator(machine, registry);

            Assert.IsFalse(licenseValidator.IsValidLicense);
        }

        [Test]
        public void HasValidLicenseProperty_WithoutLicenseServerAnswer_ReturnsFalse()
        {
            const string licenseKey = "AAAAA-BBBBB-CCCCC-DDDDD-EEEEE-FFFFF";

            IRegistry registry = MockRepository.GenerateStub<IRegistry>();
            registry.Stub(x => x.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\PDFCreator.net", "License", null)).Return(licenseKey);
            registry.Stub(x => x.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\PDFCreator.net", "LSA", null)).Return(null);

            var machine = new MachineId(12345, "12-34-56");

            var licenseValidator = new LicenseValidator(machine, registry);

            Assert.IsFalse(licenseValidator.IsValidLicense);
        }

        [Test]
        public void HasValidLicenseProperty_WithNonMd5LicenseServerAnswer_ReturnsFalse()
        {
            const string licenseKey = "AAAAA-BBBBB-CCCCC-DDDDD-EEEEE-FFFFF";

            IRegistry registry = MockRepository.GenerateStub<IRegistry>();
            registry.Stub(x => x.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\PDFCreator.net", "License", null)).Return(licenseKey);
            registry.Stub(x => x.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\PDFCreator.net", "LSA", null)).Return("aaa");

            var machine = new MachineId(12345, "12-34-56");

            var licenseValidator = new LicenseValidator(machine, registry);

            Assert.IsFalse(licenseValidator.IsValidLicense);
        }

        [Test]
        public void HasValidLicenseProperty_WithProperLicenseServerAnswer_ReturnsTrue()
        {
            const string licenseKey = "AAAAA-BBBBB-CCCCC-DDDDD-EEEEE-FFFFF";

            IRegistry registry = MockRepository.GenerateStub<IRegistry>();
            registry.Stub(x => x.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\PDFCreator.net", "License", null)).Return(licenseKey);
            registry.Stub(x => x.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\PDFCreator.net", "LSA", null)).Return("W0xpY2Vuc2VdDQpFWFBJUkVTPTE0MzU3NjQ0NDMNCklEPWUyODhhYWIyY2JhMjMwMGY2YmY1YzYyMTI0ZmM4OGE0MWUxNTZhMTYNCg==");

            var machine = new MachineId(12345, "12-34-56");

            var licenseValidator = new LicenseValidator(machine, registry);

            Assert.IsTrue(licenseValidator.IsValidLicense);
        }
    }
}
