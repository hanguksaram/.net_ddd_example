using System;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.Entity01
{
    public class ExcludeExpiredPosSpecification : Specification<UesBasePoint>
    {
        private readonly DateTime onDate;

        public ExcludeExpiredPosSpecification(DateTime onDate)
        {
            this.onDate = onDate;
        }
        public override Expression<Func<UesBasePoint, bool>> ToExpression() => x => x.ValidTo >= onDate;
    }
}
