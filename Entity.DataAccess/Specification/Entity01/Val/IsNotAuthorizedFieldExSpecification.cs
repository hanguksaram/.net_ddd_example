using System;
using System.Linq;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.Entity01
{
    public class IsNotAuthorizedEntityExSpecification : Specification<EntityExpiration>
    {
        public override Expression<Func<EntityExpiration, bool>> ToExpression() =>
            x => !x.EntityMainPoint.Lts.Any(p => p.ValidTo == null && p.RevokedByCorrelationId == null);
    }
}
