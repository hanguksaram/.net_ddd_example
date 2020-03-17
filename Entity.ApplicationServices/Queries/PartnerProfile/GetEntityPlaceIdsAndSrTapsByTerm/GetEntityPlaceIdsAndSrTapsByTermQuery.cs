using Entity.Common;

namespace Entity.ApplicationServices.Queries.PartnerProfile
{
    public class GetEntityPlaceIdsAndSrnTapsByTermQuery : ISortable
    {   
        public string Term { get; set; }
        public IdentityFilter IdentityFilter { get; set; }

        public int Skip { get; set; }
        public int Take { get; set; }
        public string OrderBy { get; set; }
        public bool OrderDesc { get; set; }
    }
}
