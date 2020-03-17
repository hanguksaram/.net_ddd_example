using System;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.Entity01
{
    public class FindByNumberEntityExSpecification : Specification<EntityExpiration>
    {
        private readonly string searchTerm;

        public FindByNumberEntityExSpecification(string searchTerm)
        {
            if (searchTerm == null)
                throw new ArgumentNullException(nameof(searchTerm));

            this.searchTerm = searchTerm.Replace("-", "").Trim();
        }

        public override Expression<Func<EntityExpiration, bool>> ToExpression() => 
            v => v.Entity.Number.Replace("-","").Contains(searchTerm);
    }
}
