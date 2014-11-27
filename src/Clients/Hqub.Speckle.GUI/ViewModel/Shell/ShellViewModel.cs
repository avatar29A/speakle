using System.Collections.ObjectModel;
using System.Diagnostics;
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
        private ImageWrapper _selectedImage;
        private int _imageProcessingAmount;
        private object _lock = new object();
        private string _lastProcessingFileName;

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

            var correlationCalcCompleatedEvent = EventAggregator.GetEvent<Events.CorrelationCalculateCompleateEvent>();
            correlationCalcCompleatedEvent.Subscribe(CalculateCorrelationCompleated);

            var etalonLoadedEvent = EventAggregator.GetEvent<Events.EtalonLoadedEvent>();
            etalonLoadedEvent.Subscribe(SetEtalon);
        }

        private void SetEtalon(ImageWrapper etalon)
        {
            Etalaon = etalon;
        }

        private void CalculateCorrelationCompleated(CorrelationCalculateCompleateEventEntity arg)
        {
//            MessageBox.Show("Рассчет закончен!");
        }

        private void ResetStatusBar()
        {
            LastProcessingFileName = string.Empty;
            ImageProcessingAmount = 0;
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

        public ImageWrapper SelectedImage
        {
            get { return _selectedImage; }
            set
            {
                _selectedImage = value;
                OnPropertyChanged(() => SelectedImage);
            }
        }

        /// <summary>
        /// Кол-во обработанных кадров
        /// </summary>
        public int ImageProcessingAmount
        {
            get { return _imageProcessingAmount; }
            set
            {
                _imageProcessingAmount = value;
                OnPropertyChanged(() => ImageProcessingAmount);
            }
        }

        public string LastProcessingFileName
        {
            get
            {
                if (string.IsNullOrEmpty(_lastProcessingFileName))
                    return string.Empty;

                return string.Format("Обработан: {0}", _lastProcessingFileName);
            }
            set
            {
                _lastProcessingFileName = value;
                OnPropertyChanged(() => LastProcessingFileName);
            }
        }

        public string ProgresStatusText
        {
            get { return string.Format("{0} из {1}", ImageProcessingAmount, Experiment.ImageCount); }
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
            SetEtalon(null);
            ResetStatusBar();

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
            var fileDialog = new OpenFileDialog();
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
                }

                return;
            }

            // Запускаем анализ:
            var processing = new Processing.CorrelationProcessing(new SpegoCorrelationEngine());
            processing.Start(Etalaon, Experiment.Images.Where(x => x.IsChecked).ToList());
        }


        private void OnValueCalcuted(CorrelationValue val)
        {
            ++ImageProcessingAmount;
            LastProcessingFileName = val.ImageName;
            OnPropertyChanged(() => ProgresStatusText);
        }

        public ICommand ShowImageCommand
        {
            get { return new DelegateCommand(ShowImageCommandExecute); }
        }

        private void ShowImageCommandExecute()
        {
            if(SelectedImage == null)
                return;

            Process.Start(SelectedImage.Path);
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
