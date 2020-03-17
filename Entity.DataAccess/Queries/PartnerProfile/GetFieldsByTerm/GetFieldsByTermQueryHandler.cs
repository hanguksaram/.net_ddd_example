using Entity.ApplicationServices.Queries.PartnerProfile;
using Entity.CrossCutting;
using Entity.DataAccess.Projections.PartnerProfile;
using Entity.DataAccess.Specification.PartnerProfile;
using System.Linq;
using System.Threading.Tasks;
using Entity.DataAccess.Specification;

namespace Entity.DataAccess.Queries.PartnerProfile
{
    public class GetEntityByTermQueryHandler : IGetEntityByTermQueryHandler
    {
        private readonly IListQueryHandler _queryHandler;
        public GetEntityByTermQueryHandler(IListQueryHandler queryHandler)
        {
            _queryHandler = queryHandler;
        }
        public async Task<IGetEntityByTermQueryResult> Handle(GetEntityByTermQuery query)
        {

            Specification<EntityExpiration> EntityEntityOwnerSpecification = new ActiveEntityNumberSpecification();
            var EusEntityOwnerSpecification = Specification<EusPlace>.Empty;

            if (!string.IsNullOrEmpty(query.Term))
            {
                EusEntityOwnerSpecification = EusEntityOwnerSpecification.And(new FindByEusEntityNumberSpecification(query.Term));
                EntityEntityOwnerSpecification = EntityEntityOwnerSpecification.And( new FindByEntityEntityNumberSpecification(query.Term));
            }

            if (query.IdentityFilter?.AgentCodes?.Any() ?? false)
            {
                EntityEntityOwnerSpecification = EntityEntityOwnerSpecification.And(new ForAgentEntityEntitypecification(query.IdentityFilter.AgentCodes));
                EusEntityOwnerSpecification = EusEntityOwnerSpecification.And(new ForAgentEusEntitypecification(query.IdentityFilter.AgentCodes));
            }

            if (query.IdentityFilter?.RegionCodes?.Any() ?? false)
            {
                EntityEntityOwnerSpecification = EntityEntityOwnerSpecification.And(new ForRegionEntityEntitypecification(query.IdentityFilter.RegionCodes));
                EusEntityOwnerSpecification = EusEntityOwnerSpecification.And(new ForRegionEusEntitypecification(query.IdentityFilter.RegionCodes));
            }
            var result = await _queryHandler.HandleAsync(EusEntityOwnerSpecification.ToExpression(),
                  x => x.DefaultNumberValidity.Select(y => new EntityhortResultProjection
                  {
                      Id = y.DefaultNumber.Id,
                      Number = y.DefaultNumber.Number,
                  }), query);

            if (!query.OnlyWithAuthorizedAccess)
            {
                var EntityEntityResult = await _queryHandler.HandleAsync(EntityEntityOwnerSpecification.ToExpression(),
                x => new EntityhortResultProjection
                {
                    Id = x.Entity.EntityGuid,
                    Number = x.Entity.Number
                }, query);

                var EusAuthorizedAccessEntityResult = await _queryHandler.HandleAsync(EusEntityOwnerSpecification.ToExpression(),
                    x => x.UADefaultNumberValidity.Select(y => new EntityhortResultProjection
                    {
                        Id = y.UADefaultNumber.Id,
                        Number = y.UADefaultNumber.Number
                    }), query);

                result = result.Union(EntityEntityResult)
                  .Union(EusAuthorizedAccessEntityResult).ToArray();
            }

            var response = result
                .Distinct()
                .ApplyPaging(query);

            if (!response.Any())
            {
                return GetEntityByTermQueryResult.Empty;
            }

            return new GetEntityByTermQueryResult(response.Select(v => new EntityItem
            {
                Id = v.Id,
                Name = v.Number,
            }).ToArray());

        }
    }
}
