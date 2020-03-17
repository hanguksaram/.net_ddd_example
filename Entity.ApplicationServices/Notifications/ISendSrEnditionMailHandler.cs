using Entity.Domain;
using System.Threading.Tasks;

namespace Entity.ApplicationServices.Notifications
{
    public interface ISendSrnRevocationMailHandler
    {
        Task Send(EntityEntity EntityEntity);
    }
}