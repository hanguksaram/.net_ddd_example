using System.Threading.Tasks;

namespace Entity.ApplicationServices.Queries.PartnerProfile
{
    public interface IGetEntityByTermQueryHandler
    {
        Task<IGetEntityByTermQueryResult> Handle(GetEntityByTermQuery query);
    }
}
