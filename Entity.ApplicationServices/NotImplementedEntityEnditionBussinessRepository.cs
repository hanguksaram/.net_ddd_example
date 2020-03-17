using Entity.Domain.Process;
using System.Threading.Tasks;
using Entity.CrossCutting;

namespace Entity.ApplicationServices
{
    public class NotImplementedEntityRevocationProcessRepository : IEntityRevocationProcessRepository
    {
        public Task<MayBe<EntityPlaceRevocationProcess>> GetByRequestNumber(string requestNumber)
        {
            throw new System.NotImplementedException();
        }

        public Task Save(EntityPlaceRevocationProcess EntitySales)
        {
            throw new System.NotImplementedException();
        }

        public Task<EntityPlaceRevocationProcess> StartNew(string requestNumber, Author author)
        {
            throw new System.NotImplementedException();
        }
    }
}