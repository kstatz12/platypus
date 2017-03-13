using System;
using System.Linq;
using Platypus.Domain;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace Platypus.WriteStore.OrmLite
{
    public abstract class OrmLiteWriteStore<TModel> : IWriteStore<TModel,Guid,long> where TModel : AggregateRoot
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        protected OrmLiteWriteStore(IDbConnectionFactory connectionFactory)
        {
            _dbConnectionFactory = connectionFactory;
        }
        public void Write(TModel model)
        {
            using (var db = _dbConnectionFactory.Open())
            {
                db.Save(model, true);
            }
        }

        public TModel Get(Guid key)
        {
            using(var db = _dbConnectionFactory.Open())
            {
                return db.Select<TModel>(x => x.Key == key).FirstOrDefault();
            }
        }

        public TModel Get(long id)
        {
            using (var db = _dbConnectionFactory.Open())
            {
                return db.Select<TModel>(x => x.Id == id).FirstOrDefault();
            }
        }
    }
}