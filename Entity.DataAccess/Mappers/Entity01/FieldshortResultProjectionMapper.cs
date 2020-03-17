using DataAccess.Projections.Entity01;
using Entity.DataAccess;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.Mappers.Entity01
{
    internal class EntityhortResultProjectionMapper
    {
        internal Expression<Func<EntityExpiration,EntityhortResultProjection>> ToExpression() => ve => new EntityhortResultProjection
                {
                    Id = ve.EntityExpirationGuid,
                    PosId = ve.EntityMainPoint.EusMainPointGuid,
                    Number = ve.Entity.Number,
                    IsAuthorized = ve.EntityMainPoint.Lts.Any(),
                    Sys = ve.EntityMainPoint.Sys,
                    Places = ve.EntityMainPoint.Lts.Where(x => x.PlaceId != null && x.PlaceId != "").Select(x => x.PlaceId)
                };
    }
}
