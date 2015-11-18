using System.Linq;
using System.Windows.Data;
using NUnit.Framework;
using pdfforge.PDFCreator.Core.Settings;
using pdfforge.PDFCreator.ViewModels.UserControls;

namespace PDFCreator.Test.ViewModels.UserControls
{
    [TestFixture]
    class TitleTabViewModelTest
    {
        #region TitleMoveUpCommand
        [Test]
        public void TitleReplacementMoveUp_WithNullList_CannotExecute()
        {
            var titleTabViewModel = new TitleTabViewModel();

            Assert.IsFalse(titleTabViewModel.TitleMoveUpCommand.CanExecute(null));
        }

        [Test]
        public void TitleReplacementMoveUp_WithEmptyList_CannotExecute()
        {
            var settings = new ApplicationSettings();
            settings.TitleReplacement = new TitleReplacement[] { };

            var titleTabViewModel = new TitleTabViewModel();
            titleTabViewModel.ApplicationSettings = settings;

            Assert.IsFalse(titleTabViewModel.TitleMoveUpCommand.CanExecute(null));
        }

        [Test]
        public void TitleReplacementMoveUp_WithFirstElement_CannotExecute()
        {
            var settings = new ApplicationSettings();
            settings.TitleReplacement = new[] { new TitleReplacement("a", "b"), new TitleReplacement("c", "d") };

            var titleTabViewModel = new TitleTabViewModel();
            titleTabViewModel.ApplicationSettings = settings;

            var cv = CollectionViewSource.GetDefaultView(titleTabViewModel.TitleReplacements);
            cv.MoveCurrentTo(titleTabViewModel.TitleReplacements[0]);

            Assert.IsFalse(titleTabViewModel.TitleMoveUpCommand.CanExecute(null));
        }

        [Test]
        public void TitleReplacementMoveUp_WithSecondElement_CanExecute()
        {
            var settings = new ApplicationSettings();
            settings.TitleReplacement = new[] { new TitleReplacement("a", "b"), new TitleReplacement("c", "d") };

            var titleTabViewModel = new TitleTabViewModel();
            titleTabViewModel.ApplicationSettings = settings;

            var cv = CollectionViewSource.GetDefaultView(titleTabViewModel.TitleReplacements);
            cv.MoveCurrentTo(titleTabViewModel.TitleReplacements[1]);

            Assert.IsTrue(titleTabViewModel.TitleMoveUpCommand.CanExecute(null));
        }

        [Test]
        public void TitleReplacementMoveUp_WithSecondElement_MovesToFirst()
        {
            var settings = new ApplicationSettings();
            settings.TitleReplacement = new[] { new TitleReplacement("a", "b"), new TitleReplacement("c", "d") }.ToList();

            var titleTabViewModel = new TitleTabViewModel();
            titleTabViewModel.ApplicationSettings = settings;

            var cv = CollectionViewSource.GetDefaultView(titleTabViewModel.TitleReplacements);
            cv.MoveCurrentTo(titleTabViewModel.TitleReplacements[1]);

            titleTabViewModel.TitleMoveUpCommand.Execute(null);

            Assert.AreEqual("c", settings.TitleReplacement[0].Search);
        }

        [Test]
        public void TitleReplacementMoveUp_CurrentItemChanged_RaisesCommandCanExecuteChanged()
        {
            bool wasRaised = false;
            var settings = new ApplicationSettings();
            settings.TitleReplacement = new[] { new TitleReplacement("a", "b"), new TitleReplacement("c", "d") }.ToList();

            var titleTabViewModel = new TitleTabViewModel();
            titleTabViewModel.ApplicationSettings = settings;

            titleTabViewModel.TitleMoveUpCommand.CanExecuteChanged += (sender, args) => wasRaised = true;

            var cv = CollectionViewSource.GetDefaultView(titleTabViewModel.TitleReplacements);
            cv.MoveCurrentToLast();

            Assert.IsTrue(wasRaised);
        }

        [Test]
        public void TitleReplacementMoveUp_WithSecondElement_RaisesCommandCanExecuteChanged()
        {
            bool wasRaised = false;
            var settings = new ApplicationSettings();
            settings.TitleReplacement = new[] { new TitleReplacement("a", "b"), new TitleReplacement("c", "d") }.ToList();

            var titleTabViewModel = new TitleTabViewModel();
            titleTabViewModel.ApplicationSettings = settings;

            var cv = CollectionViewSource.GetDefaultView(titleTabViewModel.TitleReplacements);
            cv.MoveCurrentToLast();

            titleTabViewModel.TitleMoveUpCommand.CanExecuteChanged += (sender, args) => wasRaised = true;

            titleTabViewModel.TitleMoveUpCommand.Execute(null);

            Assert.IsTrue(wasRaised);
        }
        #endregion TitleMoveUpCommand
        
