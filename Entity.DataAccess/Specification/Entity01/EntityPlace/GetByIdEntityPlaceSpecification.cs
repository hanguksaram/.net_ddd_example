using System;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.Entity01
{
    public class GetByIdEntityPlaceSpecification : Specification<Ep>
    {
        private readonly string PlaceId;

        public GetByIdEntityPlaceSpecification(string PlaceId)
        {
            if(PlaceId == null)
                throw new ArgumentNullException(nameof(PlaceId));

            this.PlaceId = PlaceId.Trim();

            if(string.IsNullOrWhiteSpace(PlaceId))
                throw new ArgumentNullException(nameof(PlaceId));
        }

        public override Expression<Func<Ep, bool>> ToExpression() => Ep => Ep.PlaceId == PlaceId;
    }
}
