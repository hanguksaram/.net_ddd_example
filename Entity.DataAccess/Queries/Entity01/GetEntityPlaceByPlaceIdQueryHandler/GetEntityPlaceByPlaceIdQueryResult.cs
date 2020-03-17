using Entity.ApplicationServices.Queries.Entity01;
using System;

namespace Entity.DataAccess.Queries.Entity01
{
    class GetEntityPlaceByPlaceIdQueryResult : IGetEntityPlaceByPlaceIdQueryResult
    {
        public static IGetEntityPlaceByPlaceIdQueryResult Empty => default;

        public static IGetEntityPlaceByPlaceIdQueryResult ForbiddenResult => new GetEntityPlaceByPlaceIdQueryResult();
        public GetEntityPlaceByPlaceIdQueryResult(Guid BasePointGuid, string agencyCode)
        {
            EntityPlaceGuid = BasePointGuid;
            AgencyCode = agencyCode;
        }

        private GetEntityPlaceByPlaceIdQueryResult()
        {
            Forbidden = true;
        }

        public Guid EntityPlaceGuid { get; }

        public string AgencyCode { get; }

        public bool Forbidden { get; }
    }
}
