namespace Platypus.WriteStore
{
    public interface IWriteStore<TModel, in TKey, in TId>
    {
        void Write(TModel model);
        TModel Get(TKey key);
        TModel Get(TId id);
    }
}