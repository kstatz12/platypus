using System.Collections.Generic;
using Platypus.Domain;
using Platypus.Event;

namespace Platypus.EventStore
{
    public interface IEventStore<in TKey>
    {
        void SaveEvent(IEvent @event, TKey key, int schemaVersion);
        IEnumerable<EventDescriptor> GetEvents(TKey key);
    }
}