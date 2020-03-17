using Autofac;
using Entity.ApplicationServices;
using Entity.DataAccess.Repository;
using Entity.Domain;
using Entity.Domain.DataAccessModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Xunit;

namespace Entity.DataAccess.Tests
{
    public class EntitySalesRepositoryTests : IDisposable
    {
        private const string BasePointId = "~~~~~~~";
        private const string AgencyCode = "~~~~~~~~~~~~~~~~";
        private const string FirstEntityNumber = "~~~~~~~~~~~~~~1";
        private const string SecondEntityNumber = "~~~~~~~~~~~~~~2";
        private const string NonActiveEntityNumber = "~~~~~~~~~~~~~~3";
        private const string NewEntityEntityNumber = "421-3110-6";
        private const string PlaceId = "0123";
        private const string NewPlaceId = "TEST";
        private static Guid FakeBasePointId = Guid.NewGuid(); 
        private static Guid ExistedBasePointIdWithElioEntity = Guid.Parse("8396B9F1-ABC0-4ED5-853B-094459770DB6");

        private readonly IContainer _container;
        private readonly DateTime _targetSqlDate = new DateTime(1753, 1, 2);
        private readonly DateTime _minSqlDate = new DateTime(1753, 1, 1);
        private readonly UesBasePoint _UesBasePoint;
        private readonly BasePoint _UesEtBasePoint;
        private readonly BasePoint _EntityEntityalePoint;
        private readonly BasePoint _SrnEntity;
        private readonly Entity _firstEntity;
        private readonly Entity _secondEntity;
        private readonly Entity _notActiveEntity;
        private readonly Entity _expiredEntity;
        private readonly Entity _futureEntity;
        private readonly EntityEntity _newEntityEntity;
        private readonly EntityEntity _newSrnEntity;
        private readonly EntityEntity _newSabreEtEntity;
        private readonly Ep _EntityEp;
        private readonly TransactionScope _scope;

