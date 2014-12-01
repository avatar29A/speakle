using System.Collections.Generic;
using System.Threading;
using Hqub.Speckle.Core.Model;
using Hqub.Speckle.GUI.Model.Events;

namespace Hqub.Speckle.GUI.Processing
{
    public class CorrelationProcessing
    {
        private readonly Core.ICorrelationEngine _engine;
        private readonly Events.CorrelationCalculatedEvent _calculateEvent;
        private readonly Events.CorrelationCalculateCompleateEvent _calculateCompleateEvent;
        private readonly object _lock = new object();
        private Queue<ImageWrapper> _imageQueue;

        #region Theading

        private const int ThreadAmount = 1;
        private int BatchSize = 0;

        private List<ManualResetEventSlim> _lockThreads = new List<ManualResetEventSlim>();

        #endregion

        public CorrelationProcessing(Core.ICorrelationEngine engine)
        {
            _engine = engine;
            _calculateEvent = Events.AggregationEventService.Instance.GetEvent<Events.CorrelationCalculatedEvent>();
            _calculateCompleateEvent =
                Events.AggregationEventService.Instance.GetEvent<Events.CorrelationCalculateCompleateEvent>();

            InitThreadPool();
        }

        public void Start(ImageWrapper etalon, IList<ImageWrapper> images)
        {
            BatchSize = CalcBatchSize(images.Count);

            ThreadPool.QueueUserWorkItem((arg) =>
            {
                _imageQueue = new Queue<ImageWrapper>(images);

                List<ImageWrapper> poolImages = null;
                do
                {
                    for (var i = 0; i < ThreadAmount; i++)
                    {
                        poolImages = GetItemsFormQueue();

                        var copyPoolImages = poolImages;
                        var mre = _lockThreads[i];

                        new Thread(() => Process(etalon, copyPoolImages, mre)).Start();
                    }

                    WaitAll(_lockThreads);

                } while (poolImages != null && poolImages.Count != 0);

                _calculateCompleateEvent.Publish(new CorrelationCalculateCompleateEventEntity());
            });
        }

        private int CalcBatchSize(int count)
        {
            if (count <= ThreadAmount)
                return count;

            return count/ThreadAmount;
        }

        private void WaitAll(IEnumerable<ManualResetEventSlim> threads)
        {
            foreach (var mre in threads)
            {
                mre.Wait();
            }
        }

        private void InitThreadPool()
        {
            for (int i = 0; i < ThreadAmount; i++)
            {
                var l = new ManualResetEventSlim(false);
                l.Reset();

                _lockThreads.Add(l);
                
            }
        }


        private List<ImageWrapper> GetItemsFormQueue()
        {
            lock (_lock)
            {
                //TODO: Проверить как влияет на скоростьинициализация листа с заданным кол-м элементов
                var pool = new List<ImageWrapper>(BatchSize*2);

                for (var i = 0; i < BatchSize; i++)
                {
                    if (_imageQueue.Count != 0)
                    {
                        pool.Add(_imageQueue.Dequeue());
                    }
                    else
                    {
                        break;
                    }
                }

                return pool;
            }
        }

        private void Process(ImageWrapper etalon, List<ImageWrapper> images, ManualResetEventSlim mre)
        {
            var experiment = Core.Experiment.Get();

            if (images == null || images.Count == 0)
            {
                mre.Set();
                return;
            }

            foreach (var image in images)
            {
                var correlation = _engine.Compare(etalon.Path, image.Path, experiment.WorkAreay);
                _calculateEvent.Publish(new CorrelationValue
                {
                    EtalonePath = etalon.Path,
                    ImagePath = image.Path,
                    ImageName = image.Name,
                    Time = experiment.StartExperiment.AddSeconds(experiment.Period * image.Number),
                    Value = correlation,
                });

                mre.Set();
            }
        }
    }
}
