using System.Threading.Tasks;

namespace Entity.ApplicationServices.Queries.PartnerProfile
{
    public interface IGetPlaceIdsByTermQueryHandler
    {
        Task<IGetPlaceIdsByTermQueryResult> Handle(GetPlaceIdsByTermQuery query);
    }
}
