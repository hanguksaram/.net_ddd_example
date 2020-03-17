using Entity.Domain;
using System.Threading.Tasks;

namespace Entity.ApplicationServices.Services
{
    public interface ISendSrnRevocationProcessMessageHandler
    {
        Task Send(EntityEntity Entity, string author);
    }
}