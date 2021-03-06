﻿using System;
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
        private readonly AbstractValidator<TCommand> _commandValidator;
        public DomainBehaviorBase(TAggregate aggregate, TCommand command, List<Tuple<AbstractValidator<TAggregate>, IEvent>> aggregateValidators, AbstractValidator<TCommand> commandValidator)
        {
            _aggregate = aggregate;
            _command = command;
            _aggregateValidators = aggregateValidators;
            _aggregateValidationResults = new List<ValidationResult>();
            _commandValidator = commandValidator;
        }

        public void Execute(IEvent @event)
        {
            if (!ValidateAggregate() || !ValidateCommand()) return;
            _aggregate.ApplyChange(@event, true);
        }

        private bool ValidateCommand()
        {
            var validationResult = _commandValidator.Validate(_command);
            return validationResult.IsValid;
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