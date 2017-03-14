using System.Collections.Generic;

namespace Platypus.ReadStore
{
    public interface IReadStore<TModel> where TModel : IReadModel
    {
        TModel Get(string key);
        TModel Get(int id);
        IEnumerable<TModel> Get(int skip, int take);
        void Save(TModel entity);
    }
}