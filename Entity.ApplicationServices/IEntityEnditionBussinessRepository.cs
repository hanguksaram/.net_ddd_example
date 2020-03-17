using Entity.Domain.Process;
using System.Threading.Tasks;
using Entity.CrossCutting;

namespace Entity.ApplicationServices
{
    public interface IEntityRevocationProcessRepository
    {
        Task<EntityPlaceRevocationProcess> StartNew(string requestNumber, Author author);
        Task<MayBe<EntityPlaceRevocationProcess>> GetByRequestNumber(string requestNumber);

        Task Save(EntityPlaceRevocationProcess EntitySales);
    }
}