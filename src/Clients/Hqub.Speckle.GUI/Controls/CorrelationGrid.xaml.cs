using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Hqub.Speckle.Core.Model;
using JetBrains.Annotations;

namespace Hqub.Speckle.GUI.Controls
{
    /// <summary>
    /// Interaction logic for CorrelationGrid.xaml
    /// </summary>
    public partial class CorrelationGrid : UserControl, INotifyPropertyChanged
    {
        private ListCollectionView _collections;
        private CorrelationValue _selectedCorrelation;
        private List<CorrelationValue> _correlationValues;

        public CorrelationGrid()
        {
            InitializeComponent();

            SubsribeOnEvents();

            CorrelationValues = new List<CorrelationValue>();
            Collections = new ListCollectionView(CorrelationValues);
            Collections.SortDescriptions.Add(new SortDescription("ImageName", ListSortDirection.Ascending));
        }

        private void SubsribeOnEvents()
        {
            var calculateEvent = Events.AggregationEventService.Instance.GetEvent<Events.CorrelationCalculatedEvent>();
            calculateEvent.Subscribe(OnCalculateCorrelation);

            var experimentCreateEvent =
                Events.AggregationEventService.Instance.GetEvent<Events.ExperimentCreatedEvent>();

            experimentCreateEvent.Subscribe(OnExperimentCreate);
        }

        private void OnCalculateCorrelation(CorrelationValue val)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                CorrelationValues.Add(val);
                Collections.Refresh();
            });
        }

        private void OnExperimentCreate(Model.Events.ExperimentCreateEventEntity entity)
        {
            Reset();   
        }

        private void Reset()
        {
            SelectedCorrelation = null;
            CorrelationValues.Clear();
            Collections.Refresh();
        }

        #region Properties

        public List<CorrelationValue> CorrelationValues
        {
            get { return _correlationValues; }
            set
            {
                _correlationValues = value;
                OnPropertyChanged();
            }
        }

        public ListCollectionView Collections
        {
            get { return _collections; }
            set
            {
                _collections = value;
                OnPropertyChanged();
            }
        }

        public CorrelationValue SelectedCorrelation
        {
            get { return _selectedCorrelation; }
            set
            {
                _selectedCorrelation = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region OnPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(SelectedCorrelation == null)
                return;

            Process.Start(SelectedCorrelation.ImagePath);

        }
    }
}
