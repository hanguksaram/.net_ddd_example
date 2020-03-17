using Entity.ApplicationServices.Queries.PartnerProfile;
using Entity.DataAccess.Queries.PartnerProfile;
using Entity.Tests.Common;
using System;
using System.Threading.Tasks;
using Xunit;
using Entity.DataAccess.Tests.Fakes;

namespace Entity.DataAccess.Tests.Queries
{
    public class GetAvailableEntityQueryHandlerTests
    {
        private static GetAvailableEntityQueryHandler GetQuery(FakeDatetimeProvider fakeDtp, FakeListQueryHandler fakeQueryHandler) => new GetAvailableEntityQueryHandler(fakeDtp, fakeQueryHandler);
        private static int days_left_untill_Entity_become_available = 180;

        [Fact]
        public async Task Should_Return_Available_Entity_If_Free_Days_Passed()
        {

            var now = new DateTime(2020, 02, 01);
            var from = now.AddDays(-days_left_untill_Entity_become_available).AddDays(-2);
            var to = now.AddDays(-days_left_untill_Entity_become_available).AddDays(-1);

            var valNum = "421-2222-0";

            var item = Default.Entity
                .SetProperty(x => x.Number, valNum)
                .WithSingleEntityExpiration(from, to)
                .Value;


            var fakeDtp = new FakeDatetimeProvider
            {
                FakeNow = now
            };

            var fakeFactory = new FakeListQueryHandler();
            
            fakeFactory.QuerySource.Add(item);

            var query = GetQuery(fakeDtp, fakeFactory);

            var queryRequest = new GetAvailableEntityQuery();

            var result = await query.Handle(queryRequest);

            Assert.NotEmpty(result.Entity);
        }

        [Fact]
        public async Task Should_Return_Not_Available_Entity_If_Free_Days_Not_Passed()
        {


            var now = new DateTime(2020, 02, 01);
            var from = now.AddDays(-days_left_untill_Entity_become_available).AddDays(-2);
            var to = now.AddDays(-days_left_untill_Entity_become_available).AddDays(1);

            var valNum = "421-2222-0";

            var item = Default.Entity
                .SetProperty(x => x.Number, valNum)
                .WithSingleEntityExpiration(from, to)
                .Value;


            var fakeDtp = new FakeDatetimeProvider
            {
                FakeNow = now
            };

            var fakeFactory = new FakeListQueryHandler();

            fakeFactory.QuerySource.Add(item);

            var query = GetQuery(fakeDtp, fakeFactory);

            var queryRequest = new GetAvailableEntityQuery();

            var result = await query.Handle(queryRequest);

            Assert.Empty(result.Entity);
        }

        [Fact]
        public async Task Should_Not_Return_If_Active_EntityExpirations_Exists()
        {
            var now = new DateTime(2020, 02, 01);
            var to = now.AddDays(1);
            var from = now.AddDays(-1);


            var valNum = "421-2222-0";

            var item = Default.Entity
                .SetProperty(x => x.Number, valNum)
                .WithSingleEntityExpiration(from, to)
                .Value;


            var fakeDtp = new FakeDatetimeProvider
            {
                FakeNow = now
            };

            var fakeFactory = new FakeListQueryHandler();

            fakeFactory.QuerySource.Add(item);

            var query = GetQuery(fakeDtp, fakeFactory);

            var queryRequest = new GetAvailableEntityQuery();

            var result = await query.Handle(queryRequest);

            Assert.Empty(result.Entity);
        }

        [Fact]
        public async Task Should_Not_Return_If_Control_Number_Is_Invalid()
        {
            var to = new DateTime(2020, 02, 01); ;

            var valNum = "421-2222-3";

            var item = Default.Entity
                .SetProperty(x => x.Number, valNum)              
                .Value;

            var fakeDtp = new FakeDatetimeProvider
            {
                FakeNow = to
            };

            var fakeFactory = new FakeListQueryHandler();

            fakeFactory.QuerySource.Add(item);

            var query = GetQuery(fakeDtp, fakeFactory);

            var queryRequest = new GetAvailableEntityQuery();

            var result = await query.Handle(queryRequest);

            Assert.Empty(result.Entity);
        }

        [Fact]
        public async Task Should_Return_If_Control_Number_Is_Valid()
        {
            var to = new DateTime(2020, 02, 01); ;

            var valNum = "421-2222-0";

            var item = Default.Entity
                .SetProperty(x => x.Number, valNum)
                .Value;

            var fakeDtp = new FakeDatetimeProvider
            {
                FakeNow = to
            };

            var fakeFactory = new FakeListQueryHandler();

            fakeFactory.QuerySource.Add(item);

            var query = GetQuery(fakeDtp, fakeFactory);

            var queryRequest = new GetAvailableEntityQuery();

            var result = await query.Handle(queryRequest);

            var number = Assert.Single(result.Entity);
            Assert.Equal(valNum, number.Number);
        }
    }
}

