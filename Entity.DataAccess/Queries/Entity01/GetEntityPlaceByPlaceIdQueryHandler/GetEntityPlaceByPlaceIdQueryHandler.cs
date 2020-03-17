using Entity.ApplicationServices.Queries.Entity01;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Entity.DataAccess.Specification.Entity01;
using Entity.CrossCutting;

namespace Entity.DataAccess.Queries.Entity01
{
    public class GetEntityPlaceByPlaceIdQueryHandler : IGetEntityPlaceByPlaceIdQueryHandler
    {
        private readonly IDatetimeProvider _datetimeProvider;
        private readonly IListQueryHandler _queryHandler;

        public GetEntityPlaceByPlaceIdQueryHandler(IDatetimeProvider datetimeProvider
            , IListQueryHandler queryHandler)
        {
            _datetimeProvider = datetimeProvider;
            _queryHandler = queryHandler;
        }

        public async Task<IGetEntityPlaceByPlaceIdQueryResult> Handle(GetEntityPlaceByPlaceIdQuery query)
        {
            if (string.IsNullOrWhiteSpace(query.PlaceId))
            {
                return GetEntityPlaceByPlaceIdQueryResult.Empty;
            }

            if (query.PlaceId.IsDirectPlace())
            {
                return GetEntityPlaceByPlaceIdQueryResult.ForbiddenResult;
            }

            var date = _datetimeProvider.GetLocalNow().Date;

            var posSpec = new HasActiveEntityEntitySalesEntityPlaceSpecification(date)
                       .And(new GetByIdEntityPlaceSpecification(query.PlaceId));

            var EntityPlace = (await _queryHandler.HandleAsync(posSpec.ToExpression(), asp => asp)).FirstOrDefault();

            return EntityPlace == null ? GetEntityPlaceByPlaceIdQueryResult.Empty : new GetEntityPlaceByPlaceIdQueryResult(EntityPlace.EpGuid, EntityPlace.EntityBasePoint.AgentCode);
        }
    }

    static class StringExtensions
    {
        public static bool IsDirectPlace(this string PlaceId)
        {
            return Regex.IsMatch(PlaceId, "^...test.*$", RegexOptions.IgnoreCase);
        }
    }
}
