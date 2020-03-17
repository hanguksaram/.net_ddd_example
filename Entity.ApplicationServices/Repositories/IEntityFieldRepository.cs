using Entity.Domain;
using System;
using System.Threading.Tasks;
using System.Security.Principal;

namespace Entity.ApplicationServices.Repositories
{
    public interface IEntityEntityRepository
    {
        Task<EntityEntity> GetEntityById(Guid id);
        Task DeleteEntity(EntityEntity Entity, string user, DateTime now);
        Task DeletePlace(Guid PlaceId, string user, DateTime now);
        Task Create(EntityEntity Entity, IPrincipal user, DateTime now);
        Task Update(EntityEntity Entity, IPrincipal principal, DateTime now);
    }
}
