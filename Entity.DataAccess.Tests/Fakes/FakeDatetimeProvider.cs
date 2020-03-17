using System;
using Entity.CrossCutting;

namespace Entity.DataAccess.Tests.Fakes
{
    internal class FakeDatetimeProvider : IDatetimeProvider
    {
        public DateTime? FakeNow { get; set; }

        public DateTime GetLocalNow()
        {
            return FakeNow ?? DateTime.Now;
        }
    }
}
