using Entity.DataAccess.Specification.Entity01;
using Entity.Tests.Common;
using System;
using Xunit;

namespace Entity.DataAccess.Tests.Specifications.Entity01
{
    public class HasActiveEntityEntitySalePosSpecificationTests
    {
        [Fact]
        public void ShouldNotSatisfy_IfHasntEntityBasePoint()
        {
            var value = Default.UesBasePoint.Value;

            var spec = new HasActiveEntityEntitySalePosSpecification(default);

            Assert.False(spec.IsSatisfiedBy(value));
        }

        [Fact]
        public void ShouldNotSatisfy_IfHasntEntityEntity()
        {
            var value = Default.UesBasePoint
                               .WithEntityBasePoint()
                               .Value;


            var spec = new HasActiveEntityEntitySalePosSpecification(default);

            Assert.False(spec.IsSatisfiedBy(value));
        }

        [Fact]
        public void ShouldSatisfy_IfHasActiveEntityEntity()
        {
            var date = DateTime.Now.Date;

            var value = Default.UesBasePoint
                               .WithEntityBasePoint(sp => sp.WithSingleReferenceEntity(date, date))
                               .Value;


            var spec = new HasActiveEntityEntitySalePosSpecification(date);

            Assert.True(spec.IsSatisfiedBy(value));
        }

        [Fact]
        public void ShouldNotSatisfy_IfHasInactiveEntityEntity()
        {
            var date = DateTime.Now.Date;
            var yest = date.AddDays(-1);

            var value = Default.UesBasePoint
                               .WithEntityBasePoint(sp => sp.WithSingleReferenceEntity(yest, yest))
                               .Value;


            var spec = new HasActiveEntityEntitySalePosSpecification(date);

            Assert.False(spec.IsSatisfiedBy(value));
        }

        [Fact]
        public void ShouldNotSatisfy_IfHasActiveEntityEntity_ButPs02IsStarted()
        {
            var date = DateTime.Now.Date;

            var value = Default.UesBasePoint

                               .WithEntityBasePoint(sp => sp.WithSingleReferenceEntity(date, date)
                                                         .SetProperty(x => x.RevokedByCorrelationId, Guid.NewGuid()))
                               .Value;


            var spec = new HasActiveEntityEntitySalePosSpecification(date);

            Assert.False(spec.IsSatisfiedBy(value));
        }
    }
}
