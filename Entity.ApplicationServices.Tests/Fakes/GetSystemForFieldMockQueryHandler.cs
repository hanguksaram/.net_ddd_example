using System;
using System.Threading.Tasks;
using Entity.ApplicationServices.Queries.PartnerProfile;

namespace Entity.ApplicationServices.Tests.Fakes
{
    class GetSystematicForEntityMockQueryHandler : IGetSystematicForEntityQueryHandler
    {
        public Task<SystematicTypesDto> Handle(Guid validityId)
        {
            throw new NotImplementedException();
        }
    }
}
