using System;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace Platypus.WriteStore.OrmLite
{
    public abstract class OrmLiteAbstractWriteStore<TModel> : IWriteStore<TModel, Guid, long> where TModel : IWriteModel
    {
        private readonly IDbConnectionFactory _connectionFactory;
        protected OrmLiteAbstractWriteStore(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public void Write(TModel model)
        {
            using (var db = _connectionFactory.Open())
            {
                db.Save<TModel>(model, true);
            }
        }

        public TModel Get(Guid key)
        {
            using(var db = _connectionFactory.Open())
            {
                return db.Single<TModel>(x => x.Key == key);
            }
        }

        public TModel Get(long id)
        {
            using (var db = _connectionFactory.Open())
            {
                return db.Single<TModel>(x => x.Id == id);
            }
        }
    }
}