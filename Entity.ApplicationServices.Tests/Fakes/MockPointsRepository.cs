using Entity.ApplicationServices.Repositories;
using Entity.Domain;
using System;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Entity.ApplicationServices.Tests.Fakes
{
    internal class MockEntitySalesRepository : IEntitySalesRepository
    {
        public EntitySales EntitySalesToReturn = null;
        
        public Task<EntitySales> GetById(Guid id) => Task.FromResult(EntitySalesToReturn);        

        public Task<EntitySales> GetByIdWithEntityActiveOnDate(Guid id, DateTime date) => Task.FromResult(EntitySalesToReturn);

        public Task<EntitySales> GetByEntityId(Guid id) => Task.FromResult(EntitySalesToReturn);

        public Task UpdateEntitySale(EntitySales EntitySales, IPrincipal user, DateTime updatedAt) => Task.CompletedTask;
    }
}
