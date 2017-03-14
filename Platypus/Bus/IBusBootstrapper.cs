using System.Collections.Generic;
using Platypus.Command;
using Platypus.Event;

namespace Platypus.Bus
{
    public interface IBusBootstrapper
    {
        IBus Bus { get; }
        IBusBootstrapper Init(IBusHost host);
        IBus Start();
        IBusBootstrapper WithCommandHandler<T>(ICommandHandler<T> handler, Dictionary<string, object> args = null) where T : ICommand;
        IBusBootstrapper WithEventHandler<T>(IEventHandler<T> handler, Dictionary<string, object> args = null) where T : IEvent;
    }
}