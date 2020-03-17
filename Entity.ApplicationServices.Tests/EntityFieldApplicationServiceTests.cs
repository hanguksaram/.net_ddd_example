using Autofac;
using System.Threading.Tasks;
using Entity.ApplicationServices.Tests.Fakes;
using Entity.CrossCutting;
using Xunit;

namespace Entity.ApplicationServices.Tests
{
    public class EntityEntityApplicationServiceTests
    {
        private readonly IContainer _container;

        public EntityEntityApplicationServiceTests()
        {
            _container = CompositionRoot.GetContainer();
        }

        private EntityEntityApplicationServiceTests GetService()
        {
            return _container.Resolve<EntityEntityApplicationServiceTests>();
        }

        private MockDatetimeProvider GetDatetimeProvider()
        {
            return (MockDatetimeProvider)_container.Resolve<IDatetimeProvider>();
        }
    }
}
