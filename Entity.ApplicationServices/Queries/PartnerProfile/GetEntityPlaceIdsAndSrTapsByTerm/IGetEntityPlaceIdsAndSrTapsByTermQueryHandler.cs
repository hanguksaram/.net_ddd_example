using System.Threading.Tasks;

namespace Entity.ApplicationServices.Queries.PartnerProfile
{
    public interface IGetEntityPlaceIdsAndSrnTapsByTermQueryHandler
    {
        Task<IGetEntityPlaceIdsAndSrnTapsByTermQueryResult> Handle(GetEntityPlaceIdsAndSrnTapsByTermQuery query);
    }
}
