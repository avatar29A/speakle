using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hqub.Speckle.Core.Model;
using Hqub.Speckle.GUI.Model.Events;

namespace Hqub.Speckle.GUI.Processing
{
    public class CorrelationProcessing
    {
        private readonly Core.ICorrelationEngine _engine;
        private readonly Events.CorrelationCalculatedEvent _calculateEvent;
        private readonly Events.CorrelationCalculateCompleateEvent _calculateCompleateEvent;

        private bool _isStopExperiment;

        public CorrelationProcessing(Core.ICorrelationEngine engine)
        {
            _engine = engine;
            _calculateEvent = Events.AggregationEventService.Instance.GetEvent<Events.CorrelationCalculatedEvent>();
            _calculateCompleateEvent =
                Events.AggregationEventService.Instance.GetEvent<Events.CorrelationCalculateCompleateEvent>();

            var stopExperiment = Events.AggregationEventService.Instance.GetEvent<Events.StopExperimentEvent>();
            stopExperiment.Subscribe(OnStopExperiment);
        }

        public void Start(ImageWrapper etalon, IList<ImageWrapper> images)
        {
            _isStopExperiment = false;

            Task.Factory.StartNew(() => Process(etalon, images));
        }

        private void OnStopExperiment(object args)
        {
            _isStopExperiment = true;
        }

        private void Process(ImageWrapper etalon, IList<ImageWrapper> images)
        {
            var experiment = Core.Experiment.Get();

            if (images == null || images.Count == 0)
            {
                return;
            }

            Parallel.ForEach(images, image =>
            {
                if (_isStopExperiment) return;

                var correlation = _engine.Compare(etalon.Path, image.Path, experiment.WorkAreay);
#if DEBUG
                var time = experiment.StartExperiment.AddSeconds(experiment.Period*image.Number);
                System.Diagnostics.Debug.WriteLine("Correlation: [{2} {3}] {0} - {1}", image.Name, correlation, image.Number, time);
#endif

                _calculateEvent.Publish(new CorrelationValue
                {
                    EtalonePath = etalon.Path,
                    ImagePath = image.Path,
                    ImageName = image.Name,
                    Time = experiment.StartExperiment.AddSeconds(experiment.Period*image.Number),
                    Value = correlation,
                });
            });

            _calculateCompleateEvent.Publish(new CorrelationCalculateCompleateEventEntity());
        }
    }
}