        #region TitleMoveDownCommand
        [Test]
        public void TitleReplacementMoveDown_WithNullList_CannotExecute()
        {
            var titleTabViewModel = new TitleTabViewModel();

            Assert.IsFalse(titleTabViewModel.TitleMoveDownCommand.CanExecute(null));
        }

        [Test]
        public void TitleReplacementMoveDown_WithEmptyList_CannotExecute()
        {
            var settings = new ApplicationSettings();
            settings.TitleReplacement = new TitleReplacement[] { };

            var titleTabViewModel = new TitleTabViewModel();
            titleTabViewModel.ApplicationSettings = settings;

            Assert.IsFalse(titleTabViewModel.TitleMoveDownCommand.CanExecute(null));
        }

        [Test]
        public void TitleReplacementMoveDown_WithLastElement_CannotExecute()
        {
            var settings = new ApplicationSettings();
            settings.TitleReplacement = new[] { new TitleReplacement("a", "b"), new TitleReplacement("c", "d") };

            var titleTabViewModel = new TitleTabViewModel();
            titleTabViewModel.ApplicationSettings = settings;

            var cv = CollectionViewSource.GetDefaultView(titleTabViewModel.TitleReplacements);
            cv.MoveCurrentToLast();

            Assert.IsFalse(titleTabViewModel.TitleMoveDownCommand.CanExecute(null));
        }

        [Test]
        public void TitleReplacementMoveDown_WithFirstElement_CanExecute()
        {
            var settings = new ApplicationSettings();
            settings.TitleReplacement = new[] { new TitleReplacement("a", "b"), new TitleReplacement("c", "d") };

            var titleTabViewModel = new TitleTabViewModel();
            titleTabViewModel.ApplicationSettings = settings;

            var cv = CollectionViewSource.GetDefaultView(titleTabViewModel.TitleReplacements);
            cv.MoveCurrentToFirst();

            Assert.IsTrue(titleTabViewModel.TitleMoveDownCommand.CanExecute(null));
        }

        [Test]
        public void TitleReplacementMoveDown_WithFirstElement_MovesToSecond()
        {
            var settings = new ApplicationSettings();
            settings.TitleReplacement = new[] { new TitleReplacement("a", "b"), new TitleReplacement("c", "d") }.ToList();

            var titleTabViewModel = new TitleTabViewModel();
            titleTabViewModel.ApplicationSettings = settings;

            var cv = CollectionViewSource.GetDefaultView(titleTabViewModel.TitleReplacements);
            cv.MoveCurrentToFirst();

            titleTabViewModel.TitleMoveDownCommand.Execute(null);

            Assert.AreEqual("a", settings.TitleReplacement[1].Search);
        }

        [Test]
        public void TitleReplacementMoveDown_CurrentItemChanged_RaisesCommandCanExecuteChanged()
        {
            bool wasRaised = false;
            var settings = new ApplicationSettings();
            settings.TitleReplacement = new[] { new TitleReplacement("a", "b"), new TitleReplacement("c", "d") }.ToList();
            var titleTabViewModel = new TitleTabViewModel();
            titleTabViewModel.ApplicationSettings = settings;

            titleTabViewModel.TitleMoveDownCommand.CanExecuteChanged += (sender, args) => wasRaised = true;

            var cv = CollectionViewSource.GetDefaultView(titleTabViewModel.TitleReplacements);
            cv.MoveCurrentToLast();

            Assert.IsTrue(wasRaised);
        }

        [Test]
        public void TitleReplacementMoveDown_WithFirstElement_RaisesCommandCanExecuteChanged()
        {
            bool wasRaised = false;
            var settings = new ApplicationSettings();
            settings.TitleReplacement = new[] { new TitleReplacement("a", "b"), new TitleReplacement("c", "d") }.ToList();

            var titleTabViewModel = new TitleTabViewModel();
            titleTabViewModel.ApplicationSettings = settings;

            var cv = CollectionViewSource.GetDefaultView(titleTabViewModel.TitleReplacements);
            cv.MoveCurrentToFirst();

            titleTabViewModel.TitleMoveDownCommand.CanExecuteChanged += (sender, args) => wasRaised = true;

            titleTabViewModel.TitleMoveDownCommand.Execute(null);

            Assert.IsTrue(wasRaised);
        }

        #endregion TitleMoveDownCommand

        #region TitleAddCommand

