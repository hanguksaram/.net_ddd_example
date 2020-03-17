using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.PartnerProfile
{
    public class ForRegionEntityEntitypecification : Specification<EntityExpiration>
    {
        private readonly HashSet<int?> _regionCodes;
        public ForRegionEntityEntitypecification(HashSet<int> regionCodes)
        {
            if (regionCodes == null || !regionCodes.Any())
                throw new ArgumentNullException(nameof(regionCodes));

            _regionCodes = new HashSet<int?>(regionCodes.Select(x => (int?)x));
        }   
        public override Expression<Func<EntityExpiration, bool>> ToExpression() => 
            sp => _regionCodes.Contains(sp.EntityMainPoint.EusMainPoint.Organization.City.RegionCode);

    }
}
