using Entity.ApplicationServices.Queries.PartnerProfile;
using Entity.DataAccess.Specification.PartnerProfile;
using Entity.Domain;
using System.Linq;
using System.Threading.Tasks;
using Entity.CrossCutting;

namespace Entity.DataAccess.Queries.PartnerProfile
{
    public class GetAvailableEntityQueryHandler : IGetAvailableEntityQueryHandler
    {
        private readonly IDatetimeProvider _datetimeProvider;
        private readonly IListQueryHandler _queryHandler;
        public GetAvailableEntityQueryHandler(
            IDatetimeProvider datetimeProvider,
            IListQueryHandler queryHandler)
        {
            _datetimeProvider = datetimeProvider;
            _queryHandler = queryHandler;
        }
        public async Task<IGetAvailableEntityQueryResult> Handle(GetAvailableEntityQuery query)
        {

            var spec = new AvailableEntitySpecification(_datetimeProvider.GetLocalNow());

            var availableEntity = await _queryHandler.HandleAsync(spec.ToExpression(), v => new EntityModel
            {
                EntityId = v.EntityGuid,
                Number = v.Number
            }, query);

            availableEntity = availableEntity
                .Where(n => EntityNumber.TryParse(n.Number, out var Entity) && Entity.IsControlNumberValid())
                .ToArray();

            return new GetAvailableEntityQueryResult(availableEntity);
        }
    }
}