        public EntitySalesRepositoryTests()
        {
            _scope = new TransactionScope(TransactionScopeOption.RequiresNew, TransactionScopeAsyncFlowOption.Enabled);
            _container = CompositionRoot.GetContainer();
            _EntityEp = new Ep
            {
                EpGuid = Guid.NewGuid(),
                ValidFrom = _targetSqlDate,
                ValidTo = _targetSqlDate,
                PlaceId = PlaceId,
                EpSystem = string.Empty,
                OFFC = string.Empty,
                AutorizeDate = _targetSqlDate,
                AK = string.Empty,
                Comment = string.Empty,
                CreatedDate = _minSqlDate,
                ModifiedDate = _minSqlDate
            };
            _EntityEp = new Ep
            {
                EpGuid = Guid.NewGuid(),
                ValidFrom = _targetSqlDate,
                ValidTo = _targetSqlDate,
                PlaceId = PlaceId,
                EpSystem = string.Empty,
                OFFC = string.Empty,
                AutorizeDate = _targetSqlDate,
                AK = string.Empty,
                Comment = string.Empty,
                CreatedDate = _minSqlDate,
                ModifiedDate = _minSqlDate
            };
            _EntityEp = new Ep
            {
                EpGuid = Guid.NewGuid(),
                ValidFrom = _targetSqlDate,
                ValidTo = _targetSqlDate,
                PlaceId = PlaceId,
                EpSystem = string.Empty,
                OFFC = string.Empty,
                AutorizeDate = _targetSqlDate,
                AK = string.Empty,
                Comment = string.Empty,
                CreatedDate = _minSqlDate,
                ModifiedDate = _minSqlDate
            };
            _EntityEntityalePoint = new BasePoint
            {
                BasePointGuid = FakeBasePointId,
                Deal = new Deal
                {
                    Code = AgencyCode
                },
                TownCode = "OVB",
                Address = string.Empty,
                BasePointTypeGuid = Guid.Parse("18D2C60A-9718-4ED0-BA31-8E563A17FF4A"),
                CreatedDate = _minSqlDate,
                ModifiedDate = _minSqlDate,
                Systematic = SystematicType.UesET,
                Eps = new[]
                {
                    _EntityEp
                }
            };
           _UesEtBasePoint = new BasePoint
            {
                BasePointGuid = FakeBasePointId,
                Deal = new Deal
                {
                    Code = AgencyCode
                },
                TownCode = "OVB",
                Address = string.Empty,
                BasePointTypeGuid = Guid.Parse("18D2C60A-9718-4ED0-BA31-8E563A17FF4A"),
                CreatedDate = _minSqlDate,
                ModifiedDate = _minSqlDate,
                Systematic = SystematicType.UesET,
                Eps = new[]
                {
                    _EntityEp
                }
            };
            _UesBasePoint = new UesBasePoint
            {
                Id = Guid.NewGuid(),
                BasePointId = BasePointId,
                DealCode = AgencyCode,
                ValidFrom = _targetSqlDate,
                ValidTo = _targetSqlDate,
                CreatedDate = _minSqlDate,
                ModifiedDate = _minSqlDate,
                TownCode = "MOW",
                Address = string.Empty,
                SaleType = 0,
                Organization = new Organization
                {
                    Code = AgencyCode,
                    Role = "~",
                    CreatedDate = _minSqlDate,
                    ModifiedDate = _minSqlDate
                },
                EntityBasePoints = new[]
                {
                    _UesEtBasePoint
                }
            };
            _firstEntity = new Entity
            {
                EntityGuid = Guid.NewGuid(),
                Number = FirstEntityNumber,
                EntityExpirations = new[]
                {
                    new EntityExpiration
                    {
                        EntityExpirationGuid = Guid.NewGuid(),
                        BasePointGuid = _UesEtBasePoint.BasePointGuid,
                        IsActive = true,
                        ValidFrom = _targetSqlDate,
                        ValidTo = _targetSqlDate,
                        ModifiedDate = _minSqlDate
                    }
                }
            };
            _secondEntity = new Entity
            {
                EntityGuid = Guid.NewGuid(),
                Number = SecondEntityNumber,
                EntityExpirations = new[]
                {
                    new EntityExpiration
                    {
                        EntityExpirationGuid = Guid.NewGuid(),
                        BasePointGuid = _UesEtBasePoint.BasePointGuid,
                        IsActive = true,
                        ValidFrom = _targetSqlDate,
                        ValidTo = _targetSqlDate,
                        ModifiedDate = _minSqlDate
                    }
                }
            };
            _notActiveEntity = new Entity
            {
                EntityGuid = Guid.NewGuid(),
                Number = NonActiveEntityNumber,
                EntityExpirations = new[]
                {
                    new EntityExpiration
                    {
                        EntityExpirationGuid = Guid.NewGuid(),
                        BasePointGuid = _UesEtBasePoint.BasePointGuid,
                        IsActive = false,
                        ValidFrom = _targetSqlDate,
                        ValidTo = _targetSqlDate,
                        ModifiedDate = _minSqlDate
                    }
                }
            };
            _expiredEntity = new Entity
            {
                EntityGuid = Guid.NewGuid(),
                Number = NonActiveEntityNumber,
                EntityExpirations = new[]
                {
                    new EntityExpiration
                    {
                        EntityExpirationGuid = Guid.NewGuid(),
                        BasePointGuid = _UesEtBasePoint.BasePointGuid,
                        IsActive = true,
                        ValidFrom = _minSqlDate,
                        ValidTo = _minSqlDate,
                        ModifiedDate = _minSqlDate
                    }
                }
            };
            _futureEntity = new Entity
            {
                EntityGuid = Guid.NewGuid(),
                Number = NonActiveEntityNumber,
                EntityExpirations = new[]
                {
                    new EntityExpiration
                    {
                        EntityExpirationGuid = Guid.NewGuid(),
                        BasePointGuid = _UesEtBasePoint.BasePointGuid,
                        IsActive = false,
                        ValidFrom = _targetSqlDate.AddDays(1),
                        ValidTo = _targetSqlDate.AddDays(1),
                        ModifiedDate = _minSqlDate
                    }
                }
            };
            _newEntityEntity = new EntityEntity(new EntityEntityDataAccessModel
            {
                Number = NewEntityEntityNumber,
                SystematicType = EntitySystematicType.ElioET,
                ValidFrom = DateTime.Now,
                ValidTo = DateTime.Now.AddDays(1),
                Places = new List<EntityPlaceDataAccessModel>
                {
                    {
                        new EntityPlaceDataAccessModel
                        {
                            Pcc = "~~~~1",
                            AuthorizationDate = DateTime.Now
                        }
                    }
                }
            });
            _newSabreEtEntity = new EntityEntity(new EntityEntityDataAccessModel
            {
                Number = NewEntityEntityNumber,
                SystematicType = EntitySystematicType.SabreET,
                ValidFrom = DateTime.Now,
                ValidTo = DateTime.Now.AddDays(1),
                Places = new List<EntityPlaceDataAccessModel>
                {
                    {
                        new EntityPlaceDataAccessModel
                        {
                            Pcc = "AZ12",
                            AuthorizationDate = DateTime.Now,
                        }
                    }
                }
            });
            _newSrnEntity = new EntityEntity(new EntityEntityDataAccessModel
            {
                Number = NewEntityEntityNumber,
                SystematicType = EntitySystematicType.Srn,
                ValidFrom = DateTime.Now,
                ValidTo = DateTime.Now.AddDays(1),           
                SrnEntity = new SrnEntityDataAccessModel
                {
                    Agn = "12Ааа",
                    Grp = "4211111111",
                    SrnPools = new List<SrnPoolDataAccessModel>
                    {
                        new SrnPoolDataAccessModel
                        {
                            PoolNumber = "Аааа12",
                            AuthorizationDate = DateTime.Now
                        }
                    }
                }
            });


            CreateEntityForTestInDatabase();
        }
        public void Dispose()
        {
            _scope.Dispose();
        }

