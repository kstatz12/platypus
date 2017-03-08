using System;
using Platypus.Event;

namespace Platypus.EventStore
{
    public class EventDescriptor
    {
        public Guid Key { get; set; }
        public int SchemaVersion { get; set; }
        public Type Type { get; set; }
        public IEvent EventBody { get; set; }
    }
}