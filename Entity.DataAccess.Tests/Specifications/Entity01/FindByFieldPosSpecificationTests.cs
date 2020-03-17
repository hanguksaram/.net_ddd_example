using Entity.DataAccess.Specification.Entity01;
using Entity.Tests.Common;
using System;
using Xunit;

namespace Entity.DataAccess.Tests.Specifications.Entity01
{
    public class FindByEntityPosSpecificationTests
    {
        [Theory]
        [InlineData("42122223")]
        [InlineData("421")]
        [InlineData("22223")]
        [InlineData("2222")]
        public void ShouldSatisfy_IfBasePointHasActiveEntity_AndNumberContainsTerm(string term)
        {
            var now = DateTime.Now.Date;
            DateTime from = now, to = now;

            var value = Default.UesBasePoint
                .WithEntityBasePoint(s => s.WithSingleReferenceEntity(from, to, v => v.SetProperty(x => x.Number, "42122223")))
                .Value;

            var spec = new FindByEntityEntityPosSpecification(term, now);

            Assert.True(spec.IsSatisfiedBy(value));
        }

        [Theory]
        [InlineData("42122223")]
        [InlineData("421")]
        [InlineData("22223")]
        [InlineData("2222")]
        public void ShouldSatisfy_IfBasePointHasActiveEntity_AndNumberContainsTerm_AndNumberHasFormatting(string term)
        {
            var now = DateTime.Now.Date;
            DateTime from = now, to = now;

            var value = Default.UesBasePoint
                .WithEntityBasePoint(s => s.WithSingleReferenceEntity(from, to, v => v.SetProperty(x => x.Number, "421-2222-3")))
                    .Value;

            var spec = new FindByEntityEntityPosSpecification(term, now);

            Assert.True(spec.IsSatisfiedBy(value));
        }

        [Theory]
        [InlineData("421-2222-3")]
        [InlineData("421-")]
        [InlineData("2222-3")]
        [InlineData("-2222-")]
        public void ShouldSatisfy_IfBasePointHasActiveEntity_AndNumberContainsTerm_AndTermHasFormatting(string term)
        {
            var now = DateTime.Now.Date;
            DateTime from = now, to = now;

            var value = Default.UesBasePoint
                .WithEntityBasePoint(s => s.WithSingleReferenceEntity(from, to, v => v.SetProperty(x => x.Number, "42122223")))
                    .Value;

            var spec = new FindByEntityEntityPosSpecification(term, now);

            Assert.True(spec.IsSatisfiedBy(value));
        }


        [Theory]
        [InlineData("421-2222-3")]
        [InlineData("421-")]
        [InlineData("2222-3")]
        [InlineData("-2222-")]
        public void ShouldSatisfy_IfBasePointHasActiveEntity_AndNumberContainsTerm_AndTermAndNumberHasFormatting(string term)
        {
            var now = DateTime.Now.Date;
            DateTime from = now, to = now;

            var value = Default.UesBasePoint
                .WithEntityBasePoint(s => s.WithSingleReferenceEntity(from, to, v => v.SetProperty(x => x.Number, "421-2222-3")))
                    .Value;

            var spec = new FindByEntityEntityPosSpecification(term, now);

            Assert.True(spec.IsSatisfiedBy(value));
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("    ")]
        [InlineData("")]
        [InlineData(null)]
        public void ShouldThrow_IfTermIsWhitespaceOrEmptyOrNull(string term)
        {
            Assert.Throws<ArgumentNullException>(() => new FindByEntityEntityPosSpecification(term, default));
        }

    }
}
