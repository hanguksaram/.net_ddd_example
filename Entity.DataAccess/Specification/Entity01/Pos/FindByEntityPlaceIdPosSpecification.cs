using System;
using System.Linq;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.Entity01
{
    public class FindByEntityPlaceIdPosSpecification : Specification<UesBasePoint>
    {
        private readonly string PlaceId;

        public FindByEntityPlaceIdPosSpecification(string PlaceId)
        {
            if (PlaceId == null)
                throw new ArgumentNullException(nameof(PlaceId));

            this.PlaceId = PlaceId.Trim();

            if (string.IsNullOrWhiteSpace(PlaceId))
                throw new ArgumentNullException(nameof(PlaceId));
        }

        public override Expression<Func<UesBasePoint, bool>> ToExpression() =>
            x => x.EntityBasePoints.Any(s => s.Eps.Any(p => p.PlaceId == PlaceId));
    }
}
