﻿using System.Windows.Controls;
using System.Windows.Threading;
using Hqub.Speckle.Core;

namespace Hqub.Speckle.GUI.Controls
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;

    using Core.Model;
    using Microsoft.Practices.Prism.PubSubEvents;
    using Microsoft.Win32;

    using Telerik.Windows.Controls;

    /// <summary>
    /// Interaction logic for ChartControl.xaml
    /// </summary>
    public partial class ChartControl : UserControl, INotifyPropertyChanged
    {
        private IEventAggregator _eventAggregator;
        private Core.Experiment _experiment;

        private List<CorrelationValue> _correlationValues; 
        private ObservableCollection<CorrelationValue> values;

        public ChartControl()
        {
            InitializeComponent();

            _eventAggregator = Events.AggregationEventService.Instance;
            radChart.DefaultView.ChartLegend.Visibility = Visibility.Collapsed;
            _experiment = Core.Experiment.Get();
            
            Values = new ObservableCollection<CorrelationValue>();
            _correlationValues = new List<CorrelationValue>();

            SetupChartAxis();

            SubsribeOnEvents();
        }

        private void SetupChartAxis()
        {
            if (_experiment.CurrentEngineName == StaticVariable.SignalLeveAlgCode)
                SetupCharAxisToBright();
            else
                SetupCharToCorrelation();
        }

        private void SetupCharToCorrelation(bool isMinus = false)
        {
            radChart.DefaultView.ChartArea.AxisY.MinValue = isMinus ? -1 : 0;
            radChart.DefaultView.ChartArea.AxisY.MaxValue = 1;
            radChart.DefaultView.ChartArea.AxisY.Step = 0.10;
            radChart.DefaultView.ChartArea.AxisY.AutoRange = false;
            radChart.DefaultView.ChartArea.EnableAnimations = false;
        }

        private void SetupCharAxisToBright()
        {
            radChart.DefaultView.ChartArea.AxisY.MinValue = 0;
            radChart.DefaultView.ChartArea.AxisY.MaxValue = 260;
            radChart.DefaultView.ChartArea.AxisY.Step = 10;
            radChart.DefaultView.ChartArea.AxisY.AutoRange = false;
            radChart.DefaultView.ChartArea.EnableAnimations = false;
        }

        #region Commands

        #region Export Chart Commadns

        public ICommand ExportXlsCommand
        {
            get { return new DelegateCommand(ExportXlsCommandExecute); }
        }

        private void ExportXlsCommandExecute(object obj)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = "*.xls";
            dialog.Filter = "Files(*.xls)|*.xls";
            if (!(bool)dialog.ShowDialog()) return;
            Stream fileStream = dialog.OpenFile();
            radChart.ExportToExcelML(fileStream);
            fileStream.Close();
        }

        public ICommand ExportPngCommand
        {
            get { return new DelegateCommand(ExportPngCommandExecute); }
        }

        private void ExportPngCommandExecute(object obj)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = "*.png";
            dialog.Filter = "Files(*.png)|*.png";
            if (!(bool)dialog.ShowDialog()) return;
            Stream fileStream = dialog.OpenFile();
            radChart.ExportToImage(fileStream, new PngBitmapEncoder());
            fileStream.Close();
        }

        #endregion

        #endregion

        #region Properties

        public ObservableCollection<CorrelationValue> Values
        {
            get
            {
                return this.values;
            }
            set
            {
                this.values = value;
                this.OnPropertyChanged();
            }
        }

        #endregion

        private void SubsribeOnEvents()
        {
            var correlationEvent = _eventAggregator.GetEvent<Events.CorrelationCalculatedEvent>();
            correlationEvent.Subscribe(OnCalculatedCorrelationEvent);

            var startAnalising = _eventAggregator.GetEvent<Events.StartNewAnalysisEvent>();
            startAnalising.Subscribe(this.OnStartAnalising);

            var compleate = _eventAggregator.GetEvent<Events.CorrelationCalculateCompleateEvent>();
            compleate.Subscribe(this.OnCompleate);

            var createNewExperiment = _eventAggregator.GetEvent<Events.ExperimentCreatedEvent>();
            createNewExperiment.Subscribe(this.OnStartAnalising);
        }

        private void OnStartAnalising(object args)
        {
            Values.Clear();
            _correlationValues.Clear();

            SetupChartAxis();
        }

        private void OnCompleate(object e)
        {
            Values = new ObservableCollection<CorrelationValue>(_correlationValues.OrderBy(x => x.Time));
        }

        private int _counter;

        private void OnCalculatedCorrelationEvent(CorrelationValue val)
        {
            Dispatcher.Invoke(
                () =>
                {
                    ++_counter;
                    _correlationValues.Add(val);

                    if (_counter%10 == 0)
                    {
                        _counter = 0;

                        // Если есть отрицательные значения корреляции меняет оси с (0 до 1) на (-1 до 1):
                        if (val.Value < 0)
                            SetupCharToCorrelation(true);

                        Values = new ObservableCollection<CorrelationValue>(_correlationValues.OrderBy(x => x.Time));
                    }

                }, DispatcherPriority.Background);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
