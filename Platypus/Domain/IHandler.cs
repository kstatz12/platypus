using Platypus.Event;

namespace Platypus.Domain
{
    public interface IHandler<in T>
    {
        void Handle(T @event);
    }
}