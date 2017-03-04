using System;

namespace Platypus.Domain
{
    public interface IAggregate
    {
        Guid Key { get; set; }
    }
}