using System;
using Newtonsoft.Json.Linq;
using Platypus.Event;

namespace Platypus.EventStore
{
    public static class EventResolver
    {
        public static IEvent ResolveEvent(this object input, Type targetType)
        {
            return JObject.Parse(input.ToString()).ToObject(targetType) as IEvent;
        }
    }
}