using System;

namespace Platypus.WriteStore.OrmLite
{
    public abstract class OrmLiteWriteStoreModel
    {
        public Guid Key { get; set; }
        public long Id { get; set; }
    }
}