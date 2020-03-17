using System;

namespace Entity.CrossCutting
{
    public class DefaultDatetimeProvider : IDatetimeProvider
    {
        public static DateTime now => DateTime.Now;
        public DateTime GetLocalNow() => now;
    }

}
