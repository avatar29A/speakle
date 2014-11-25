using Microsoft.Practices.Prism.PubSubEvents;

namespace Hqub.Speckle.GUI.Events
{
    public class AggregationEventService : EventAggregator
    {
        private static AggregationEventService _service;
        public static AggregationEventService Instance
        {
            get { return _service = _service ?? new AggregationEventService(); }
        }

        private AggregationEventService()
        {
            
        }
    }
}
