using Entity.Domain;
using System;

namespace Entity.ApplicationServices
{
    public enum LocationTypesDto : byte
    {
        Place = 0,
        Town = 1,
        Online = 2
    }
    public enum SiteAudienceTypesDto : byte
    {
        Default = 0,
        B2C = 1,
        B2B = 2,
        B2A = 3
    }
    public enum SystematicTypesDto : byte
    {

        Default = 0,
        UesET = 1,
        ElioET = 2,
        Srn = 3,
        SabreET = 4,
        Uestest = 5
    }

    public static class EnumDtoExtensions {
        public static EntitySystematicType ToEntitySystematic(this SystematicTypesDto dataAccessRsystem)
        {
            switch (dataAccessRsystem)
            {
                case SystematicTypesDto.UesET:
                    return EntitySystematicType.UesET;
                case SystematicTypesDto.ElioET:
                    return EntitySystematicType.ElioET;
                case SystematicTypesDto.SabreET:
                    return EntitySystematicType.SabreET;
                case SystematicTypesDto.Srn:
                    return EntitySystematicType.Srn;
                default:
                    return EntitySystematicType.Unknown;
            }
        }
        public static LocationTypes ToDomainType(this LocationTypesDto locationTypesDto)
        {
            switch (locationTypesDto)
            {
                case LocationTypesDto.Place:
                    return LocationTypes.Place;
                case LocationTypesDto.Town:
                    return LocationTypes.Town;
                case LocationTypesDto.Online:
                    return LocationTypes.Online;
                default:
                    throw new ArgumentOutOfRangeException(nameof(locationTypesDto), locationTypesDto, "Cannot convert value");
            }
        }
        public static SiteAudienceTypes ToDomainType(this SiteAudienceTypesDto siteAudienceTypesDto)
        {
            switch (siteAudienceTypesDto)
            {
                case SiteAudienceTypesDto.Default:
                    return SiteAudienceTypes.Default;
                case SiteAudienceTypesDto.B2A:
                    return SiteAudienceTypes.B2A;
                case SiteAudienceTypesDto.B2B:
                    return SiteAudienceTypes.B2B;
                case SiteAudienceTypesDto.B2C:
                    return SiteAudienceTypes.B2C;
                default:
                    throw new ArgumentOutOfRangeException(nameof(siteAudienceTypesDto), siteAudienceTypesDto, "Cannot convert value");
            }
        }
    }
}
