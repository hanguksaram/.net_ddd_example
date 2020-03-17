using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.PartnerProfile
{
    public class ForRegionEusEntitypecification : Specification<EusPlace>
    {
        private readonly HashSet<int?> _regionCodes;
        public ForRegionEusEntitypecification(HashSet<int> regionCodes)
        {
            if (regionCodes == null || !regionCodes.Any())
                throw new ArgumentNullException(nameof(regionCodes));

            _regionCodes = new HashSet<int?>(regionCodes.Select(x => (int?)x));
        }
        public override Expression<Func<EusPlace, bool>> ToExpression() =>
            sp => _regionCodes.Contains(sp.EusMainPoint.Organization.City.RegionCode.Value);
    }
}
