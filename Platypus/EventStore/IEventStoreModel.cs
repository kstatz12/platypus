using System.Security.Cryptography.X509Certificates;

namespace Platypus.EventStore
{
    public interface IEventStoreModel
    {
        long Id { get; set; }
        string Key { get; set; }
        int SchemaVersion { get; set; }
        string EventType { get; set; }
        string EventBody { get; set; }
    }
}