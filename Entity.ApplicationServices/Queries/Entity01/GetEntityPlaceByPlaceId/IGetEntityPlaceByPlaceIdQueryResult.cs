using System;

namespace Entity.ApplicationServices.Queries.Entity01
{
    public interface IGetEntityPlaceByPlaceIdQueryResult
    {
        Guid EntityPlaceGuid { get; }
        string AgencyCode { get; }
        bool Forbidden { get; }
    }
}