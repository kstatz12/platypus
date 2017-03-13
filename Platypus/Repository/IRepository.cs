namespace Platypus.Repository
{
    public interface IRepository<TEntity, in TKey>
    {
        TEntity Get(TKey key);
        void Set(TEntity entity);
    }
}