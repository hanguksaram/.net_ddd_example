using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Entity.ApplicationServices.Repositories;
using Entity.CrossCutting;
using Entity.CrossCutting.Exceptions;
using Entity.Domain;

namespace Entity.ApplicationServices
{
    public class EntitySalesApplicationService
    {
        private readonly IEntitySalesRepository _EntitySalesRepository;
        private readonly IDatetimeProvider _dateTimeProvider;

        public EntitySalesApplicationService(
            IEntitySalesRepository EntitySalesRepository,
            IDatetimeProvider dateTimeProvider)
        {
            _EntitySalesRepository = EntitySalesRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<EntityPlace> AttachNewPlace(Guid BasePointGuid, Guid EntityId, string PlaceId, IPrincipal user)
        {
            var now = _dateTimeProvider.GetLocalNow();
            var EntitySales = await _EntitySalesRepository.GetByIdWithEntityActiveOnDate(BasePointGuid, now);
            if (EntitySales == null)
            {
                throw new NotFoundException($"EntitySales({BasePointGuid}) не найден");
            }

            var Place = new EntityPlace(PlaceId, now);
            EntitySales.CreateNewPlaceForEntity(EntityId, Place);

            await _EntitySalesRepository.UpdateEntitySale(EntitySales, user, now);

            return Place;
        } 
    }
}
