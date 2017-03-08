using ServiceStack.DataAnnotations;

namespace Platypus.EventStore.OrmLite
{
    [Alias("EventStore")]
    [Schema("Event")]
    public class OrmLiteEventStoreModel : IEventStoreModel
    {
        [PrimaryKey]
        [AutoIncrement]
        public long Id { get; set; }
        [Required]
        [CustomField("UNIQUEIDENTIFIER")]
        public string Key { get; set; }
        [Required]
        public int SchemaVersion { get; set; }
        [Required]
        [CustomField("VARCHAR(100)")]
        public string EventType { get; set; }
        [Required]
        [CustomField("NVARCHAR(MAX)")]
        public string EventBody { get; set; }
    }
}