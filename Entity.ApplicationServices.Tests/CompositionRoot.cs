using Autofac;
using FluentValidation;
using Entity.ApplicationServices.Queries.PartnerProfile;
using Entity.ApplicationServices.Repositories;
using Entity.ApplicationServices.Services;
using Entity.ApplicationServices.Tests.Fakes;
using Entity.CrossCutting;

namespace Entity.ApplicationServices.Tests
{
    internal class CompositionRoot
    {
        public static IContainer GetContainer()
        {
            var builder = new ContainerBuilder();
            var assembly = typeof(EntityEntityApplicationService).Assembly;

            builder.RegisterAssemblyTypes(assembly)
                .AsClosedTypesOf(typeof(AbstractEntity<>))
                .InstancePerLifetimeScope();
            builder.RegisterType<EntitySalesApplicationService>().SingleInstance();
            builder.RegisterType<EntityEntityApplicationServiceTests>().SingleInstance();
            builder.RegisterType<MockEntitySalesRepository>().As<IEntitySalesRepository>().SingleInstance();
            builder.RegisterType<MockEntityEntityRepository>().As<IEntityEntityRepository>().SingleInstance();
            builder.RegisterType<MockDatetimeProvider>().As<IDatetimeProvider>().SingleInstance();
            builder.RegisterType<GetSystematicForEntityMockQueryHandler>().As<IGetSystematicForEntityQueryHandler>().SingleInstance();
            builder.RegisterType<GetEntityAuthorizationsStatusMockQueryHandler>().As<IGetEntityAuthorizationsStatusQueryHandler>().SingleInstance();
            return builder.Build();
        }
    }
}
