using System;
using System.Threading.Tasks;
using Entity.ApplicationServices.Queries.PartnerProfile;
using Entity.ApplicationServices.Queries.PartnerProfile.Models;

namespace Entity.ApplicationServices.Tests.Fakes
{
    class GetEntityAuthorizationsStatusMockQueryHandler : IGetEntityAuthorizationsStatusQueryHandler
    {
        public Task<EntityEntityAuthorizationStatus> Handle(Guid validityId)
        {
            throw new NotImplementedException();
        }
    }
}
