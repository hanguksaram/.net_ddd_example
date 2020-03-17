using Entity.DataAccess.Specification.Entity01;
using Entity.Tests.Common;
using Xunit;

namespace Entity.DataAccess.Tests.Specifications.Entity01
{
    public class ExcludeRgPosSpecificationTests
    {
        [Fact]
        public void PosOfDeal1488ShouldBeExcluded()
        {
            var value = Default.UesBasePoint
                    .SetProperty(x => x.DealCode, "1488")
                    .Value;

            var spec = new ExcludeRgPosSpecification();

            Assert.False(spec.IsSatisfiedBy(value));
        }


        [Fact]
        public void PosOfDeal01ShouldBeIncluded()
        {
            var value = Default.UesBasePoint
                    .SetProperty(x => x.DealCode, "01")
                    .Value;

            var spec = new ExcludeRgPosSpecification();

            Assert.True(spec.IsSatisfiedBy(value));
        }        
    }
}
