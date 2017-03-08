namespace Platypus.Event
{
    public interface IHandler<in TEvent> where TEvent : IEvent
    {
        void Handle(TEvent @event);
    }
}