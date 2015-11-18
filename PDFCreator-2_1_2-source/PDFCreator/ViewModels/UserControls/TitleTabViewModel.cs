using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using pdfforge.PDFCreator.Core.Settings;
using pdfforge.PDFCreator.Shared.ViewModels;
using pdfforge.PDFCreator.Shared.ViewModels.Wrapper;

namespace pdfforge.PDFCreator.ViewModels.UserControls
{
    public class TitleTabViewModel : ApplicationSettingsViewModel
    {
        private SynchronizedCollection<TitleReplacement> _titleReplacements;
        private ICollectionView _titleReplacementView;

        public TitleTabViewModel()
        {
            SettingsChanged += OnSettingsChanged;

            TitleMoveUpCommand = new DelegateCommand(TitleMoveUpCommandExecute, TitleMoveUpCommandCanExecute);
            TitleMoveDownCommand = new DelegateCommand(TitleMoveDownCommandExecute, TitleMoveDownCommandCanExecute);
            TitleAddCommand = new DelegateCommand(TitleAddCommandExecute);
            TitleDeleteCommand = new DelegateCommand(TitleDeleteCommandExecute, TitleDeleteCommandCanExecute);
        }

        public DelegateCommand TitleMoveUpCommand { get; set; }
        public DelegateCommand TitleMoveDownCommand { get; set; }
        public DelegateCommand TitleAddCommand { get; set; }
        public DelegateCommand TitleDeleteCommand { get; set; }

        public ObservableCollection<TitleReplacement> TitleReplacements
        {
            get
            {
                if (_titleReplacements == null)
                    return null;
                return _titleReplacements.ObservableCollection;
            }
        }

        private void OnSettingsChanged(object sender, EventArgs args)
        {
            var titleReplacement = ApplicationSettings == null ? null : ApplicationSettings.TitleReplacement;
            ApplyTitleReplacements(titleReplacement);
        }

        private void ApplyTitleReplacements(IEnumerable<TitleReplacement> titleReplacements)
        {
            if (TitleReplacements != null)
            {
                TitleReplacements.CollectionChanged -= TitleReplacementsOnCollectionChanged;
            }

            if (titleReplacements != null)
            {
                var replacements = titleReplacements as IList<TitleReplacement> ?? titleReplacements.ToList();

                _titleReplacements = new SynchronizedCollection<TitleReplacement>(replacements);
                _titleReplacements.ObservableCollection.CollectionChanged += TitleReplacementsOnCollectionChanged;
                _titleReplacementView = CollectionViewSource.GetDefaultView(_titleReplacements.ObservableCollection);
                _titleReplacementView.CurrentChanged += TitleReplacement_OnCurrentChanged;
            }

            RaisePropertyChanged("TitleReplacements");
        }

        private void RaiseTitleCommandsCanExecuteChanged()
        {
            TitleMoveUpCommand.RaiseCanExecuteChanged();
            TitleMoveDownCommand.RaiseCanExecuteChanged();
            TitleDeleteCommand.RaiseCanExecuteChanged();
        }

        private void TitleReplacement_OnCurrentChanged(object sender, EventArgs eventArgs)
        {
            RaiseTitleCommandsCanExecuteChanged();
        }

        private bool TitleMoveUpCommandCanExecute(object obj)
        {
            if (_titleReplacementView == null || _titleReplacementView.CurrentItem == null)
                return false;

            return
                _titleReplacements.ObservableCollection.IndexOf(_titleReplacementView.CurrentItem as TitleReplacement) >
                0;
        }

        private void TitleMoveUpCommandExecute(object obj)
        {
            var index = TitleReplacements.IndexOf(_titleReplacementView.CurrentItem as TitleReplacement);
            TitleReplacements.Move(index, index - 1);
        }

        private bool TitleMoveDownCommandCanExecute(object obj)
        {
            if (_titleReplacementView == null || _titleReplacementView.CurrentItem == null)
                return false;

            return
                _titleReplacements.ObservableCollection.IndexOf(_titleReplacementView.CurrentItem as TitleReplacement) <
                (_titleReplacements.ObservableCollection.Count - 1);
        }

        private void TitleMoveDownCommandExecute(object obj)
        {
            var index = TitleReplacements.IndexOf(_titleReplacementView.CurrentItem as TitleReplacement);
            TitleReplacements.Move(index, index + 1);
        }

        private bool TitleDeleteCommandCanExecute(object obj)
        {
            if (_titleReplacementView == null || _titleReplacementView.CurrentItem == null ||
                _titleReplacementView.CurrentItem == CollectionView.NewItemPlaceholder)
                return false;

            return true;
        }

        private void TitleDeleteCommandExecute(object obj)
        {
            TitleReplacements.Remove(_titleReplacementView.CurrentItem as TitleReplacement);
        }

        private void TitleAddCommandExecute(object obj)
        {
            var newItem = new TitleReplacement();
            TitleReplacements.Add(newItem);
            _titleReplacementView.MoveCurrentTo(newItem);
        }

        private void TitleReplacementsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaiseTitleCommandsCanExecuteChanged();
        }
    }
}