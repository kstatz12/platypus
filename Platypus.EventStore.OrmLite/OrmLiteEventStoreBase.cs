using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Platypus.Domain;
using Platypus.Event;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace Platypus.EventStore.OrmLite
{
    public class OrmLiteEventStoreBase : IEventStore<Guid>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IEventStoreModelHelpers _eventStoreModelHelpers;
        protected OrmLiteEventStoreBase(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _eventStoreModelHelpers = new OrmLiteEventStoreModelHelpers();
        }

        public void SaveEvent(IEvent @event, Guid key, int schemaVersion)
        {
            var descriptor = @event.ToEventDescriptor(key, schemaVersion);
            using (var db = _connectionFactory.Open())
            {
                var model = _eventStoreModelHelpers.ToEventStoreModel(descriptor) as OrmLiteEventStoreModel;
                db.Insert(model);
            }
        }

        public IEnumerable<EventDescriptor> GetEvents(Guid key)
        {
            using (var db = _connectionFactory.Open())
            {
                var models = db.Select<OrmLiteEventStoreModel>(x => x.Key == key.ToString());
                return models.ConvertAll(x => x.ToEventDescriptor());
            }
        }

    }
}