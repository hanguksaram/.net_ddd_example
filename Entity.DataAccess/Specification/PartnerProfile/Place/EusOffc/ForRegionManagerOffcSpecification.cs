using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.PartnerProfile
{
    public class ForRegionManagerOffcSpecification : Specification<UesPlace>
    {
        private readonly HashSet<int> regionCodes;
        public ForRegionManagerOffcSpecification(HashSet<int> regionCodes)
        {
            if (regionCodes == null || !regionCodes.Any())
                throw new ArgumentNullException(nameof(regionCodes));

            this.regionCodes = regionCodes;
        }
        public override Expression<Func<UesPlace, bool>> ToExpression() =>
            sp => regionCodes.Contains(sp.Organization.Stc.RegionCode.Value);
    }
}
