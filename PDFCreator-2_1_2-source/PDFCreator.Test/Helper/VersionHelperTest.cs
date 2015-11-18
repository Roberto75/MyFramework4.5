using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SystemInterface.Microsoft.Win32;
using NUnit.Framework;
using pdfforge.PDFCreator.Helper;
using Rhino.Mocks;

namespace PDFCreator.Test
{
    [TestFixture]
    class VersionHelperTest
    {
        [Test]
        public void CurrentVersionWithTwoDigits_HasValidFormat()
        {
            var currentVersion = VersionHelper.GetCurrentApplicationVersion_TwoDigits();
            Assert.IsTrue(Regex.IsMatch(currentVersion, @"^\d+.\d+$"), "Current Version has invalid formatting.");
        }

        [Test]
        public void CurrentVersionWithThreeDigits_HasValidFormat()
        {
            var currentVersion = VersionHelper.GetCurrentApplicationVersion_ThreeDigits();
            Assert.IsTrue(Regex.IsMatch(currentVersion, @"^\d+.\d+.\d+$"), "Current Version has invalid formatting.");
        }

        [Test]
        public void CurrentVersion_WithBuildnumber_HasValidFormat()
        {
            var currentVersion = VersionHelper.GetCurrentApplicationVersion_WithBuildnumber();
            if (VersionHelper.GetCurrentApplicationVersion().Revision == 0)
                Assert.IsTrue(Regex.IsMatch(currentVersion, @"^v\d+.\d+.\d+ \(Developer Preview\)$"), "Current Version has invalid formatting.");
            else
                Assert.IsTrue(Regex.IsMatch(currentVersion, @"^v\d+.\d+.\d+ Build \d+$"), "Current Version has invalid formatting.");
                
        }
    }
}
