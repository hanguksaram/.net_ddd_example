namespace Entity.DataAccess
{
    public static class Converters
    {
        public static Domain.EntitySystematicType EpSystemToSystematicType(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Domain.EntitySystematicType.Unknown;

            if (value.Equals(EpSystems.Sabre, System.StringComparison.OrdinalIgnoreCase))
                return Domain.EntitySystematicType.SabreET;

            if (value.Equals(EpSystems.Ues, System.StringComparison.OrdinalIgnoreCase))
                return Domain.EntitySystematicType.UesET;

            if (value.Equals(EpSystems.Elio, System.StringComparison.OrdinalIgnoreCase))
                return Domain.EntitySystematicType.ElioET;

            return Domain.EntitySystematicType.Unknown;
        }


        public static Domain.EntitySystematicType BasePointSystemToSystematicType(SystematicType value)
        {
            switch (value)
            {
                case SystematicType.UesET:
                    return Domain.EntitySystematicType.UesET;
                case SystematicType.ElioET:
                    return Domain.EntitySystematicType.ElioET;
                case SystematicType.SabreET:
                    return Domain.EntitySystematicType.SabreET;
                default:
                    return Domain.EntitySystematicType.Unknown;
            }
        }
    }
}