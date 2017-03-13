using System;
using System.Collections.Generic;
using Platypus.Event;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace Platypus.EventStore.OrmLite
{
    public abstract class OrmLiteEventStore : IEventStore<Guid>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IEventStoreModelHelpers _eventStoreModelHelpers;
        protected OrmLiteEventStore(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _eventStoreModelHelpers = new OrmLiteEventStoreModelHelpers();
        }

        public virtual void SaveEvent(IEvent @event, Guid key, int schemaVersion)
        {
            var descriptor = @event.ToEventDescriptor(key, schemaVersion);
            using (var db = _connectionFactory.Open())
            {
                var model = _eventStoreModelHelpers.ToEventStoreModel(descriptor) as OrmLiteEventStoreModel;
                db.Insert(model);
            }
        }

        public virtual IEnumerable<EventDescriptor> GetEvents(Guid key)
        {
            using (var db = _connectionFactory.Open())
            {
                var models = db.Select<OrmLiteEventStoreModel>(x => x.Key == key.ToString());
                return models.ConvertAll(x => x.ToEventDescriptor());
            }
        }

    }
}