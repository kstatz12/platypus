namespace Platypus.EventStore
{
    public interface IEventStoreModelHelpers
    {
        IEventStoreModel ToEventStoreModel(EventDescriptor input);
    }
}