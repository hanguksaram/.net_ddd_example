using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.PartnerProfile
{
    public class ForRegionRnOffcSpecification : Specification<RnOffc>
    {
        private readonly HashSet<int> regionCodes;
        public ForRegionRnOffcSpecification(HashSet<int> regionCodes)
        {
            if (regionCodes == null || !regionCodes.Any())
                throw new ArgumentNullException(nameof(regionCodes));

            this.regionCodes = regionCodes;
        }
        public override Expression<Func<RnOffc, bool>> ToExpression() =>
            sp => regionCodes.Contains(sp.Lt.Agent.Organization.City.RegionCode.Value);
    }
}
