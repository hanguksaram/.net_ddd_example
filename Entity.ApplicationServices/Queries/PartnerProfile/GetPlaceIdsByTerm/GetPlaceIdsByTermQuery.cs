using Entity.Common;

namespace Entity.ApplicationServices.Queries.PartnerProfile
{
    public class GetPlaceIdsByTermQuery : ISortable
    {   
        public string Term { get; set; }
        public IdentityFilter IdentityFilter { get; set; }

        public string OrderBy { get; set; }
        public bool OrderDesc { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
