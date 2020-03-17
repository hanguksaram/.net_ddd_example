using Entity.ApplicationServices.Queries.PartnerProfile;
using Entity.CrossCutting;
using System.Linq;
using System.Threading.Tasks;
using Entity.DataAccess.Specification;
using Entity.DataAccess.Specification.PartnerProfile;

namespace Entity.DataAccess.Queries.PartnerProfile
{
    public class GetEntityPlaceIdsAndRnTapsByTermQueryHandler : IGetEntityPlaceIdsAndRnTapsByTermQueryHandler
    {
        private readonly IListQueryHandler _queryHandler;
        public GetEntityPlaceIdsAndRnTapsByTermQueryHandler(IListQueryHandler queryHandler)
        {
            _queryHandler = queryHandler;
        }

        public async Task<IGetEntityPlaceIdsAndRnTapsByTermQueryResult> Handle(GetEntityPlaceIdsAndRnTapsByTermQuery query)
        {
            var EusPlaceSpecification = Specification<EusPlace>.Empty;
            Specification<Lt> EntityPlaceSpecification = new NotEmptyPlaceIdLtSpecification();
            Specification<Lt> grpPlaceSpecification = new NotEmptyGrpLtSpecification();
            Specification<RnOffc> RnPlaceSpecification = new NotEmptyRnOffcSpecification();
                
            if (!string.IsNullOrEmpty(query.Term))
            {
                EusPlaceSpecification = EusPlaceSpecification.And(new FindByPlaceIdOffcSpecification(query.Term));
                EntityPlaceSpecification = EntityPlaceSpecification.And(new FindByPlaceIdLtSpecification(query.Term));
                RnPlaceSpecification = RnPlaceSpecification.And(new FindByRnOffcIdSpecification(query.Term));
            }
            if (query.IdentityFilter?.AgentCodes?.Any() ?? false)
            {
                EusPlaceSpecification = EusPlaceSpecification.And(new ForAgentOffcSpecification(query.IdentityFilter.AgentCodes));
                EntityPlaceSpecification = EntityPlaceSpecification.And(new ForAgentLtSpecification(query.IdentityFilter.AgentCodes));
                RnPlaceSpecification = RnPlaceSpecification.And(new ForAgentRnOffcSpecification(query.IdentityFilter.AgentCodes));
            }
            if (query.IdentityFilter?.RegionCodes?.Any() ?? false)
            {
                EusPlaceSpecification = EusPlaceSpecification.And(new ForRegionManagerOffcSpecification(query.IdentityFilter.RegionCodes));
                EntityPlaceSpecification = EntityPlaceSpecification.And(new ForRegionLtSpecificaton(query.IdentityFilter.RegionCodes));
                RnPlaceSpecification = RnPlaceSpecification.And(new ForRegionRnOffcSpecification(query.IdentityFilter.RegionCodes));
            }
     
            var EusPlaceIds = await _queryHandler.HandleAsync(EusPlaceSpecification.ToExpression(), x => x.PlaceId, query);

            var EntityPlaceIds = await _queryHandler.HandleAsync(EntityPlaceSpecification.ToExpression(), x => x.PlaceId, query);

            var PlaceGrps = await _queryHandler.HandleAsync(EntityPlaceSpecification.ToExpression(), x => x.GRP, query);

            var RnOffcs = await _queryHandler.HandleAsync(RnPlaceSpecification.ToExpression(), x => x.Offc, query);

            var result = EusPlaceIds
                .Union(EntityPlaceIds)
                .Union(PlaceGrps)
                .Union(RnOffcs)
                .Distinct()
                .ApplyPaging(query)
                .ToArray();

            if (!result.Any())
                return GetEntityPlaceIdsAndRnTapsByTermQueryResult.Empty;

            return new GetEntityPlaceIdsAndRnTapsByTermQueryResult(result);
        }
    }
}
