using MassTransit;

namespace Entity.Infrastructure.Messaging
{
    public interface IProcessBusControl
    {
        IBusControl Control { get; }
    }
}