using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Hqub.Speckle.Core.Correlation;
using Hqub.Speckle.Core.Model;
using Hqub.Speckle.GUI.Model.Events;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Win32;

namespace Hqub.Speckle.GUI.ViewModel.Shell
{
    public class ShellViewModel : BaseViewModel
    {
        private Core.Experiment _experiment;

        public ShellViewModel()
        {
            _experiment = Core.Experiment.Get();
            EventAggregator = Events.AggregationEventService.Instance;

            // Подписываемся на событие обработки:
            SubsribeOnEvents();
        }

        private void SubsribeOnEvents()
        {
            var correlationCalcEvent = EventAggregator.GetEvent<Events.CorrelationCalculatedEvent>();
            correlationCalcEvent.Subscribe(OnValueCalcuted);

            var etalonLoadedEvent = EventAggregator.GetEvent<Events.EtalonLoadedEvent>();
            etalonLoadedEvent.Subscribe(SetEtalon);
        }

        private void SetEtalon(ImageWrapper etalon)
        {
            Etalaon = etalon;
        }

        #region Properties

        public Core.Experiment Experiment
        {
            get { return _experiment; }
            set
            {
                _experiment = value;
                OnPropertyChanged(() => Experiment);
            }
        }

        public ImageWrapper Etalaon { get; set; }

        public IEventAggregator EventAggregator { get; set; }

        #endregion

        #region Commands

        public ICommand CreateExpirementCommand
        {
            get { return new DelegateCommand(CreateExpirementCommandExecute); }
        }

        private void CreateExpirementCommandExecute()
        {
            Experiment = Core.Experiment.Get(true);

            // Рассылаем всем модуляем, что создали новый эксперимент:
            var eventInstance = EventAggregator.GetEvent<Events.ExperimentCreatedEvent>();
            eventInstance.Publish(new ExperimentCreateEventEntity());
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

        public ICommand RunAnalysingCommand
        {
            get { return new DelegateCommand(RunAnalysingCommandExecute); }
        }

        private void RunAnalysingCommandExecute()
        {
            if (Etalaon == null)
            {
                MessageBox.Show("Для начала анализа требуется загрузить эталонной кадр.", "Эталон не загружен",
                    MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            if (Experiment.Images == null || Experiment.Images.Count == 0)
            {
                if (
                    MessageBox.Show("Для проведения анализа требуется загрузить кадры. Желаете загрузить сейчас?",
                        "Загрузка кадров",
                        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    LoadExpirementFilesExecute();
                    return;
                }
            }

            // Запускаем анализ:
            var processing = new Processing.CorrelationProcessing(new SpegoCorrelationEngine());
            processing.Start(Etalaon, Experiment.Images);
        }

        private void OnValueCalcuted(CorrelationValue val)
        {
            System.Diagnostics.Debug.WriteLine("Correlation: {0} = {1}", val.ImageName, val.Value);
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
