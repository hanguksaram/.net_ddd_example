using System;
using System.Linq;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.Entity01
{
    public class ForPosEntityExSpecification : Specification<EntityExpiration>
    {
        private readonly Guid[] posId;

        public ForPosEntityExSpecification(params Guid[] posId)
        {
            this.posId = posId;
        }

        public override Expression<Func<EntityExpiration, bool>> ToExpression() => x => x.EntityMainPoint.EusMainPointGuid != null && posId.Contains((Guid)x.EntityMainPoint.EusMainPointGuid);
    }
}
