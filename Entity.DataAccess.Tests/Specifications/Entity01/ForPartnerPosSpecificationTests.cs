using Entity.DataAccess.Specification.Entity01;
using Entity.Tests.Common;
using System;
using Xunit;

namespace Entity.DataAccess.Tests.Specifications.Entity01
{
    public class ForDealPosSpecificationTests
    {
        [Fact]
        public void ShouldThrow_IfCodeIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new ForDealPosSpecification(null));
        }


        [Fact]
        public void ShouldSatisfy_IfCodeIsEquals()
        {
            var value = Default.UesBasePoint
                .SetProperty(x => x.DealCode, "test")
                    .Value;

            var spec = new ForDealPosSpecification("test");

            Assert.True(spec.IsSatisfiedBy(value));
        }

        [Fact]
        public void ShouldNotSatisfy_IfCodeIsNotEquals()
        {
            var value = Default.UesBasePoint
                .SetProperty(x => x.DealCode, "test")
                    .Value;

            var spec = new ForDealPosSpecification("tes");

            Assert.False(spec.IsSatisfiedBy(value));
        }
    }
}
