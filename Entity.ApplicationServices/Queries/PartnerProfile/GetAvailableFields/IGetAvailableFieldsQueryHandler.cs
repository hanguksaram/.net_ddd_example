using System.Threading.Tasks;

namespace Entity.ApplicationServices.Queries.PartnerProfile
{
    public interface IGetAvailableEntityQueryHandler
    {
        Task<IGetAvailableEntityQueryResult> Handle(GetAvailableEntityQuery query);
    }
}
