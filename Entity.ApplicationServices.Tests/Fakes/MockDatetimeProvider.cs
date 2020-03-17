using System;
using Entity.CrossCutting;

namespace Entity.ApplicationServices.Tests.Fakes
{
    internal class MockDatetimeProvider : IDatetimeProvider
    {
        public DateTime LocalNow = DateTime.Now;
        public DateTime GetLocalNow()
        {
            return LocalNow;
        }
    }
}
