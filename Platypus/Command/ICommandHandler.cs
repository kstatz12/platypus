using Platypus.Domain;

namespace Platypus.Command
{
    public interface ICommandHandler<in TCommand> : IHandler<TCommand> where TCommand : ICommand
    {

    }
}