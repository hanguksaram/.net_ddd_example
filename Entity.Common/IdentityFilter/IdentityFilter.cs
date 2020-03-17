using System.Collections.Generic;

namespace Entity.Common
{
    public class IdentityFilter
    {
        public HashSet<string> DealCodes { get; set; } = new HashSet<string>();
        public HashSet<int> RegionCodes { get; set; } = new HashSet<int>();
        public bool IsAdmin { get; set; }
    }
}
