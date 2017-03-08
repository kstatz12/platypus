using System;
using System.Collections.Generic;
using Platypus.Event;

namespace Platypus.Domain
{
    public interface IAggregate
    {
        long Id { get; set; }
        Guid Key { get; set; }
        void RestoreFromHistory(List<IEvent> events);
        void ApplyChange(IEvent @event, bool isNew);
    }
}