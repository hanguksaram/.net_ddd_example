using System;
using System.Linq;
using System.Threading.Tasks;
using Entity.ApplicationServices.Services;
using Entity.CrossCutting;
using Entity.DataAccess.Specification.Entity01;
using Entity.DataAccess.Specification.PartnerProfile;
using Entity = Entity.DataAccess.Specification.PartnerProfile.Entity;
using Srn = Entity.DataAccess.Specification.PartnerProfile.Srn;

namespace Entity.DataAccess.ServicesImpl
{
    public class EntityPlaceApplicationService : IEntityPlaceApplicationService
    {
        private readonly IDatetimeProvider _datetimeProvider;
        private readonly IListQueryHandler _queryHandler;

        public EntityPlaceApplicationService(
            IDatetimeProvider dateTimeProvider,
            IListQueryHandler queryHandler)
        {
            _datetimeProvider = dateTimeProvider;
            _queryHandler = queryHandler;
        }

        public async Task<bool> CheckIfEntityPlaceExistsByIdentificator(string PlaceIdOrTap, Guid? existingPlace)
        {
            if (string.IsNullOrEmpty(PlaceIdOrTap?.Trim()))
            {
                throw new ArgumentNullException();
            }

            var date = _datetimeProvider.GetLocalNow().Date;

            var posSpec = new HasActiveEntityEntitySalesEntityPlaceSpecification(date)
                       .And(new GetByIdEntityPlaceSpecification(PlaceIdOrTap));

            if (existingPlace.HasValue)
                posSpec= posSpec.And(new Entity.NotOwnedByEntitypecification(existingPlace.Value));

            var EntityPlace = (await _queryHandler.HandleAsync(posSpec.ToExpression(), asp => asp.PlaceId));

            var SrnSpec = new SrnTermianlExactTapMatchingSpecification(PlaceIdOrTap)
                .And(new ActiveSrnPoolSpecification());

            if (existingPlace.HasValue)
                SrnSpec = SrnSpec.And(new Srn.NotOwnedByEntitypecification(existingPlace.Value));

            var SrnPools = await _queryHandler.HandleAsync(SrnSpec.ToExpression(), o => o.Offc);

            return SrnPools.Any() || EntityPlace.Any();
        }
    }
}

