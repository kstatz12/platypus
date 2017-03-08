using Newtonsoft.Json;

namespace Platypus.EventStore.OrmLite
{
    public class OrmLiteEventStoreModelHelpers : IEventStoreModelHelpers
    {
        public IEventStoreModel ToEventStoreModel(EventDescriptor input)
        {
            return new OrmLiteEventStoreModel
            {
                Key = input.Key.ToString(),
                SchemaVersion = input.SchemaVersion,
                EventBody = JsonConvert.SerializeObject(input.EventBody),
                EventType = input.Type.FullName
            };
        }
    }
}