        private void CreateEntityForTestInDatabase()
        {
            // Если вдруг при предыдущих запусках тестов сущность не удалилась.
            // RemoveEntityForTestFromDatabase();
            using (var context = _container.Resolve<EntityContext>())
            {
                context.UesBasePoint.Add(_UesBasePoint);
                context.Entity.AddRange(new[] { _firstEntity, _secondEntity, _notActiveEntity, _expiredEntity, _futureEntity });
                context.SaveChanges();
            }
        }

        [Fact]
        public async Task GetByIdWithEntityActiveOnDateShouldReturnNullIfEntitySalesDoesNotExist()
        {
            var EntitySales = await GetRepository().GetByIdWithEntityActiveOnDate(Guid.NewGuid(), DateTime.MinValue);
            Assert.Null(EntitySales);
        }
        [Fact]
        public async Task Repository_Should_Return_Exactly_Matched_by_id_EntitySale_By_Id()
        {
            var EntitySales = await GetRepository().GetById(_UesBasePoint.Id);
            Assert.NotNull(EntitySales);
            Assert.Equal(EntitySales.Id, _UesBasePoint.Id);
        }
       //[Fact]
       //public async Task New_Entity_Entity_Should_Be_Able_To_Correctly_Created_And_Attached_To_EntitySale()
       //{
       //
       //    var repo = GetRepository();
       //
       //    var EntitySales = await repo.GetByIdWithEntityActiveOnDate(_UesBasePoint.Id, DateTime.Now); ;
       //
       //    var EntityEntity = EntitySales.ActiveEntity?.SelectMany(v => v.EntityPlaces) ?? Array.Empty<EntityPlace>();
       //
       //    Assert.Empty(EntityEntity);
       //
       //    EntitySales.AttachEntityEntity(_newEntityEntity);
       //
       //    await repo.Create(EntitySales, PredefinedUsers.MockUser, DateTime.Now);
       //
       //    EntitySales = await repo.GetByIdWithEntityActiveOnDate(_UesBasePoint.Id, DateTime.Now);
       //
       //    EntityEntity = EntitySales.ActiveEntity.SelectMany(v => v.EntityPlaces);
       //
       //    Assert.Single(EntityEntity);
       //
       //    Assert.Equal(EntityEntity.First().Pcc, _newEntityEntity.EntityPlaces.First().Pcc);
       //
       //}
       //[Fact]
       //public async Task New_UesET_Entity_Should_Be_Able_To_Correctly_Created_And_Attached_To_EntitySale()
       //{
       //    var repo = GetRepository();
       //
       //    var EntitySales = await repo.GetByIdWithEntityActiveOnDate(_UesBasePoint.Id, DateTime.Now); ;
       //
       //    var UesEtEntity = EntitySales.ActiveEntity?.SelectMany(v => v.EntityPlaces) ?? Array.Empty<EntityPlace>();
       //
       //    Assert.Empty(UesEtEntity);
       //
       //    EntitySales.AttachEntityEntity(_newSabreEtEntity);
       //
       //    await repo.Create(EntitySales, PredefinedUsers.MockUser, DateTime.Now);
       //
       //    EntitySales = await repo.GetByIdWithEntityActiveOnDate(_UesBasePoint.Id, DateTime.Now);
       //
       //    UesEtEntity = EntitySales.ActiveEntity.SelectMany(v => v.EntityPlaces);
       //
       //    Assert.Single(UesEtEntity);
       //
       //    Assert.Equal(UesEtEntity.First().Pcc, _newSabreEtEntity.EntityPlaces.First().Pcc);
       //}
       //[Fact]
       //public async Task New_Srn_Entity_Should_Be_Able_To_Correctly_Created_And_Attached_To_EntitySale()
       //{
       //
       //    var repo = GetRepository();
       //
       //    var EntitySales = await repo.GetByIdWithEntityActiveOnDate(_UesBasePoint.Id, DateTime.Now); ;
       //
       //    var SrnEntity = EntitySales.ActiveEntity?.Select(pos => pos.SrnEntity) ?? Array.Empty<SrnEntity>();
       //
       //    Assert.Empty(SrnEntity);
       //
       //    EntitySales.AttachEntityEntity(_newSrnEntity);
       //
       //    await repo.Create(EntitySales, PredefinedUsers.MockUser, DateTime.Now);
       //
       //    EntitySales = await repo.GetByIdWithEntityActiveOnDate(_UesBasePoint.Id, DateTime.Now);
       //
       //    SrnEntity = EntitySales.ActiveEntity.Select(v => v.SrnEntity);
       //
       //    Assert.Single(SrnEntity);
       //
       //    AssertionExtensions.CompareProperties(SrnEntity.First(), _newSrnEntity.SrnEntity);
       //
       //    AssertionExtensions.CompareProperties(SrnEntity.First().Pools.First(), _newSrnEntity.SrnEntity.Pools.First());
       //
       //}
        //[Fact]
        //public async Task New_Srn_Pool_Should_Be_Able_To_Attached_To_Srn_Entity()
        //{
        //    var repo = GetRepository();
        //
        //    var EntitySales = await repo.GetByIdWithEntityActiveOnDate(_UesBasePoint.Id, DateTime.Now); ;
        //
        //    var SrnEntity = EntitySales.ActiveEntity?.Select(pos => pos.SrnEntity) ?? Array.Empty<SrnEntity>();
        //
        //    EntitySales.AttachEntityEntity(_newSrnEntity);
        //
        //    EntitySales = await repo.GetByIdWithEntityActiveOnDate(_UesBasePoint.Id, DateTime.Now); ;
        //    
        //    await repo.GetByEntityId(cmd.EntityEntitySaleId); 
        //
        //}

