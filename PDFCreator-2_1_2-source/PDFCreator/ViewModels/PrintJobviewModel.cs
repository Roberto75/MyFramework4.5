using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Threading;
using pdfforge.PDFCreator.Core.Jobs;
using pdfforge.PDFCreator.Core.Settings;
using pdfforge.PDFCreator.Helper;
using pdfforge.PDFCreator.Shared.Helper;
using pdfforge.PDFCreator.Shared.ViewModels;

namespace pdfforge.PDFCreator.ViewModels
{
    public class PrintJobViewModel : ViewModelBase
    {
        private readonly Dispatcher _currentThreadDispatcher;
        private readonly IJobInfoQueue _jobInfoQueue;
        private readonly ConversionProfile _preselectedProfile;
        private ApplicationSettings _applicationSettings;
        private IJobInfo _jobInfo;
        private IList<ConversionProfile> _profiles;

        public PrintJobViewModel(ApplicationSettings appSettings, IList<ConversionProfile> profiles,
            IJobInfoQueue jobInfoQueue, ConversionProfile preselectedProfile = null, JobInfo jobInfo = null)
        {
            _currentThreadDispatcher = Dispatcher.CurrentDispatcher;

            Profiles = profiles;
            _preselectedProfile = preselectedProfile;
                //must be set before ApplicationSettings because it is evaluated in the Set method of appsettings.
            ApplicationSettings = appSettings;

            _jobInfoQueue = jobInfoQueue;
            _jobInfoQueue.OnNewJobInfo += OnNewJobInfo;

            SaveCommand = new DelegateCommand(ExecuteSave);
            EmailCommand = new DelegateCommand(ExecuteMail);
            ManagePrintJobsCommand = new DelegateCommand(ExecuteManagePrintJobs);

            if (jobInfo != null)
            {
                JobInfo = jobInfo;
            }
        }

        public PrintJobViewModel(IJobInfoQueue jobInfoQueue, ConversionProfile preselectedProfile = null)
            : this(
                SettingsHelper.Settings.ApplicationSettings, SettingsHelper.Settings.ConversionProfiles, jobInfoQueue,
                preselectedProfile, null)
        {
        }

        public PrintJobViewModel()
            : this(JobInfoQueue.Instance, null)
        {
            JobInfo = new JobInfo
            {
                Metadata = new Metadata()
            };
        }

        public PrintJobViewModel(IJobInfo jobInfo, ConversionProfile preselectedProfile = null)
            : this(JobInfoQueue.Instance, preselectedProfile)
        {
            JobInfo = jobInfo;
        }

        public PrintJobAction PrintJobAction { get; private set; }
        public ICollectionView ProfilesView { get; private set; }
        public Metadata Metadata { get; set; }
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand EmailCommand { get; private set; }
        public DelegateCommand ManagePrintJobsCommand { get; private set; }

        public IJobInfo JobInfo
        {
            get { return _jobInfo; }
            private set
            {
                _jobInfo = value;
                RaisePropertyChanged("JobInfo");
                if (_jobInfo != null)
                {
                    Metadata = _jobInfo.Metadata.Copy();
                }
            }
        }

        public ConversionProfile SelectedProfile
        {
            get { return (ConversionProfile) ProfilesView.CurrentItem; }
        }

        public IList<ConversionProfile> Profiles
        {
            get { return _profiles; }
            set
            {
                _profiles = value;
                ProfilesView = CollectionViewSource.GetDefaultView(_profiles);
                ProfilesView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                RaisePropertyChanged("Profiles");
                RaisePropertyChanged("ProfilesView");
            }
        }

        public ApplicationSettings ApplicationSettings
        {
            get { return _applicationSettings; }
            set
            {
                _applicationSettings = value;
                RaisePropertyChanged("App");
                if (_preselectedProfile != null)
                    SelectProfileByGuid(_preselectedProfile.Guid);
                else
                    SelectProfileByGuid(value.LastUsedProfileGuid);
            }
        }

        public string PendingJobsText
        {
            get
            {
                var additionalJobs = _jobInfoQueue.Count - 1;
                if (additionalJobs == 1)
                    return TranslationHelper.Instance.GetTranslation("PrintJobWindow", "OneMoreJobWaiting",
                        "One more Job waiting");

                if (additionalJobs > 1)
                    return TranslationHelper.Instance.GetFormattedTranslation("PrintJobWindow", "MoreJobsWaiting",
                        "There are {0} more Jobs waiting", additionalJobs);

                return TranslationHelper.Instance.GetTranslation("PrintJobWindow", "NoJobsWaiting",
                    "Print more documents to merge or rearrange them");
                ;
            }
        }

        public void SelectProfileByGuid(string guid)
        {
            foreach (var conversionProfile in Profiles)
            {
                if (conversionProfile.Guid == guid)
                    ProfilesView.MoveCurrentTo(conversionProfile);
            }
        }

        private void ExecuteSave(object obj)
        {
            JobInfo.Metadata = Metadata;
            ApplicationSettings.LastUsedProfileGuid = SelectedProfile.Guid;
            PrintJobAction = PrintJobAction.Save;
        }

        private void ExecuteMail(object obj)
        {
            JobInfo.Metadata = Metadata;

            ApplicationSettings.LastUsedProfileGuid = SelectedProfile.Guid;
            PrintJobAction = PrintJobAction.EMail;
        }

        private void ExecuteManagePrintJobs(object obj)
        {
            PrintJobAction = PrintJobAction.ManagePrintJobs;
        }

        private void OnNewJobInfo(object sender, NewJobInfoEventArgs e)
        {
            Action<IJobInfo> addMethod = RaisePropertyChangedEvents;

            if (!_currentThreadDispatcher.Thread.IsAlive)
            {
                _jobInfoQueue.OnNewJobInfo -= OnNewJobInfo;
                return;
            }

            _currentThreadDispatcher.Invoke(addMethod, e.JobInfo);
        }

        private void RaisePropertyChangedEvents(IJobInfo jobInfo)
        {
            RaisePropertyChanged("JobCount");
            RaisePropertyChanged("PendingJobsText");
            ManagePrintJobsCommand.RaiseCanExecuteChanged();
            RaisePropertyChanged("CanManagePrintJobs");
        }
    }

    public enum PrintJobAction
    {
        Cancel,
        Save,
        EMail,
        ManagePrintJobs
    }
}