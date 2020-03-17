using Entity.ApplicationServices.Repositories;
using Entity.Domain;
using System;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Entity.ApplicationServices.Tests.Fakes
{
    internal class MockEntityEntityRepository : IEntityEntityRepository
    {
        public EntityEntity EntityEntityToReturn = null;

        public Task<EntityEntity> GetEntityById(Guid id) => Task.FromResult(EntityEntityToReturn);

        public Task Create(EntityEntity EntitySales, IPrincipal user, DateTime now) => Task.CompletedTask;

        public Task DeletePlace(Guid PlaceId, string user, DateTime now) => Task.CompletedTask;

        public Task DeleteEntity(EntityEntity Entity, string user, DateTime now) => Task.CompletedTask;

        public Task Update(EntityEntity Entity, IPrincipal principal, DateTime now) => Task.CompletedTask;
    }
}
