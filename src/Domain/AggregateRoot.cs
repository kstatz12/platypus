using System;
using System.Collections.Generic;
using System.Linq;
using Platypus.Configuration;
using Platypus.Event;

namespace Platypus.Domain
{
    public abstract class AggregateRoot : IAggregate
    {
        public virtual Guid Key { get; set; }
        private readonly List<IEvent> _changes;

        protected AggregateRoot()
        {
            _changes = new List<IEvent>();
        }

        public IEnumerable<IEvent> GetChanges()
        {
            return _changes;
        }

        public void RestoreFromHistory(List<IEvent> events)
        {
            foreach (var @event in events.OrderBy(x => x.Version))
            {
                ApplyChange(@event, false);
            }
        }

        /// <summary>
        /// Use an event to make a change to the state of an aggregate
        /// </summary>
        /// <param name="event">Event with intent to change aggregate</param>
        /// <param name="isNew">if this is a new event, ie not from a rebuild of the aggregate</param>
        public virtual void ApplyChange(IEvent @event, bool isNew)
        {
            DomainBootstrapper.GetAction(@event.GetType()).Invoke(@event, this);
            if(isNew)
                _changes.Add(@event);
        }
    }
}