using System;
using System.Collections.Generic;
using Platypus.Command;
using Platypus.Event;

namespace Platypus.Bus
{
    public interface IBus : IDisposable
    {
        void Start();
        void Publish<T>(T @event, Dictionary<string, object> args = null) where T : IEvent;
        void Send<T>(T command, Dictionary<string, object> args = null) where T : ICommand;
        void RegisterHandler<T>(Action<T> action, Dictionary<string, object> args = null);
    }
}