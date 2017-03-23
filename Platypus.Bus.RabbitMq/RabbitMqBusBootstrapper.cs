using System.Collections.Generic;
using Platypus.Command;
using Platypus.Event;

namespace Platypus.Bus.RabbitMq
{
    public class RabbitMqBusBootstrapper : IBusBootstrapper
    {
        public IBus Bus { get; private set; }
        public IBusBootstrapper Init(IBusHost host)
        {
            Bus = new RabbitMqBus(host);
            return this;
        }

        public IBus Start()
        {
            Bus.Start();
            return Bus;
        }

        public IBusBootstrapper WithCommandHandler<T>(ICommandHandler<T> handler, Dictionary<string, object> args = null) where T : ICommand
        {
            Bus.RegisterHandler<T>(handler.Handle, args);
            return this;
        }

        public IBusBootstrapper WithEventHandler<T>(IEventHandler<T> handler, Dictionary<string, object> args = null) where T : IEvent
        {
            Bus.RegisterHandler<T>(handler.Handle, args);
            return this;
        }
    }
}