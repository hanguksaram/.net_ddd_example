using Entity.ApplicationServices.Queries.PartnerProfile.Models;
using System;
using System.Threading.Tasks;

namespace Entity.ApplicationServices.Queries.PartnerProfile
{
    public interface IGetEntityAuthorizationsStatusQueryHandler
    {
        Task<EntityEntityAuthorizationStatus> Handle(Guid validityId);
    }
}
