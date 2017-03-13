using Platypus.Domain;

namespace Platypus.Event
{
    public interface IEventHandler<in TEvent> : IHandler<TEvent> where TEvent : IEvent
    {
        
    }
}