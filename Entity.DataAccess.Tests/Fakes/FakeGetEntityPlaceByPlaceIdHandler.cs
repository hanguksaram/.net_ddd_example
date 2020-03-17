using System.Threading.Tasks;
using Entity.ApplicationServices.Queries.Entity01;

namespace Entity.DataAccess.Tests.Fakes
{
    public class FakeGetEntityPlaceByPlaceIdHandler : IGetEntityPlaceByPlaceIdQueryHandler
    {
        public Task<IGetEntityPlaceByPlaceIdQueryResult> Handle(GetEntityPlaceByPlaceIdQuery query) => Task.FromResult<IGetEntityPlaceByPlaceIdQueryResult>(null);
    }
}
