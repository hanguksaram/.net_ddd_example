using System.Threading.Tasks;

namespace Entity.ApplicationServices.Queries.Entity01
{
    public interface IGetEntityPlaceByPlaceIdQueryHandler
    {
        Task<IGetEntityPlaceByPlaceIdQueryResult> Handle(GetEntityPlaceByPlaceIdQuery query);
    }
}
