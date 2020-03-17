using System;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.Entity01
{
    public class NotOnTerminationEntityExSpecification : Specification<EntityExpiration>
    {
        public override Expression<Func<EntityExpiration, bool>> ToExpression() =>
            x => x.EntityMainPoint.RevokedByCorrelationId == null;
    }
}
