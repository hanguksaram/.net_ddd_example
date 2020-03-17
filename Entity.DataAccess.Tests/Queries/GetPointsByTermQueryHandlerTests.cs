using Entity.ApplicationServices.Queries.Entity01;
using Entity.Tests.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Entity.DataAccess.Queries.Entity01;
using Entity.Domain;

namespace Entity.DataAccess.Tests.Queries
{
    public class GetEntitySalesByTermQueryHandlerTests
    {
        private static GetEntitySalesByTermQueryHandler GetQuery(Fakes.FakeDatetimeProvider fakeDtp, Fakes.FakeListQueryHandler fakeQueryHandler)
        {
            return new GetEntitySalesByTermQueryHandler(fakeDtp, fakeQueryHandler, new NullObjectFeatureChecker());
        }

        [Fact]
        public async Task ShouldReturnOne_IfTermEqualsToEntity_AndEntityIsAuthorized()
        {
            var now = DateTime.Now;

            var aCode = "999";
            var valNum = "421-2222-3";

            var item = Default.UesBasePoint
                .SetProperty(x => x.DealCode, aCode)
                .WithEntityBasePoint(c =>
                    c.WithSingleReferenceEntity(now.Date, now.Date, v => v.SetProperty(x => x.Number, valNum))
                     .WithEntityEp())
                .Value;

            var veItem = item.EntityBasePoints.First().EntityExpirations.First();

            var fakeDtp = new Fakes.FakeDatetimeProvider
            {
                FakeNow = now
            };

            var fakeFactory = new Fakes.FakeListQueryHandler();
            fakeFactory.QuerySource.Add(item);
            fakeFactory.QuerySource.Add(veItem);

            var query = GetQuery(fakeDtp, fakeFactory);

            var queryRequest = new GetEntitySalesByTermQuery(aCode, valNum, EntitySalesByTermQueryType.SearchByEntityNumber);

            var result = await query.Handle(queryRequest);

            Assert.Single(result.EntitySales);
        }

        [Fact]
        public async Task ShouldReturnOne_IfTermEqualsToEntity_AndEntityIsNotAuthorized()
        {
            var now = DateTime.Now;

            var aCode = "999";
            var valNum = "421-2222-3";

            var item = Default.UesBasePoint
                .SetProperty(x => x.DealCode, aCode)
                .WithEntityBasePoint(c => {
                    c.WithEntityEp();
                    c.WithSingleReferenceEntity(now.Date, now.Date, v => v.SetProperty(x => x.Number, valNum));
                }
                  )
                .Value;

            var veItem = item.EntityBasePoints.First()
                .EntityExpirations.First();

            var fakeDtp = new Fakes.FakeDatetimeProvider
            {
                FakeNow = now
            };

            var fakeFactory = new Fakes.FakeListQueryHandler();
            fakeFactory.QuerySource.Add(item);
            fakeFactory.QuerySource.Add(veItem);

            var query = GetQuery(fakeDtp, fakeFactory);

            var queryRequest = new GetEntitySalesByTermQuery(aCode, valNum, EntitySalesByTermQueryType.SearchByEntityNumber);

            var result = await query.Handle(queryRequest);

            Assert.Single(result.EntitySales);
        }

        [Fact]
        public async Task ShouldReturnEmpty_IfTermEqualsToEntity_ButSearchByBasePoint()
        {
            var now = DateTime.Now;

            var aCode = "999";
            var valNum = "421-2222-3";

            var item = Default.UesBasePoint
                .SetProperty(x => x.DealCode, aCode)
                .WithEntityBasePoint(c =>
                    c.WithSingleReferenceEntity(now.Date, now.Date, v => v.SetProperty(x => x.Number, valNum))
                     .WithEntityEp())
                .Value;

            var veItem = item.EntityBasePoints.First().EntityExpirations.First();

            var fakeDtp = new Fakes.FakeDatetimeProvider
            {
                FakeNow = now
            };

            var fakeFactory = new Fakes.FakeListQueryHandler();
            fakeFactory.QuerySource.Add(item);
            fakeFactory.QuerySource.Add(veItem);

            var query = GetQuery(fakeDtp, fakeFactory);

            var queryRequest = new GetEntitySalesByTermQuery(aCode, valNum, EntitySalesByTermQueryType.SearchByEntitySalesInfo);

            var result = await query.Handle(queryRequest);

            Assert.Empty(result.EntitySales);
        }

