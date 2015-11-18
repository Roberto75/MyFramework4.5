using System.Collections.Generic;
using System.ComponentModel;
using NUnit.Framework;
using PDFCreator.Test.ViewModels.Helper;
using pdfforge.PDFCreator;
using pdfforge.PDFCreator.Core.Jobs;
using pdfforge.PDFCreator.Core.Settings;
using pdfforge.PDFCreator.ViewModels;
using Rhino.Mocks;

namespace PDFCreator.Test.ViewModels
{
    [TestFixture]
    public class PrintJobViewModelTests
    {
        [Test]
        public void ViewModelWithOneJob_PendingJobsText_IsNull()
        {
            var queueStub = MockRepository.GenerateStub<IJobInfoQueue>();
            queueStub.Stub(x => x.Count).Return(1);

            var settings = new PdfCreatorSettings(null);

            var pjViewModel = new PrintJobViewModel(settings.ApplicationSettings, settings.ConversionProfiles, queueStub);

            Assert.AreEqual("Print more documents to merge or rearrange them", pjViewModel.PendingJobsText);
        }

        [Test]
        public void ViewModelWithOneJobs_PendingJobsText_IsCorrect()
        {
            var queueStub = MockRepository.GenerateStub<IJobInfoQueue>();
            queueStub.Stub(x => x.Count).Return(2);

            var settings = new PdfCreatorSettings(null);

            var pjViewModel = new PrintJobViewModel(settings.ApplicationSettings, settings.ConversionProfiles, queueStub);

            Assert.AreEqual("One more Job waiting", pjViewModel.PendingJobsText);
        }

        [Test]
        public void ViewModelWithThreeJobs_PendingJobsText_IsCorrect()
        {
            var queueStub = MockRepository.GenerateStub<IJobInfoQueue>();
            queueStub.Stub(x => x.Count).Return(3);

            var settings = new PdfCreatorSettings(null);

            var pjViewModel = new PrintJobViewModel(settings.ApplicationSettings, settings.ConversionProfiles, queueStub);

            Assert.AreEqual("There are 2 more Jobs waiting", pjViewModel.PendingJobsText);
        }

        [Test]
        public void EmptyViewModel_OnNewJob_CalledRaisePropertyChanged()
        {
            var queueStub = MockRepository.GenerateStub<IJobInfoQueue>();
            var eventStub = MockRepository.GenerateStub<IEventHandler<PropertyChangedEventArgs>>();
            var pjViewModel = new PrintJobViewModel(queueStub);
            pjViewModel.PropertyChanged += eventStub.OnEventRaised;
            var jobInfoStub = MockRepository.GenerateStub<IJobInfo>();
            var e = new NewJobInfoEventArgs(jobInfoStub);
            var propertyListener = new PropertyChangedListenerMock(pjViewModel, "JobCount");

            queueStub.Raise(x => x.OnNewJobInfo += null, jobInfoStub, e);

            Assert.IsTrue(propertyListener.WasCalled);
        }

        [Test]
        public void JobInfo_SaveMetadataWithoutChanges_ContainsSameMetadata()
        {
            var metadata = new Metadata
            {
                Title = "Title",
                Author = "Author",
                Subject = "Subject",
                Keywords = "Keywords"
            };

            var jobInfo = MockRepository.GenerateStub<IJobInfo>();
            jobInfo.Metadata = metadata.Copy();

            var pjViewModel = new PrintJobViewModel(jobInfo);

            pjViewModel.SaveCommand.Execute(null);

            Assert.AreEqual(metadata.Title, pjViewModel.JobInfo.Metadata.Title);
            Assert.AreEqual(metadata.Author, pjViewModel.JobInfo.Metadata.Author);
            Assert.AreEqual(metadata.Subject, pjViewModel.JobInfo.Metadata.Subject);
            Assert.AreEqual(metadata.Keywords, pjViewModel.JobInfo.Metadata.Keywords);
        }

        [Test]
        public void JobInfo_SaveMetadata_ContainsChangedMetadata()
        {
            var metadata = new Metadata
            {
                Title = "Title",
                Author = "Author",
                Subject = "Subject",
                Keywords = "Keywords"
            };

            var jobInfo = MockRepository.GenerateStub<IJobInfo>();
            jobInfo.Metadata = metadata.Copy();

            var pjViewModel = new PrintJobViewModel(jobInfo);

            pjViewModel.Metadata.Title = "MyTitle";
            pjViewModel.Metadata.Author = "MyAuthor";
            pjViewModel.Metadata.Subject = "MySubject";
            pjViewModel.Metadata.Keywords = "MyKeywords";

            pjViewModel.SaveCommand.Execute(null);

            Assert.AreEqual("MyTitle", pjViewModel.JobInfo.Metadata.Title);
            Assert.AreEqual("MyAuthor", pjViewModel.JobInfo.Metadata.Author);
            Assert.AreEqual("MySubject", pjViewModel.JobInfo.Metadata.Subject);
            Assert.AreEqual("MyKeywords", pjViewModel.JobInfo.Metadata.Keywords);
        }

