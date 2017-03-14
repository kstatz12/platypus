using System.Collections.Generic;
using System.Linq;
using Raven.Client.Document;

namespace Platypus.ReadStore.RavenDb
{
    public abstract class ReadStoreBase<TModel> : IReadStore<TModel> where TModel : IReadModel
    {
        private readonly DocumentStore _documentStore;
        private readonly string _databaseName;
        protected ReadStoreBase(DocumentStore documentStore, string databaseName)
        {
            _documentStore = documentStore;
            _databaseName = databaseName;
        }

        public virtual TModel Get(string key)
        {
            using (var session = _documentStore.OpenSession(_databaseName))
            {
                return session.Query<TModel>().FirstOrDefault(x => x.Key == key);
            }
        }

        public virtual IEnumerable<TModel> Get(int skip, int take)
        {
            using (var session = _documentStore.OpenSession(_databaseName))
            {
                return session.Query<TModel>().Skip(skip).Take(take);
            }
        }

        public virtual void Save(TModel entity)
        {
            using (var session = _documentStore.OpenSession(_databaseName))
            {
                session.Store(entity);
            }
        }

        public TModel Get(int id)
        {
            using (var session = _documentStore.OpenSession(_databaseName))
            {
                return session.Load<TModel>(id);
            }
        }
    }
}