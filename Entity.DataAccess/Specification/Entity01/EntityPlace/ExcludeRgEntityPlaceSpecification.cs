using System;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.Entity01
{
    public class ExcludeRgEntityPlaceSpecification : Specification<Lt>
    {
        public override Expression<Func<Lt, bool>> ToExpression() => Lt => Lt.EntitySp.EusSp.AgentCode != "1488";
    }
}
