using System;
using System.Linq;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.Entity01
{
    public class HasActiveEntityEntitySalesEntityPlaceSpecification : Specification<Lt>
    {
        private readonly DateTime onDate;

        public HasActiveEntityEntitySalesEntityPlaceSpecification(DateTime onDate)
        {
            this.onDate = onDate;
        }

        public override Expression<Func<Lt, bool>> ToExpression() => Lt =>
            Lt.EntityMainPoint.EntityExpirations.Any(v => v.IsActive && v.ValidFrom <= onDate && onDate <= v.ValidTo);
    }
}
