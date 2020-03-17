using Entity.Common;

namespace Entity.ApplicationServices.Queries.PartnerProfile
{
    public class GetAvailableEntityQuery : ISortable
    {
        public string OrderBy { get; set; }
        public bool OrderDesc { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
