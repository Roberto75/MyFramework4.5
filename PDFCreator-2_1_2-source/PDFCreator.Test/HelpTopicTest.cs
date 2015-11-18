﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using pdfforge.PDFCreator.Helper;
using pdfforge.PDFCreator.Shared.Helper;

namespace PDFCreator.Test
{
    [TestFixture]
    class HelpTopicTest
    {
        private readonly List<string> _tempFoldersList = new List<string>();

        [TearDown]
        public void TearDown()
        {
            foreach (var folder in _tempFoldersList)
            {
                if (Directory.Exists(folder))
                {
                    try
                    {
                        Directory.Delete(folder, true);
                    }
                    catch (IOException)
                    {
                    }
                }
            }
        }

        [Test]
        public void TestEnglishManualExists()
        {
            string manualFolder = FindManualFolder();

            Assert.IsTrue(File.Exists(Path.Combine(manualFolder, "PDFCreator_english.chm")));
        }

        [Test]
        public void TestHelpTopicFilesExist()
        {
            string manualFolder = FindManualFolder();

            foreach (string manual in Directory.GetFiles(manualFolder, "*.chm"))
            {
                string folder = DecompileChmFile(manual);
                //Assert.IsTrue(Directory.Exists(Path.Combine(folder, "html")), "Decompiling failed, the html folder does not exist");

                _tempFoldersList.Add(folder);
                TestSingleHelpFile(Path.GetFileName(manual), folder);
            }
        }

        private void TestSingleHelpFile(string filename, string folder)
        {
            string helpPath = folder; //Path.Combine(folder, "html");

            Console.WriteLine(@"Checking " + filename);

            foreach (HelpTopic topic in Enum.GetValues(typeof(HelpTopic)))
            {
                string sourceFile = UserGuideHelper.GetTopic(topic);
                sourceFile = Path.Combine(helpPath, sourceFile + ".html");
                Assert.IsTrue(condition: File.Exists(sourceFile), message: string.Format("Help file '{0}' does not exist in {1}!", sourceFile, filename));
            }
        }

        private string DecompileChmFile(string chmFile)
        {
            Assert.NotNull(chmFile);

            string tmpFolder = GetTemporaryDirectory();

            string tmpFile = Path.Combine(tmpFolder, Path.GetFileName(chmFile));
            File.Copy(chmFile, tmpFile);
            string args = string.Format("-decompile {0} {1}", ".", Path.GetFileName(chmFile));
            args = args.Replace("\\", "/");

            var psi = new ProcessStartInfo("hh", args);
            psi.WorkingDirectory = tmpFolder;
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;

            Process p = Process.Start(psi);
            p.WaitForExit();

            return tmpFolder;
        }

        [Test]
        public void TestHelpTopicsAssigned()
        {
            foreach (HelpTopic topic in Enum.GetValues(typeof(HelpTopic)))
            {
                Assert.IsNotNullOrEmpty(UserGuideHelper.GetTopic(topic), string.Format("Topic {0} does not have a html reference attached", topic));
            }
        }

        private static string FindManualFolder()
        {
            System.Reflection.Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
            string appDir = Path.GetDirectoryName(a.CodeBase.Replace(@"file:///", ""));
            Assert.IsNotNull(appDir);

            var candidates = new[] { appDir, Path.GetFullPath(Path.Combine(appDir, @"..\..\..\")), Path.GetFullPath(Path.Combine(appDir, @"..\..\..\PDFCreator")), Path.GetFullPath(Path.Combine(appDir, @"..\..\..\..\Manual")), "", "Manuals" };

            foreach (string dir in candidates)
            {
                Console.Write(@"Translation folder candidate: {0}: ", dir);
                if (Directory.Exists(dir) && Directory.GetFiles(dir, "*.chm").Length > 0)
                {
                    Console.WriteLine(@"OK");
                    return dir;
                }

                Console.WriteLine(@"FAIL");
            }

            throw new IOException("Could not find test file folder");
        }

        public string GetTemporaryDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), "PDFCreatorTest-" + Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }
    }
}
