using Entity.ApplicationServices.Queries.PartnerProfile;
using Entity.CrossCutting;
using System.Linq;
using System.Threading.Tasks;
using Entity.DataAccess.Specification;
using Entity.DataAccess.Specification.PartnerProfile;

namespace Entity.DataAccess.Queries.PartnerProfile
{
    public class GetPlaceIdsByTermQueryHandler : IGetPlaceIdsByTermQueryHandler
    {
        private readonly IListQueryHandler _queryHandler;
        public GetPlaceIdsByTermQueryHandler(IListQueryHandler queryHandler)
        {
            _queryHandler = queryHandler;
        }

        public async Task<IGetPlaceIdsByTermQueryResult> Handle(GetPlaceIdsByTermQuery query)
        {
            var UesPlaceSpecification = Specification<UesPlace>.Empty;
            var EntityPlaceSpecification = Specification<Ep>.Empty;
                
            if (!string.IsNullOrEmpty(query.Term))
            {
                UesPlaceSpecification = UesPlaceSpecification.And(new FindByPlaceIdOffcSpecification(query.Term));
                EntityPlaceSpecification = EntityPlaceSpecification.And(new FindByPlaceIdEpSpecification(query.Term));
            }
            if (query.IdentityFilter?.AgentCodes?.Any() ?? false)
            {
                UesPlaceSpecification = UesPlaceSpecification.And(new ForAgentOffcSpecification(query.IdentityFilter.AgentCodes));
                EntityPlaceSpecification = EntityPlaceSpecification.And(new ForAgentEpSpecification(query.IdentityFilter.AgentCodes));
            }
            if (query.IdentityFilter?.RegionCodes?.Any() ?? false)
            {
                UesPlaceSpecification = UesPlaceSpecification.And(new ForRegionManagerOffcSpecification(query.IdentityFilter.RegionCodes));
                EntityPlaceSpecification = EntityPlaceSpecification.And(new ForRegionEpSpecificaton(query.IdentityFilter.RegionCodes));
            }
     
            var UesPlaceIds = await _queryHandler.HandleAsync(UesPlaceSpecification.ToExpression(), x => x.PlaceId, query);

            var EntityPlaceIds = await _queryHandler.HandleAsync(EntityPlaceSpecification.ToExpression(), x => x.PlaceId, query);

            var result = UesPlaceIds
                .Union(EntityPlaceIds)
                .Where(v => !string.IsNullOrEmpty(v))
                .Distinct()
                .ApplyPaging(query)
                .ToArray();

            if (!result.Any())
                return GetPlaceIdsByTermQueryResult.Empty;

            return new GetPlaceIdsByTermQueryResult(result);
        }
    }
}
