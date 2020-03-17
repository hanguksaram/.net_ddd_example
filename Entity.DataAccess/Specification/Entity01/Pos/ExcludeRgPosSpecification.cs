using System;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.Entity01
{
    public class ExcludeRgPosSpecification : Specification<EusSp>
    {
        public override Expression<Func<EusSp, bool>> ToExpression() => x => x.AgentCode != "1488";
    }
}
