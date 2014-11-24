using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Input;
using Hqub.Speckle.Core.Model;
using Hqub.Speckle.GUI.Model;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Win32;

namespace Hqub.Speckle.GUI.ViewModel.Shell
{
    public class ShellViewModel : BaseViewModel
    {
        private readonly Core.Experiment _experiment;

        public ShellViewModel()
        {
            _experiment = Core.Experiment.Get();
        }

        #region Properties

        public Core.Experiment Experiment
        {
            get { return _experiment; }
        }

        #endregion

        #region Commands

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

            if(result != true)
                return;

            Experiment.Images = new ObservableCollection<ImageWrapper>(fileDialog.FileNames.Select(f => new ImageWrapper(f)));
        }

        public ICommand CreateExpirementCommand
        {
            get { return new DelegateCommand(CreateExpirementCommandExecute); }
        }

        private void CreateExpirementCommandExecute()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
