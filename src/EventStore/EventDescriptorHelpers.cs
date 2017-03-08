using System;
using Newtonsoft.Json;
using Platypus.Event;

namespace Platypus.EventStore
{
    public static class EventDescriptorHelpers
    {
        public static EventDescriptor ToEventDescriptor(this IEvent @event, Guid key, int schemaVersion)
        {
            return new EventDescriptor
            {
                Key = key,
                SchemaVersion = schemaVersion,
                Type = @event.GetType(),
                EventBody = @event
            };
        }

        public static EventDescriptor ToEventDescriptor(this IEventStoreModel model)
        {
            var eventType = Type.GetType(model.EventBody);
            return new EventDescriptor
            {
                Key = Guid.Parse(model.Key),
                SchemaVersion = model.SchemaVersion,
                Type = eventType,
                EventBody = JsonConvert.DeserializeObject(model.EventBody).ResolveEvent(eventType)
            };
        }
    }
}