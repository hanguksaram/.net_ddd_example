using Entity.ApplicationServices.Queries.Entity01;
using DataAccess.Mappers.Entity01;
using DataAccess.Projections.Entity01;
using Entity.DataAccess.Specification.Entity01;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entity.CrossCutting;

namespace Entity.DataAccess.Queries.Entity01
{
    public class GetEntitySalesByTermQueryHandler: IGetEntitySalesByTermQueryHandler
    {
        private readonly IDatetimeProvider _datetimeProvider;
        private readonly IListQueryHandler _queryHandler;
        private readonly IFeatureChecker _featureChecker;

        public GetEntitySalesByTermQueryHandler(IDatetimeProvider datetimeProvider
            , IListQueryHandler queryHandler
            , IFeatureChecker featureChecker)
        {
            _datetimeProvider = datetimeProvider;
            _queryHandler = queryHandler;
            _featureChecker = featureChecker;
        }

        //todo get mapping for check tests
        public async Task<IGetEntitySalesByTermQueryResult> Handle(GetEntitySalesByTermQuery query)
        {
            var date = _datetimeProvider.GetLocalNow().Date;
            var excludePos = _featureChecker.Check("ExcludeExpiredEusPosForEntity01");
            var posSpec = new ExcludeCargoPosSpecification()
                       .And(new ForAgentPosSpecification(query.AgentCode));

            if (excludePos)
                posSpec = posSpec.And(new ExcludeExpiredPosSpecification(date));

            var valSpec = new ActiveOnDateEntityExSpecification(date)
                        .And(new NotOnTerminationEntityExSpecification())
                        .And(new IsNotAuthorizedEntityExSpecification().Or(new HasEntityLtEntityExSpecification()));
            if (string.IsNullOrWhiteSpace(query.Term) == false)
                switch (query.SearchType)
                {
                    case EntitySalesByTermQueryType.SearchByEntityNumber:
                        {
                            posSpec = posSpec.And(new FindByEntityEntityPosSpecification(query.Term, date));
                            valSpec = valSpec.And(new FindByNumberEntityExSpecification(query.Term));
                            break;
                        }
                    case EntitySalesByTermQueryType.SearchByEntitySalesInfo:
                        {
                            posSpec = posSpec.And(new FindByEntitySalePosSpecification(query.Term))
                                             .And(new HasActiveEntityEntitySalePosSpecification(date));
                            break;
                        }
                }

            var result = await _queryHandler.HandleAsync(posSpec.ToExpression(), asp => new EntitySalesShortResultProjection
            {
                Address = asp.Address,
                AddressLatin = asp.AddressLatin,
                Id = asp.Id,
                MainPointId = asp.MainPointId
            });

            if (result.Any() == false)
                return GetEntitySalesByTermQueryResult.Empty;

            valSpec = valSpec.And(new ForPosEntityExSpecification(result.Select(x => x.Id).ToArray()));

            var EntityResult = await _queryHandler.HandleAsync(valSpec.ToExpression(), new EntityhortResultProjectionMapper().ToExpression());

            EntityResult = FilterUnknownEntity(query.AgentCode, result, EntityResult);

            if (EntityResult.Any() == false)
                return GetEntitySalesByTermQueryResult.Empty;


            var mappedResult = result.Join(EntityResult.GroupBy(x => x.PosId), x => x.Id, x => x.Key,
                (pos, val) => new EntitySalesShortResult
                {
                    Address = pos.Address,
                    AddressLatin = pos.AddressLatin,
                    Id = pos.Id,
                    MainPointId = pos.MainPointId,
                    Entity = val.Select(x => new EntityhortResult
                    {
                        Number = x.Number,
                        SysType = Converters.MainPointSystemToSysType(x.Sys),
                        Id = x.Id,
                        Places = x.IsAuthorized ? x.Places.ToArray() : Array.Empty<string>()
                    }).ToArray()
                }).ToArray();

            return new GetEntitySalesByTermQueryResult(mappedResult);
        }

        private static EntityhortResultProjection[] FilterUnknownEntity(string agentCode, EntitySalesShortResultProjection[] result, EntityhortResultProjection[] EntityResult)
        {
            var unknowns = new List<Tuple<string, string>>();

            EntityResult = EntityResult.Where(x =>
            {
                if (Converters.MainPointSystemToSysType(x.Sys) == Entity.Domain.EntitySysType.Unknown)
                {
                    var pos = result.FirstOrDefault(y => y.Id == x.PosId);
                    unknowns.Add(Tuple.Create(pos?.MainPointId ?? "<empty>", x.Number));
                    return false;
                }
                return true;
            }).ToArray();

            if (unknowns.Count > 0)
            {
                Log.Error("Processs will not recieve this Entity for agent {agentCode}, becouse Reservation System is not specified: {@EntityWithUnknownSystem}", agentCode, unknowns);
            }

            return EntityResult;
        }
    }
}