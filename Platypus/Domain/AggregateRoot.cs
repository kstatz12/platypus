using System;
using System.Collections.Generic;
using Platypus.Configuration;
using Platypus.Event;

namespace Platypus.Domain
{
    public abstract class AggregateRoot : IAggregate
    {
        public long Id { get; set; }
        public Guid Key { get; set; }
        private readonly List<IEvent> _changes;
        private readonly List<IEvent> _publishOnlyEvents;

        protected AggregateRoot()
        {
            _changes = new List<IEvent>();
            _publishOnlyEvents = new List<IEvent>();
        }

        public IEnumerable<IEvent> GetChanges()
        {
            return _changes;
        }

        /// <summary>
        /// Restore the aggregate to current state via historical events
        /// </summary>
        /// <param name="events"></param>
        public void RestoreFromHistory(List<IEvent> events)
        {
            events.Fold(e => ApplyChange(e, false));
        }

        /// <summary>
        /// Use an event to make a change to the state of an aggregate
        /// </summary>
        /// <param name="event">Event with intent to change aggregate</param>
        /// <param name="isNew">if this is a new event, ie not from a rebuild of the aggregate</param>
        public void ApplyChange(IEvent @event, bool isNew)
        {
            DomainBootstrapper.GetAction(@event.GetType()).Invoke(@event, this);
            if(isNew)
                _changes.Add(@event);
        }

        public void AddPublishOnly(IEvent @event)
        {
            _publishOnlyEvents.Add(@event);
        }

        public List<IEvent> GetPublishOnlyEvents()
        {
            return _publishOnlyEvents;
        }
    }
}