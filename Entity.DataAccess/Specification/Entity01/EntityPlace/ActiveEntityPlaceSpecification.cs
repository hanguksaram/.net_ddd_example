using System;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.Entity01
{
    public class ActiveEntityPlaceSpecification : Specification<Ep>
    {
        public override Expression<Func<Ep, bool>> ToExpression() => Ep => Ep.Status == true;
    }
}
