using Autofac;
using Entity.DataAccess.Repository;

namespace Entity.DataAccess.Tests
{
    internal static class CompositionRoot
    {
        public static IContainer GetContainer()
        {
            var builder = new ContainerBuilder();
         
            builder.RegisterType<EntitySalesRepository>().AsSelf();
            builder.RegisterType<EntityContext>().AsSelf();
            return builder.Build();
        }
    }
}
