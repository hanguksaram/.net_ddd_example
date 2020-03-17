using Autofac;
using Entity.CrossCutting.Exceptions;
using Entity.Domain;
using Entity.Domain.DataAccessModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Entity.ApplicationServices.Repositories;
using Entity.CrossCutting;
using Entity.ApplicationServices.Tests.Fakes;

namespace Entity.ApplicationServices.Tests
{
    public class EntitySalesApplicationServiceTests
    {
        private readonly IContainer _container;

        public EntitySalesApplicationServiceTests()
        {
            _container = CompositionRoot.GetContainer();
        }

        [Fact]
        public async Task AttachNewPlaceShouldThrowIfBasePointWasNotFound()
        {
            Func<Task> act = () => GetService().AttachNewPlace(Guid.NewGuid(), Guid.NewGuid(), string.Empty, PredefinedUsers.MockUser);
            await Assert.ThrowsAsync<NotFoundException>(act);
        }

        [Fact]
        public async Task AttachNewPlaceShouldThrowIfEntityWasNotFound()
        {
            var dataAccessModel = new EntitySalesDataAccessModel
            {
                Id = Guid.NewGuid(),
                ActiveEntity = Array.Empty<EntityEntityDataAccessModel>()
            };
            GetRepository().EntitySalesToReturn = new EntitySales(dataAccessModel);
            Func<Task> act = () => GetService().AttachNewPlace(dataAccessModel.Id, Guid.NewGuid(), "MOW1", PredefinedUsers.MockUser);
            await Assert.ThrowsAsync<DomainException>(act);
        }

        [Fact]
        public async Task ShouldAddNewPlace()
        {
            var targetEntityGuid = Guid.NewGuid();
            var dataAccessModel = new EntitySalesDataAccessModel
            {
                Id = Guid.NewGuid(),
                ActiveEntity = new[]
                {
                    new EntityEntityDataAccessModel
                    {
                        Id = targetEntityGuid,
                        Places = Array.Empty<EntityPlaceDataAccessModel>(),
                        SystematicType = EntitySystematicType.ElioET
                    },
                    new EntityEntityDataAccessModel
                    {
                        Id = Guid.NewGuid(),
                        Places = Array.Empty<EntityPlaceDataAccessModel>(),
                        SystematicType = EntitySystematicType.ElioET
                    }
                }
            };


            var EntitySales = new EntitySales(dataAccessModel);
            GetRepository().EntitySalesToReturn = EntitySales;

            var now = DateTime.Now;
            GetDatetimeProvider().LocalNow = now;

            await GetService().AttachNewPlace(dataAccessModel.Id, targetEntityGuid, "MOW1", PredefinedUsers.MockUser);
            var Place = EntitySales.ActiveEntity.First(v => v.Id == targetEntityGuid).EntityPlaces.FirstOrDefault(v => v.Pcc == "MOW1");
            Assert.NotNull(Place);
            Assert.Equal("MOW1", Place.Pcc);
            Assert.NotEqual(Guid.Empty, (Guid)Place.Id);
            Assert.Equal(now.Date, Place.AuthorizationDate);
        }

        [Fact]
        public async Task ShouldReturnNewPlace()
        {
            var targetEntityGuid = Guid.NewGuid();
            var dataAccessModel = new EntitySalesDataAccessModel
            {
                Id = Guid.NewGuid(),
                ActiveEntity = new[]
                {
                    new EntityEntityDataAccessModel
                    {
                        Id = targetEntityGuid,
                        Places = Array.Empty<EntityPlaceDataAccessModel>(),
                        SystematicType = EntitySystematicType.ElioET
                    },
                    new EntityEntityDataAccessModel
                    {
                        Id = Guid.NewGuid(),
                        Places = Array.Empty<EntityPlaceDataAccessModel>(),
                        SystematicType = EntitySystematicType.ElioET
                    }
                }
            };


            var EntitySales = new EntitySales(dataAccessModel);
            GetRepository().EntitySalesToReturn = EntitySales;

            var now = DateTime.Now;
            GetDatetimeProvider().LocalNow = now;

            var Place = await GetService().AttachNewPlace(dataAccessModel.Id, targetEntityGuid, "MOW1", PredefinedUsers.MockUser);
            Assert.NotNull(Place);
            Assert.Equal("MOW1", Place.Pcc);
            Assert.NotEqual(Guid.Empty, Place.Id);
            Assert.Equal(now.Date, Place.AuthorizationDate);
        }

        private EntitySalesApplicationService GetService()
        {
            return _container.Resolve<EntitySalesApplicationService>();
        }

        private MockEntitySalesRepository GetRepository()
        {
            return (MockEntitySalesRepository)_container.Resolve<IEntitySalesRepository>();
        }

        private MockDatetimeProvider GetDatetimeProvider()
        {
            return (MockDatetimeProvider)_container.Resolve<IDatetimeProvider>();
        }
    }
}