        [Test]
        public void TitleReplacementAdd_WithEmptyList_CanExecute()
        {
            var settings = new ApplicationSettings();
            settings.TitleReplacement = new TitleReplacement[] { };

            var titleTabViewModel = new TitleTabViewModel();
            titleTabViewModel.ApplicationSettings = settings;

            Assert.IsTrue(titleTabViewModel.TitleAddCommand.CanExecute(null));
        }

        [Test]
        public void TitleReplacementAdd_WithEmptyList_InsertsOneElement()
        {
            var settings = new ApplicationSettings();
            settings.TitleReplacement = new TitleReplacement[] { }.ToList();

            var titleTabViewModel = new TitleTabViewModel();
            titleTabViewModel.ApplicationSettings = settings;

            titleTabViewModel.TitleAddCommand.Execute(null);

            Assert.AreEqual(1, settings.TitleReplacement.Count);
        }

        [Test]
        public void TitleReplacementAdd_WithNonEmptyList_NewElementIsCurrent()
        {
            var settings = new ApplicationSettings();
            settings.TitleReplacement = new[] { new TitleReplacement("a", "b"), new TitleReplacement("c", "d") }.ToList();

            var titleTabViewModel = new TitleTabViewModel();
            titleTabViewModel.ApplicationSettings = settings;
            var cv = CollectionViewSource.GetDefaultView(titleTabViewModel.TitleReplacements);

            titleTabViewModel.TitleAddCommand.Execute(null);

            Assert.AreEqual(2, cv.CurrentPosition);
        }

        #endregion

        #region TitleDeleteCommand

        [Test]
        public void TitleReplacementDelete_WithNullList_CannotExecute()
        {
            var titleTabViewModel = new TitleTabViewModel();

            Assert.IsFalse(titleTabViewModel.TitleDeleteCommand.CanExecute(null));
        }

        [Test]
        public void TitleReplacementDelete_WithEmptyList_CannotExecute()
        {
            var settings = new ApplicationSettings();
            settings.TitleReplacement = new TitleReplacement[] { };

            var titleTabViewModel = new TitleTabViewModel();
            titleTabViewModel.ApplicationSettings = settings;

            Assert.IsFalse(titleTabViewModel.TitleDeleteCommand.CanExecute(null));
        }

        [Test]
        public void TitleReplacementDelete_WithNonEmptyList_CanExecute()
        {
            var settings = new ApplicationSettings();
            settings.TitleReplacement = new[] { new TitleReplacement("a", "b"), new TitleReplacement("c", "d") }.ToList();

            var titleTabViewModel = new TitleTabViewModel();
            titleTabViewModel.ApplicationSettings = settings;

            Assert.IsTrue(titleTabViewModel.TitleDeleteCommand.CanExecute(null));
        }

        [Test]
        public void TitleReplacementDelete_AfterExecute_ElementIsRemoved()
        {
            var settings = new ApplicationSettings();
            settings.TitleReplacement = new[] { new TitleReplacement("a", "b"), new TitleReplacement("c", "d") }.ToList();

            var titleTabViewModel = new TitleTabViewModel();
            titleTabViewModel.ApplicationSettings = settings;

            var deletedElement = settings.TitleReplacement[0];

            titleTabViewModel.TitleDeleteCommand.Execute(null);

            Assert.IsFalse(titleTabViewModel.TitleReplacements.Contains(deletedElement));
        }

        [Test]
        public void TitleReplacementDelete_AfterExecute_OtherElementStillThere()
        {
            var settings = new ApplicationSettings();
            settings.TitleReplacement = new[] { new TitleReplacement("a", "b"), new TitleReplacement("c", "d") }.ToList();

            var titleTabViewModel = new TitleTabViewModel();
            titleTabViewModel.ApplicationSettings = settings;

            var otherElement = settings.TitleReplacement[1];

            titleTabViewModel.TitleDeleteCommand.Execute(null);

            Assert.IsTrue(titleTabViewModel.TitleReplacements.Contains(otherElement));
        }

        [Test]
        public void TitleReplacementDelete_CurrentItemChanged_RaisesCommandCanExecuteChanged()
        {
            bool wasRaised = false;
            var settings = new ApplicationSettings();
            settings.TitleReplacement = new[] { new TitleReplacement("a", "b"), new TitleReplacement("c", "d") }.ToList();

            var titleTabViewModel = new TitleTabViewModel();
            titleTabViewModel.ApplicationSettings = settings;

            titleTabViewModel.TitleDeleteCommand.CanExecuteChanged += (sender, args) => wasRaised = true;

            var cv = CollectionViewSource.GetDefaultView(titleTabViewModel.TitleReplacements);
            cv.MoveCurrentToLast();

            Assert.IsTrue(wasRaised);
        }

        #endregion
    
    }
}
