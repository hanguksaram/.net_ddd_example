using System;
using System.Linq;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.Entity01
{
    public class FindByEntityEntityPosSpecification : Specification<EusMainPoint>
    {
        private readonly string searchTerm;
        private readonly DateTime onDate;

        public FindByEntityEntityPosSpecification(string searchTerm, DateTime onDate)
        {
            if (searchTerm == null)
                throw new ArgumentNullException(nameof(searchTerm));

            this.searchTerm = searchTerm.Replace("-", "").Trim();

            if (string.IsNullOrWhiteSpace(searchTerm))
                throw new ArgumentNullException(nameof(searchTerm));

            this.onDate = onDate;
        }

        public override Expression<Func<EusMainPoint, bool>> ToExpression() =>
            x => x.EntityMainPoints.Any(s => s.RevokedByCorrelationId == null
                                       && s.EntityExpirations.Any(v => v.IsActive
                                                                       && v.ValidFrom <= onDate
                                                                       && v.ValidTo >= onDate
                                                                       && v.Entity.Number.Replace("-","").Contains(searchTerm)));
    }
}
