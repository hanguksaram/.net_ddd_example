using System;
using System.Linq;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.Entity01
{
    public class HasActiveEntityEntitySalePosSpecification : Specification<EusMainPoint>
    {
        private readonly DateTime onDate;

        public HasActiveEntityEntitySalePosSpecification(DateTime onDate)
        {
            this.onDate = onDate;
        }
        public override Expression<Func<EusMainPoint, bool>> ToExpression() =>
            x => x.EntityMainPoints.Any(y => y.RevokedByCorrelationId == null
                                       && y.EntityExpirations.Any(v => v.IsActive
                                                                       && v.ValidFrom <= onDate
                                                                       && v.ValidTo >= onDate));
    }
}
