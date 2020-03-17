using System;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.Entity01
{
    public class FindByEntitySalePosSpecification : Specification<EusMainPoint>
    {
        private readonly string searchTerm;

        public FindByEntitySalePosSpecification(string searchTerm)
        {
            if (searchTerm == null)
                throw new ArgumentNullException(nameof(searchTerm));

            this.searchTerm = searchTerm.Trim();

            if (string.IsNullOrWhiteSpace(searchTerm))
                throw new ArgumentNullException(nameof(searchTerm));
        }

        public override Expression<Func<EusMainPoint, bool>> ToExpression() =>
            x => x.MainPointId.StartsWith(searchTerm)
            || x.Address.Contains(searchTerm)
            || x.AddressLatin.Contains(searchTerm);
    }
}
