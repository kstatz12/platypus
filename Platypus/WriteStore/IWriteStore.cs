namespace Platypus.WriteStore
{
    public interface IWriteStore<TModel, in TKey>
    {
        void Write(TModel model);
        TModel Get(TKey key);
        TModel Get(long id);
    }
}