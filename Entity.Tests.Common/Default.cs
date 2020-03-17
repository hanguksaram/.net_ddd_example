using Entity.ApplicationServices.Services;
using Entity.DataAccess;
using System;
using System.Collections.Generic;

namespace Entity.Tests.Common
{
    public static class Default
    {
        public static class Data {
            public static string DefaultBasePointId = "PS00001";
            public static string DefaultSrnGrp = "4210012345";
            public static string DefaultSrnAgn = "AGN12345";
            public static string DefaultEntityNumber = "421-0000-0";
            public static string DefaultEntityPlaceId = "OVB111";
            public static string DefaultSrnPoolNumber = "OVB111";
            public static string DefaultEntitySystem = EpSystems.Ues;
            public static string DefaultUesPlaceId = "KHVtest2019";
        };


        public static TestData<UesBasePoint> UesBasePoint => new TestData<UesBasePoint>(new UesBasePoint {
            Id = Guid.NewGuid(),
            BasePointId= Data.DefaultBasePointId,
            Address = string.Empty,
            AddressLatin = string.Empty,
        });
        public static TestData<UesPlace> UesPlace => new TestData<UesPlace>(new UesPlace
        {
            Id = Guid.NewGuid(),
            PlaceId = Data.DefaultUesPlaceId

        });
        public static TestData<EntityEntityDataTransferModel> EntityTransferModel => new TestData<EntityEntityDataTransferModel>(new EntityEntityDataTransferModel
        {
            Number = "421-8264-1",
            Systematic = Domain.EntitySystematicType.Unknown
        });

        public static TestData<BasePoint> BasePoint => new TestData<BasePoint>(new BasePoint {
            BasePointGuid = Guid.NewGuid()
        });

        public static TestData<Entity> Entity => new TestData<Entity>(new Entity
        {
            EntityGuid = Guid.NewGuid(),
            Number = Data.DefaultEntityNumber
        });

        public static TestData<Ep> EntityEp => new TestData<Ep>(new Ep
        {
            EpGuid = Guid.NewGuid(),
            EpSystem = Data.DefaultEntitySystem,
            PlaceId = Data.DefaultEntityPlaceId
        });

        public static TestData<Ep> SrnEp => new TestData<Ep>(new Ep
        {
            EpGuid = Guid.NewGuid(),
            EpSystem = EpSystems.Srn,
            SrnStatus = true,
            AGN = Data.DefaultSrnAgn,
            GRP = Data.DefaultSrnGrp,
            SrnTaps = new List<SrnOffc>()
        });
    }

    public static class DefaultExtensions
    {
        public static TestData<Ep> WithTap(this TestData<Ep> Srn, string tap)
        {
            var offc = new SrnOffc
            {
                SrnOffcGuid = Guid.NewGuid(),
                Offc = tap,
                EpGuid = Srn.Value.EpGuid,
                Ep = Srn.Value
            };

            Srn.Value.SrnTaps.Add(offc);

            return Srn;
        }

        public static TestData<UesBasePoint> WithEntityBasePoint(this TestData<UesBasePoint> testdata, Action<TestData<BasePoint>> actions = null)
        {
            var spTestData = Default.BasePoint;
            spTestData.Value.UesBasePointGuid = testdata.Value.Id;
            spTestData.Value.UesBasePoint = testdata.Value;
            actions?.Invoke(spTestData);
            testdata.Value.EntityBasePoints = new List<BasePoint> { spTestData.Value };
            return testdata;
        }

        public static TestData<BasePoint> WithSingleReferenceEntity(this TestData<BasePoint> testdata, DateTime from, DateTime to, Action<TestData<Entity>> actions = null)
        {
            var spTestData = Default.Entity;
            actions?.Invoke(spTestData);

            var exp = new EntityExpiration
               {
                   EntityGuid = spTestData.Value.EntityGuid,
                   Entity = spTestData.Value,
                   EntityExpirationGuid = Guid.NewGuid(),
                   ValidFrom = from,
                   ValidTo = to,
                   IsActive = true,
                    BasePointGuid = testdata.Value.BasePointGuid,
                    EntityBasePoint = testdata.Value
               };

            spTestData.Value.EntityExpirations.Add(exp);
            testdata.Value.EntityExpirations.Add(exp);


            return testdata;
        }
        public static TestData<Entity> WithSingleEntityExpiration(this TestData<Entity> testdata, DateTime from, DateTime to, Action<TestData<Entity>> actions = null)
        {
            var spTestData = Default.Entity;
            actions?.Invoke(spTestData);

            var exp = new EntityExpiration
            {
                EntityGuid = spTestData.Value.EntityGuid,
                Entity = spTestData.Value,
                EntityExpirationGuid = Guid.NewGuid(),
                ValidFrom = from,
                ValidTo = to,
                IsActive = true,
            };

            spTestData.Value.EntityExpirations.Add(exp);
            testdata.Value.EntityExpirations.Add(exp);

            return testdata;
        }

        public static TestData<BasePoint> WithEntityEp(this TestData<BasePoint> testdata, Action<TestData<Ep>> actions = null)
        {
            var spTestData = Default.EntityEp;

            actions?.Invoke(spTestData);

            testdata.Value.Systematic = Enum.TryParse<SystematicType>(spTestData.Value.EpSystem + "ET", out var res) ? res : SystematicType.Default;
            testdata.Value.Eps.Add(spTestData.Value);
            return testdata;
        }

        public static TestData<BasePoint> WithSrnEp(this TestData<BasePoint> testdata, Action<TestData<Ep>> actions = null)
        {
            var spTestData = Default.EntityEp;
            actions?.Invoke(spTestData);
            spTestData.Value.EpSystem = EpSystems.Srn;
            testdata.Value.Systematic = SystematicType.Srn;
            testdata.Value.Eps.Add(spTestData.Value);
            return testdata;
        }
    }
}