        [Fact]
        public async Task EntitySalesShouldNotIncludeDeactivatedEntity()
        {
            var EntitySales = await GetRepository().GetByIdWithEntityActiveOnDate(_UesBasePoint.Id, _targetSqlDate);
            var Entity = EntitySales.ActiveEntity.FirstOrDefault(x => x.Number == _notActiveEntity.Number);
            Assert.Null(Entity);
        }

        [Fact]
        public async Task EntitySalesShouldNotIncludeExpiredEntity()
        {
            var EntitySales = await GetRepository().GetByIdWithEntityActiveOnDate(_UesBasePoint.Id, _targetSqlDate);
            var Entity = EntitySales.ActiveEntity.FirstOrDefault(x => x.Number == _expiredEntity.Number);
            Assert.Null(Entity);
        }

        [Fact]
        public async Task EntitySalesShouldNotIncludeFutureEntity()
        {
            var EntitySales = await GetRepository().GetByIdWithEntityActiveOnDate(_UesBasePoint.Id, _targetSqlDate);
            var Entity = EntitySales.ActiveEntity.FirstOrDefault(x => x.Number == _futureEntity.Number);
            Assert.Null(Entity);
        }

        [Fact]
        public async Task EntitySalesShouldHaveCorrectMapping()
        {
            var EntitySales = await GetRepository().GetByIdWithEntityActiveOnDate(_UesBasePoint.Id, _targetSqlDate);
            Assert.NotNull(EntitySales);
            Assert.Equal(_UesBasePoint.Id, EntitySales.Id);
            Assert.Equal(BasePointId, EntitySales.BasePointId);
            Assert.Equal(_targetSqlDate, EntitySales.DateForActiveEntity);
            Assert.Equal(_UesBasePoint.DealCode, EntitySales.Deal.Code);

            var firstEntity = EntitySales.ActiveEntity.First(v => v.Number == FirstEntityNumber);
            AssertionExtensions.EntityAreEqual(_firstEntity, firstEntity, _UesEtBasePoint, _EntityEp);

            var secondEntity = EntitySales.ActiveEntity.First(v => v.Number == SecondEntityNumber);
            AssertionExtensions.EntityAreEqual(_secondEntity, secondEntity, _UesEtBasePoint, _EntityEp);
        }

