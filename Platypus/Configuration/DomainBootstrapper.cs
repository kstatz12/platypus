using System;
using System.Collections.Generic;
using Platypus.Domain;
using Platypus.Event;

namespace Platypus.Configuration
{
    public static class DomainBootstrapper
    {
        private static readonly Dictionary<Type, Action<object, object>> Actions = new Dictionary<Type, Action<object, object>>();

        public static void With<TEvent, TAggregate>(Action<TEvent, TAggregate> action) where TEvent : IEvent where TAggregate : IAggregate
        {
            Actions.Add(typeof(TEvent), action.Convert());
        }

        public static Action<IEvent, IAggregate> GetAction(Type eventType)
        {
            foreach (var action in Actions)
            {
                if (action.Key != eventType) continue;
                return action.Value.Convert<IEvent, IAggregate>();
            }
            throw new Exception("No Aggregate Action Found For That Type");
        }

        public static void Clear()
        {
            Actions.Clear();
        }

        private static Action<object, object> Convert<TEvent, TAggregate>(this Action<TEvent, TAggregate> action)
        {
            if (action == null)
            {
                throw new Exception("No Action Found");
            }
            return (e, a) => action((TEvent)e, (TAggregate)a);
        }

        private static Action<TEvent, TAggregate> Convert<TEvent, TAggregate>(this Action<object, object> action) where TEvent : IEvent where TAggregate : IAggregate
        {
            if (action == null)
            {
                throw new Exception("No Action Found");
            }
            return (e, a) => action(e, a);
        }
    }
}