        [Fact]
        public async Task ShouldReturnEmpty_IfTermEqualsToBasePointId_ButSearchByEntity()
        {
            var now = DateTime.Now;

            var aCode = "999";
            var spId = "PS11111";

            var item = Default.UesBasePoint
                .SetProperty(x => x.DealCode, aCode)
                .SetProperty(x => x.BasePointId, spId)
                .WithEntityBasePoint(c =>
                    c.WithSingleReferenceEntity(now.Date, now.Date)
                     .WithEntityEp())
                .Value;

            var veItem = item.EntityBasePoints.First().EntityExpirations.First();

            var fakeDtp = new Fakes.FakeDatetimeProvider
            {
                FakeNow = now
            };

            var fakeFactory = new Fakes.FakeListQueryHandler();
            fakeFactory.QuerySource.Add(item);
            fakeFactory.QuerySource.Add(veItem);

            var query = GetQuery(fakeDtp, fakeFactory);

            var queryRequest = new GetEntitySalesByTermQuery(aCode, spId, EntitySalesByTermQueryType.SearchByEntityNumber);

            var result = await query.Handle(queryRequest);

            Assert.Empty(result.EntitySales);
        }

        [Fact]
        public async Task ShouldReturnOne_IfTermEqualsToBasePointId()
        {
            var now = DateTime.Now;

            var aCode = "999";
            var spId = "PS11111";

            var item = Default.UesBasePoint
                .SetProperty(x => x.DealCode, aCode)
                .SetProperty(x => x.BasePointId, spId)
                .WithEntityBasePoint(c =>
                    c.WithSingleReferenceEntity(now.Date, now.Date)
                     .WithEntityEp())
                .Value;

            var veItem = item.EntityBasePoints.First().EntityExpirations.First();

            var fakeDtp = new Fakes.FakeDatetimeProvider
            {
                FakeNow = now
            };

            var fakeFactory = new Fakes.FakeListQueryHandler();
            fakeFactory.QuerySource.Add(item);
            fakeFactory.QuerySource.Add(veItem);

            var query = GetQuery(fakeDtp, fakeFactory);

            var queryRequest = new GetEntitySalesByTermQuery(aCode, spId, EntitySalesByTermQueryType.SearchByEntitySalesInfo);

            var result = await query.Handle(queryRequest);

            Assert.Single(result.EntitySales);
        }
        
        [Fact]
        public async Task ShouldReturnEmpty_IfEntityIsExpired()
        {
            var now = DateTime.Now;

            var aCode = "999";
            var valNum = "421-2222-3";

            var item = Default.UesBasePoint
                .SetProperty(x => x.DealCode, aCode)
                .WithEntityBasePoint(c =>
                    c.WithSingleReferenceEntity(now.Date.AddDays(-1), now.Date.AddDays(-1), v => v.SetProperty(x => x.Number, valNum))
                     .WithEntityEp())
                .Value;

            var veItem = item.EntityBasePoints.First().EntityExpirations.First();

            var fakeDtp = new Fakes.FakeDatetimeProvider
            {
                FakeNow = now
            };

            var fakeFactory = new Fakes.FakeListQueryHandler();
            fakeFactory.QuerySource.Add(item);
            fakeFactory.QuerySource.Add(veItem);

            var query = GetQuery(fakeDtp, fakeFactory);

            var queryRequest = new GetEntitySalesByTermQuery(aCode, valNum, EntitySalesByTermQueryType.SearchByEntityNumber);

            var result = await query.Handle(queryRequest);

            Assert.Empty(result.EntitySales);
        }

