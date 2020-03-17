using System;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.Entity01
{
    public class ActiveOnDateEntityExSpecification : Specification<EntityExpiration>
    {
        private readonly DateTime date;

        public ActiveOnDateEntityExSpecification(DateTime date)
        {
            this.date = date;
        }

        public override Expression<Func<EntityExpiration, bool>> ToExpression() => x => x.IsActive && x.ValidFrom <= date && x.ValidTo >= date;
    }
}
