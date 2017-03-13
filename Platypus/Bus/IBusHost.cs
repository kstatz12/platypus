namespace Platypus.Bus
{
    public interface IBusHost
    {
        string Host { get; }
        string UserName { get; }
        string Password { get; }
    }
}