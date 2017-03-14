using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Platypus.Command;
using Platypus.Event;

namespace Platypus.Domain
{
    public class DomainBehaviorBase<TAggregate, TCommand> where TAggregate : AggregateRoot 
        where TCommand : ICommand 
    {
        private readonly TAggregate _aggregate;
        private readonly TCommand _command;
        private readonly List<Tuple<AbstractValidator<TAggregate>, IEvent>> _aggregateValidators;
        private readonly List<ValidationResult> _aggregateValidationResults;
        public DomainBehaviorBase(TAggregate aggregate, TCommand command, List<Tuple<AbstractValidator<TAggregate>, IEvent>> aggregateValidators)
        {
            _aggregate = aggregate;
            _command = command;
            _aggregateValidators = aggregateValidators;
            _aggregateValidationResults = new List<ValidationResult>();
        }

        public void Execute(Func<TCommand, IEvent> behavior)
        {
            if (!ValidateAggregate()) return;
            var @event = behavior(_command);
            _aggregate.ApplyChange(@event, true);
        }

        private bool ValidateAggregate()
        {
            foreach (var validator in _aggregateValidators)
            {
                var result = validator.Item1.Validate(_aggregate);
                if (result.IsValid) continue;
                _aggregate.AddPublishOnly(validator.Item2);
                _aggregateValidationResults.Add(result);
            }
            return _aggregateValidationResults.Any();
        }
    }
}