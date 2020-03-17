using Entity.ApplicationServices;
using Entity.ApplicationServices.Queries.PartnerProfile;
using Entity.CrossCutting.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Entity.DataAccess.Queries.PartnerProfile
{
    public class GetSysForEntityQueryHandler : IGetSysForEntityQueryHandler
    {
        private readonly IListQueryHandler _queryHandler;

        public GetSysForEntityQueryHandler(IListQueryHandler queryHandler)
        {
            _queryHandler = queryHandler;
        }

        public async Task<SysTypesDto> Handle(Guid validityId)
        {
            if (validityId == Guid.Empty)
            {
                throw new ArgumentNullException();
            }

            var queryResult = await _queryHandler.HandleAsync<EntityExpiration, SysType>(v => v.EntityExpirationGuid == validityId, v => v.EntityMainPoint.Sys);

            if (!queryResult.Any())
            {
                throw new NotFoundException($"Валидатор с validityId {validityId} не найден для определения типа гдс");
            }

            var Sys = queryResult.First();

            return Sys.ToSysDto();
       
        }      
    }
    internal static class SysExtension
    {
        public static SysTypesDto ToSysDto(this SysType rst)
        {
            switch (rst)
            {
                case SysType.EusET:
                    return SysTypesDto.EusET;
                case SysType.ElioET:
                    return SysTypesDto.ElioET;
                case SysType.Rn:
                    return SysTypesDto.Rn;
                case SysType.SabreET:
                    return SysTypesDto.SabreET;
                case SysType.EusTest:
                    return SysTypesDto.EusTest;
                case SysType.Default:
                    return SysTypesDto.Default;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