        [Fact]
        public async Task UpdateShouldDoNothingIfNoChanges()
        {
            var EntitySales = await GetRepository().GetByIdWithEntityActiveOnDate(_UesBasePoint.Id, _targetSqlDate);
            await GetRepository().UpdateEntitySale(EntitySales, PredefinedUsers.MockUser, _minSqlDate);

            await AssureThatEntitiesStillTheSame();
        }

        [Fact]
        public async Task UpdateShouldSaveCreatedPlace()
        {
            var validFrom = DateTime.Now;
            var EntitySales = await GetRepository().GetByIdWithEntityActiveOnDate(_UesBasePoint.Id, _targetSqlDate);
            EntitySales.CreateNewPlaceForEntity(_firstEntity.EntityExpirations.First().EntityExpirationGuid, new EntityPlace(NewPlaceId, validFrom));

            await GetRepository().UpdateEntitySale(EntitySales, PredefinedUsers.MockUser, _targetSqlDate);

            await AssureThatNewPlaceCreated(EntitySales, validFrom, _targetSqlDate);
        }

        private EntitySalesRepository GetRepository()
        {
            return _container.Resolve<EntitySalesRepository>();
        }

        private async Task AssureThatEntitiesStillTheSame()
        {
            using (var context = _container.Resolve<EntityContext>())
            {
                var UesBasePoint = await context.UesBasePoint.FirstOrDefaultAsync(x => x.Id == _UesBasePoint.Id);
                AssertionExtensions.CompareSimpleProperties(_UesBasePoint, UesBasePoint);

                var BasePoint = await context.BasePoint.FirstOrDefaultAsync(x => x.BasePointGuid == _UesEtBasePoint.BasePointGuid);
                AssertionExtensions.CompareSimpleProperties(_UesEtBasePoint, BasePoint);

                var Ep = await context.Ep.FirstOrDefaultAsync(x => x.EpGuid == _EntityEp.EpGuid);
                AssertionExtensions.CompareSimpleProperties(_EntityEp, Ep);

                var Entity = new[] { _firstEntity, _secondEntity, _notActiveEntity, _expiredEntity, _futureEntity };
                foreach (var Entity in Entity)
                {
                    var dbEntity = await context.Entity.FirstOrDefaultAsync(x => x.EntityGuid == Entity.EntityGuid);
                    AssertionExtensions.CompareSimpleProperties(Entity, dbEntity);
                }

                var EntityExpirations = Entity.SelectMany(x => x.EntityExpirations).ToList();
                foreach (var EntityExpiration in EntityExpirations)
                {
                    var dbEntityExpiration = await context.EntityExpiration.FirstOrDefaultAsync(x => x.EntityExpirationGuid == EntityExpiration.EntityExpirationGuid);
                    AssertionExtensions.CompareSimpleProperties(EntityExpiration, dbEntityExpiration);
                }
            }
        }

