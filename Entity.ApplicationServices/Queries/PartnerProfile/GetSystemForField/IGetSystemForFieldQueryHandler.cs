using System;
using System.Threading.Tasks;

namespace Entity.ApplicationServices.Queries.PartnerProfile
{
    public interface IGetSystematicForEntityQueryHandler
    {
        Task<SystematicTypesDto> Handle(Guid validityId);
    }
}
