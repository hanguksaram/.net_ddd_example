using System;
using System.Linq;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.Entity01
{
    public class HasEntityLtEntityExSpecification : Specification<EntityExpiration>
    {
        public override Expression<Func<EntityExpiration, bool>> ToExpression() =>
            x => x.EntityMainPoint.Lts.Any(y => (y.LtSystem == LtSystems.Eus 
                                         || y.LtSystem == LtSystems.Sabre
                                         || y.LtSystem == LtSystems.Elio)
                                         && y.ValidTo == null
                                         && y.RevokedByCorrelationId == null);
    }
}