        private async Task AssureThatNewPlaceCreated(EntitySales EntitySales, DateTime validFrom, DateTime updatedAt)
        {
            using (var context = _container.Resolve<EntityContext>())
            {
                var Ep = await context.Ep.FirstOrDefaultAsync(x => x.PlaceId == NewPlaceId);

                Assert.NotEqual(Guid.Empty, Ep.EpGuid);
                Assert.Equal(EntitySales.Deal.Code, Ep.ASIACode);
                Assert.Equal(_UesEtBasePoint.BasePointGuid, Ep.BasePointGuid);
                Assert.Equal(NewPlaceId, Ep.PlaceId);
                Assert.Equal(EpSystems.Ues, Ep.EpSystem);
                Assert.Equal(validFrom.Date, Ep.AutorizeDate);
                Assert.Equal("test", Ep.AK);
                Assert.False(Ep.EMD);
                Assert.Equal(string.Empty, Ep.Comment);
                Assert.Equal(string.Empty, Ep.OFFC);
                Assert.True(Ep.Status);
                Assert.Equal(PredefinedUsers.MockUser.Identity.Name, Ep.CreatedBy);
                Assert.Equal(PredefinedUsers.MockUser.Identity.Name, Ep.ModifiedBy);
                Assert.Equal(updatedAt, Ep.ModifiedDate);
                Assert.Equal(updatedAt, Ep.CreatedDate);
            }
        }
    }

    internal static class AssertionExtensions
    {
        public static void EntityAreEqual(Entity Entity, EntityEntity EntityEntity, BasePoint BasePoint, Ep Ep)
        {
            Assert.Equal(Entity.EntityExpirations.First().EntityExpirationGuid, EntityEntity.Id);
            Assert.Equal(Entity.Number, EntityEntity.Number);
            Assert.Equal(Entity.EntityExpirations.First().ValidFrom, EntityEntity.ValidFrom);
            Assert.Equal(Entity.EntityExpirations.First().ValidTo, EntityEntity.ValidTo);
            Assert.Equal(BasePoint.Systematic.ToEntitySystematic(), EntityEntity.Systematic);

            var Place = EntityEntity.EntityPlaces.First();
            EtDirectPlaceAreEqual(Place, Ep);
        }

        public static void EtDirectPlaceAreEqual(EntityPlace Place, Ep Ep)
        {
            Assert.Equal(Ep.EpGuid, Place.Id);
            Assert.Equal(Ep.PlaceId, Place.Pcc);
            Assert.Equal(Ep.ValidFrom, Place.AuthorizationDate);
        }

        public static void CompareSimpleProperties<T>(T object1, T object2)
        {
            var propertyTypesForCompare = new[]
            {
                typeof(string),
                typeof(Guid), typeof(Guid?),
                typeof(byte),
                typeof(bool), typeof(bool?),
                typeof(DateTime), typeof(DateTime?)
            };

            var properties = typeof(T).GetProperties().Where(x => propertyTypesForCompare.Contains(x.PropertyType)).ToList();
            foreach (var property in properties)
            {
                Assert.Equal(property.GetValue(object1), property.GetValue(object2));
            }
        }
        public static void CompareProperties<T>(T object1, T object2)
        {
            var propertyTypesForCompare = new[]
            {
                typeof(string),
                typeof(byte),
                typeof(bool), typeof(bool?),
                typeof(DateTime), typeof(DateTime?)
            };

            var properties = typeof(T).GetProperties().Where(x => propertyTypesForCompare.Contains(x.PropertyType)).ToList();
            foreach (var property in properties)
            {
                Assert.Equal(property.GetValue(object1), property.GetValue(object2));
            }
        }
    }
}