        [Test]
        public void ViewModelWithProfiles_SelectProfiles_SelectsCorrectProfile()
        {
            var appSettings = new ApplicationSettings();

            appSettings.LastUsedProfileGuid = "guid1";

            IList<ConversionProfile> profiles = new List<ConversionProfile>();
            profiles.Add(new ConversionProfile { Guid = "guid1" });
            profiles.Add(new ConversionProfile { Guid = "guid2" });

            var pjViewModel = new PrintJobViewModel(appSettings, profiles, JobInfoQueue.Instance);

            const string guid = "guid2";

            pjViewModel.SelectProfileByGuid(guid);

            Assert.AreEqual(guid, pjViewModel.SelectedProfile.Guid);
        }

        [Test]
        public void ViewModelWithProfiles_PreSelectedProfileIsSelected()
        {
            var appSettings = new ApplicationSettings();

            appSettings.LastUsedProfileGuid = "guid1";

            IList<ConversionProfile> profiles = new List<ConversionProfile>();

            var preselectedConversionProfile = new ConversionProfile{ Guid = "Preselected"};

            profiles.Add(new ConversionProfile { Guid = "guid1" });
            profiles.Add(preselectedConversionProfile);

            var pjViewModel = new PrintJobViewModel(appSettings, profiles, JobInfoQueue.Instance, preselectedConversionProfile);

            Assert.AreEqual(preselectedConversionProfile.Guid, pjViewModel.SelectedProfile.Guid, "PreselectedProfile not selected in profile combo box");
        }

        [Test]
        public void ViewModelWithProfiles_AndAppSettings_InitializedWithLastUsedProfile()
        {
            var appSettings = new ApplicationSettings();

            const string guid = "guid2";

            appSettings.LastUsedProfileGuid = guid;

            IList<ConversionProfile> profiles = new List<ConversionProfile>();
            profiles.Add(new ConversionProfile { Guid = "guid1" });
            profiles.Add(new ConversionProfile { Guid = guid });

            var pjViewModel = new PrintJobViewModel(appSettings, profiles, JobInfoQueue.Instance);

            Assert.AreEqual(guid, pjViewModel.SelectedProfile.Guid);
        }

        [Test]
        public void ViewModel_WithoutChanges_HasCancelAction()
        {
            var pjViewModel = new PrintJobViewModel();

            Assert.AreEqual(PrintJobAction.Cancel, pjViewModel.PrintJobAction);
        }

        [Test]
        public void ViewModel_WithSaveCommand_HasSaveAction()
        {
            var pjViewModel = new PrintJobViewModel();

            pjViewModel.SaveCommand.Execute(null);

            Assert.AreEqual(PrintJobAction.Save, pjViewModel.PrintJobAction);
        }

        [Test]
        public void ViewModel_WithEmailCommand_HasEmailAction()
        {
            var pjViewModel = new PrintJobViewModel();

            pjViewModel.EmailCommand.Execute(null);

            Assert.AreEqual(PrintJobAction.EMail, pjViewModel.PrintJobAction);
        }

        [Test]
        public void ViewModel_WithManagePrintJobsCommand_HasSaveAction()
        {
            var pjViewModel = new PrintJobViewModel();

            pjViewModel.ManagePrintJobsCommand.Execute(null);

            Assert.AreEqual(PrintJobAction.ManagePrintJobs, pjViewModel.PrintJobAction);
        }

        [Test]
        public void ViewModel_WithEmptyQueue_DoesAllowManagePrintJobs()
        {
            var queueStub = MockRepository.GenerateStub<IJobInfoQueue>();
            queueStub.Stub(x => x.Count).Return(0);

            var pjViewModel = new PrintJobViewModel(queueStub);

            Assert.IsTrue(pjViewModel.ManagePrintJobsCommand.IsExecutable);
        }

        [Test]
        public void ViewModel_WithSingleJobQueue_DoesAllowManagePrintJobs()
        {
            var queueStub = MockRepository.GenerateStub<IJobInfoQueue>();
            queueStub.Stub(x => x.Count).Return(1);

            var pjViewModel = new PrintJobViewModel(queueStub);

            Assert.IsTrue(pjViewModel.ManagePrintJobsCommand.IsExecutable);
        }

        [Test]
        public void ViewModel_WithTwoJobQueue_DoesAllowManagePrintJobs()
        {
            var queueStub = MockRepository.GenerateStub<IJobInfoQueue>();
            queueStub.Stub(x => x.Count).Return(2);

            var pjViewModel = new PrintJobViewModel(queueStub);

            Assert.IsTrue(pjViewModel.ManagePrintJobsCommand.IsExecutable);
        }


    }
}
