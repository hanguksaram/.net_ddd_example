using Entity.DataAccess.ServicesImpl;
using Entity.Tests.Common;
using System;
using System.Threading.Tasks;
using Xunit;
using Entity.DataAccess.Tests.Fakes;
using System.Linq;

namespace Entity.DataAccess.Tests.Services
{
    public class EntityPlaceApplicationServiceTests
    {
        private static EntityPlaceApplicationService GetQuery(FakeListQueryHandler fakeQueryHandler) => new EntityPlaceApplicationService(new FakeDatetimeProvider(), fakeQueryHandler);

        private static string PoolNumber = "ГРТБ70";

        [Fact]
        public async Task Should_Find_Srn_Pool_If_PoolNumber_Already_Exists_And_Active()
        {
            var Ep = Default.SrnEp
              .WithTap(PoolNumber)
              .Value;

            var tap = Ep.SrnTaps.Single();

            var fakeFactory = new FakeListQueryHandler();
            fakeFactory.QuerySource.Add(tap);

            var query = GetQuery(fakeFactory);

            var result = await query.CheckIfEntityPlaceExistsByIdentificator(PoolNumber, null);

            Assert.True(result);
        }

        [Fact]
        public async Task Should_Not_Find_Srn_Pool_If_PoolNumber_Already_Exists_But_Has_Inactive_Status()
        {
            var Ep = Default.SrnEp
              .SetProperty(x => x.SrnStatus, false)
              .WithTap(PoolNumber)
              .Value;

            var tap = Ep.SrnTaps.Single();

            var fakeFactory = new FakeListQueryHandler();
            fakeFactory.QuerySource.Add(tap);

            var query = GetQuery(fakeFactory);

            var result = await query.CheckIfEntityPlaceExistsByIdentificator(PoolNumber, null);

            Assert.False(result);
        }

        [Fact]
        public async Task Should_Not_Find_Srn_Pool_If_PoolNumber_Does_Not_Exists()
        {
            var fakeFactory = new FakeListQueryHandler();

            var query = GetQuery(fakeFactory);

            var result = await query.CheckIfEntityPlaceExistsByIdentificator(PoolNumber, null);

            Assert.False(result);
        }

        [Theory]
        [InlineData("  ")]
        [InlineData("")]
        [InlineData(null)]
        public async Task Should_Throw_Exception_If_Srn_Tap_Format_Does_Not_Correct(string fakePlaceId)
        {
            var fakeFactory = new FakeListQueryHandler();

            var query = GetQuery(fakeFactory);

            await Assert.ThrowsAsync<ArgumentNullException>(() => query.CheckIfEntityPlaceExistsByIdentificator(fakePlaceId, null));
        }

    }
}
