using System.ComponentModel;

namespace Entity.Domain
{
    public enum SaTypes : byte
    {
        Default = 0,
        [Description("T")]
        c2C = 1,
        [Description("s")]
        c2B = 2,
        [Description("l")]
        c2A = 3
    }
}
