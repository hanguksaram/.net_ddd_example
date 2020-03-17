using System;

namespace Entity.CrossCutting
{
    public interface IDatetimeProvider
    {
        DateTime GetLocalNow();
    }
}
