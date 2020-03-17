using Entity.Common;

namespace Entity.ApplicationServices.Queries.PartnerProfile
{
    public class GetEntityByTermQuery : ISortable
    {
        public string Term { get; set; }
        public IdentityFilter IdentityFilter { get; set; }
        public bool OnlyWithAuthorizedAccess { get; set; }


        public int Skip { get; set; }
        public int Take { get; set; }
        public string OrderBy { get; set; }
        public bool OrderDesc { get; set; }
    }
}
