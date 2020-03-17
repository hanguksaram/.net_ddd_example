using System;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.PartnerProfile
{
    public class FindByPlaceIdEpSpecification : Specification<Ep>
    {
        private readonly string searchTerm;
        public FindByPlaceIdEpSpecification(string searchTerm)
        {
            if (searchTerm == null)
                throw new ArgumentNullException(nameof(searchTerm));

            this.searchTerm = searchTerm.Trim();

            if (string.IsNullOrWhiteSpace(searchTerm))
                throw new ArgumentNullException(nameof(searchTerm));
        }

        public override Expression<Func<Ep, bool>> ToExpression() =>
            sp => sp.PlaceId.Contains(searchTerm) || sp.GRP.Contains(searchTerm);

    }
}
