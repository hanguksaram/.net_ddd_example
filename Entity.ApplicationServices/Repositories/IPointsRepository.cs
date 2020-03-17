using Entity.Domain;
using System;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Entity.ApplicationServices.Repositories
{
    public interface IEntitySalesRepository
    {
        Task<EntitySales> GetByIdWithEntityActiveOnDate(Guid id, DateTime date);
        Task UpdateEntitySale(EntitySales EntitySales, IPrincipal user, DateTime updateAt);
        Task<EntitySales> GetById(Guid id);
        Task<EntitySales> GetByEntityId(Guid id);
    }
}
