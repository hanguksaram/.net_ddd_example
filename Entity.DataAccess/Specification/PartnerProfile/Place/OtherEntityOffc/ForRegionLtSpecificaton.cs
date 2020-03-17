using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.PartnerProfile
{
    public class ForRegionEpSpecificaton : Specification<Ep>
    {
        private readonly HashSet<int> regionCodes;
        public ForRegionEpSpecificaton(HashSet<int> regionCodes)
        {
            if (regionCodes == null || !regionCodes.Any())
                throw new ArgumentNullException(nameof(regionCodes));

            this.regionCodes = regionCodes;
        }
        public override Expression<Func<Ep, bool>> ToExpression() =>
            sp => regionCodes.Contains(sp.Agent.Organization.Stc.RegionCode.Value);
    }
}
