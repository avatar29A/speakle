using System.Windows.Controls;

namespace Hqub.Speckle.GUI.Controls
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;

    using Hqub.Speckle.Core.Model;
    using Microsoft.Practices.Prism.PubSubEvents;

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
                            Values = new ObservableCollection<CorrelationValue>(_correlationValues);
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
