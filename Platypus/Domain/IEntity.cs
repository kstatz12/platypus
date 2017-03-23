using System;

namespace Platypus.Domain
{
    public interface IEntity
    {
        long Id { get; set; }
        Guid Key { get; set; }
    }
}