        [Fact]
        public async Task ShouldReturnEmpty_IfHasOnlySrnEp()
        {
            var now = DateTime.Now;

            var aCode = "999";
            var valNum = "421-2222-3";

            var item = Default.UesBasePoint
                .SetProperty(x => x.DealCode, aCode)
                .WithEntityBasePoint(c =>
                    c.WithSingleReferenceEntity(now.Date, now.Date, v => v.SetProperty(x => x.Number, valNum))
                     .WithSrnEp())
                .Value;

            var veItem = item.EntityBasePoints.First().EntityExpirations.First();

            var fakeDtp = new Fakes.FakeDatetimeProvider
            {
                FakeNow = now
            };

            var fakeFactory = new Fakes.FakeListQueryHandler();
            fakeFactory.QuerySource.Add(item);
            fakeFactory.QuerySource.Add(veItem);

            var query = GetQuery(fakeDtp, fakeFactory);

            var queryRequest = new GetEntitySalesByTermQuery(aCode, valNum, EntitySalesByTermQueryType.SearchByEntityNumber);

            var result = await query.Handle(queryRequest);

            Assert.Empty(result.EntitySales);
        }

        [Fact]
        //todo refactor
        public async Task ShouldReturnExpectedValues()
        {
            var now = DateTime.Now;

            var aCode = "999";
            var spId = "PS99999";
            var aspGuid = Guid.NewGuid();
            var valNum = "421-2222-3";
            var adr = "Лорем ипсум";
            var adrLat = "Lorem ipsum";
            var id = Guid.NewGuid();
            var valExpGuid = Guid.NewGuid();
            var offId = "OVB999";
            var EpType = EpSystems.Ues;

            var item = Default.UesBasePoint
                .SetProperty(x => x.Id, aspGuid)
                .SetProperty(x => x.BasePointId, spId)
                .SetProperty(x => x.DealCode, aCode)
                .SetProperty(x => x.Address, adr)
                .SetProperty(x => x.AddressLatin, adrLat)
                .WithEntityBasePoint(c =>
                    c.WithSingleReferenceEntity(now.Date, now.Date, v => v.SetProperty(x => x.Number, valNum))
                     .WithEntityEp(v => v.SetProperty(t => t.PlaceId, offId).SetProperty(t => t.EpSystem, EpType)))
                .Value;

            var veItem = item.EntityBasePoints.First().EntityExpirations.First();
            veItem.EntityExpirationGuid = valExpGuid;

            var fakeDtp = new Fakes.FakeDatetimeProvider
            {
                FakeNow = now
            };

            var fakeFactory = new Fakes.FakeListQueryHandler();
            fakeFactory.QuerySource.Add(item);
            fakeFactory.QuerySource.Add(veItem);

            var query = GetQuery(fakeDtp, fakeFactory);

            var queryRequest = new GetEntitySalesByTermQuery(aCode, valNum, EntitySalesByTermQueryType.SearchByEntityNumber);

            var result = await query.Handle(queryRequest);

            Assert.NotNull(result);
            //todo refactor to srp tests

            var value = Assert.Single(result.EntitySales);
            Assert.Equal(spId, value.BasePointId);
            Assert.Equal(aspGuid, value.Id);
            Assert.Equal(adr, value.Address);
            Assert.Equal(adrLat, value.AddressLatin);

            var val = Assert.Single(value.Entity);
            Assert.Equal(valNum, val.Number);
            Assert.Equal(valExpGuid, val.Id);
            Assert.Equal(EntitySystematicType.UesET, val.SystematicType);

            var offc = Assert.Single(val.Places);
            Assert.Equal(offId, offc);
        }
    }
}
