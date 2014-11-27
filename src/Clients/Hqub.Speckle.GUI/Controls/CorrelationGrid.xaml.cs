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
    using System.Text;

    using Microsoft.Win32;

    using Telerik.Windows.Controls;

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

            var startAnalising = Events.AggregationEventService.Instance.GetEvent<Events.StartNewAnalysisEvent>();
            startAnalising.Subscribe(this.OnStartAnalising);

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

        private void OnStartAnalising(object args)
        {
            this.Reset();
        }

        #region Command

        public ICommand ExportCsvCommand
        {
            get
            {
                return new DelegateCommand(ExportCsvCommandExecute);
            }
        }

        private void ExportCsvCommandExecute(object obj)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "CSV Files (*.csv)|*.csv";
            dialog.FileName = "experiment.csv";

            dialog.FileOk += this.DialogFileOk;

            dialog.ShowDialog();
        }

        private void DialogFileOk(object sender, CancelEventArgs e)
        {
            var dialog = (SaveFileDialog)sender;

            using (var file = System.IO.File.OpenWrite(dialog.FileName))
            {
                foreach (var correlationValue in CorrelationValues)
                {
                    var line = string.Format("{0};{1}\n", correlationValue.ImageName, correlationValue.Value);
                    var bline = Encoding.UTF8.GetBytes(line);

                    file.Write(bline, 0, bline.Length);
                }

                file.Close();
            }
        }

        #endregion

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
