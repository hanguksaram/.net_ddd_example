using System.Threading.Tasks;

namespace Entity.Infrastructure.Messaging
{
    public interface IMessageSender
    {
        Task Send<T>(T message, string endpoint) where T : class;
    }
}
