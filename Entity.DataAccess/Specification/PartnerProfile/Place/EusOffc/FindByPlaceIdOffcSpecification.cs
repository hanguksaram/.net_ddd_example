using System;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.PartnerProfile
{
    public class FindByPlaceIdOffcSpecification : Specification<UesPlace>
    {
        private readonly string searchTerm;
        public FindByPlaceIdOffcSpecification(string searchTerm)
        {
            if (searchTerm == null)
                throw new ArgumentNullException(nameof(searchTerm));

            this.searchTerm = searchTerm.Trim();

            if (string.IsNullOrWhiteSpace(searchTerm))
                throw new ArgumentNullException(nameof(searchTerm));
        }

        public override Expression<Func<UesPlace, bool>> ToExpression() =>
            sp => sp.PlaceId.Contains(searchTerm);

    }   
}
