using System;
using Platypus.Repository;
using Platypus.WriteStore;

namespace Platypus.Domain
{
    public interface IDomainRepository<TAggregate,TModel> : IRepository<TAggregate, Guid> where TAggregate : AggregateRoot
    {
        IWriteStore<TModel, Guid, long> WriteStore { get; }
        TModel ToWriteModel(TAggregate aggregate);
        TAggregate ToAggregate(TModel model);
        TAggregate Get(long id);
    }
}