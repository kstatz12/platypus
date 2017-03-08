using System;
using Newtonsoft.Json.Linq;
using Platypus.Event;

namespace Platypus.EventStore
{
    public static class EventResolver
    {
        public static IEvent ResolveEvent(this object input, Type targetType)
        {
            var obj = JObject.Parse(input.ToString());
            return obj.ToObject(targetType) as IEvent;
        }
    }
}