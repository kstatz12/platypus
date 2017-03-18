using System;

namespace Platypus.WriteStore.OrmLite
{
    public abstract class OrmLiteWriteStoreModel
    {
        public virtual Guid Key { get; set; }
        public virtual long Id { get; set; }
    }
}