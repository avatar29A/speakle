using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Hqub.Speckle.Core.Model;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Win32;

namespace Hqub.Speckle.GUI.ViewModel.Shell
{
    public class ShellViewModel : BaseViewModel
    {
        private Core.Experiment _experiment;

        public ShellViewModel()
        {
            _experiment = Core.Experiment.Get();
        }

        #region Properties

        public Core.Experiment Experiment
        {
            get { return _experiment; }
            set
            {
                _experiment = value;
                RaisePropertyChanged("Experiment");
            }
        }

        #endregion

        #region Commands

        public ICommand CreateExpirementCommand
        {
            get { return new DelegateCommand(CreateExpirementCommandExecute); }
        }

        private void CreateExpirementCommandExecute()
        {
            Experiment = Core.Experiment.Get(true);
        }

        public ICommand LoadExpirementFilesCommand
        {
            get { return new DelegateCommand(LoadExpirementFilesExecute); }
        }

        private void LoadExpirementFilesExecute()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Filter = "Image Files (*.bmp, *.jpg)|*.bmp;*.jpg|All Files (*.*)|*.*";
            var result = fileDialog.ShowDialog();

            if (result != true)
                return;

            Experiment.Images =
                new ObservableCollection<ImageWrapper>(fileDialog.FileNames.Select(f => new ImageWrapper(f)));
        }

        #region Checked Unchecked commands

        public ICommand CheckAllItemsCommand
        {
            get { return new DelegateCommand(CheckAllItemsCommandExecute); }
        }

        private void CheckAllItemsCommandExecute()
        {
            CheckUncheckAllItems(true);
        }

        public ICommand UnCheckAllItemsCommand
        {
            get { return new DelegateCommand(UnCheckAllItemsCommandExecute); }
        }

        private void UnCheckAllItemsCommandExecute()
        {
            CheckUncheckAllItems(false);
        }

        private void CheckUncheckAllItems(bool val)
        {
            foreach (var image in Experiment.Images)
            {
                image.IsChecked = val;
            }
        }

        #endregion

        #endregion
    }
}
