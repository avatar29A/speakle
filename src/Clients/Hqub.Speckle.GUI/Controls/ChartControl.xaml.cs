﻿using System.Windows.Controls;

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

    using Hqub.Speckle.Core.Model;
    using Microsoft.Practices.Prism.PubSubEvents;
    using Microsoft.Win32;

    using Telerik.Windows.Controls;

    /// <summary>
    /// Interaction logic for ChartControl.xaml
    /// </summary>
    public partial class ChartControl : UserControl, INotifyPropertyChanged
    {
        private IEventAggregator _eventAggregator;

        private List<CorrelationValue> _correlationValues; 
        private ObservableCollection<CorrelationValue> values;

        public ChartControl()
        {
            InitializeComponent();

            _eventAggregator = Events.AggregationEventService.Instance;
            radChart.DefaultView.ChartLegend.Visibility = Visibility.Collapsed;
            
            Values = new ObservableCollection<CorrelationValue>();
            _correlationValues = new List<CorrelationValue>();
            SubsribeOnEvents();
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

        public ObservableCollection<Core.Model.CorrelationValue> Values
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
        }

        private int _counter = 0;
        private void OnCalculatedCorrelationEvent(Core.Model.CorrelationValue val)
        {
            this.Dispatcher.Invoke(
                () =>
                    {
                        ++_counter;
                        _correlationValues.Add(val);

                        if (_counter % 10 == 0)
                        {
                            _counter = 0;
                            Values = new ObservableCollection<CorrelationValue>(_correlationValues.OrderBy(x => x.Time));
                        }
                    });
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
