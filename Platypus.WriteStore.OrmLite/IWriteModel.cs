using System;

namespace Platypus.WriteStore.OrmLite
{
    public interface IWriteModel
    {
        Guid Key { get; set; }
        long Id { get; set; }
    }
}