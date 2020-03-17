using Entity.DataAccess.Specification.Entity01;
using Entity.Tests.Common;
using System;
using Xunit;

namespace Entity.DataAccess.Tests.Specifications.Entity01
{
    public class FindByEntitySalePosSpecificationTests
    {
        [Theory]
        [InlineData("PS")]
        [InlineData("PS1")]
        [InlineData("PS11")]
        [InlineData("PS111")]
        [InlineData("PS1111")]
        [InlineData("PS11111")]
        public void ShouldSatisfy_IfBasePointIdStartsWithTerm(string term)
        {
            var value = Default.UesBasePoint
                    .SetProperty(x => x.BasePointId, "PS11111")
                    .Value;

            var spec = new FindByEntitySalePosSpecification(term);

            Assert.True(spec.IsSatisfiedBy(value));
        }
        [Theory]
        [InlineData("1")]
        [InlineData("11")]
        [InlineData("111")]
        [InlineData("11111")]
        public void ShouldNotSatisfy_IfBasePointIdContansButNotStartsWithTerm(string term)
        {
            var value = Default.UesBasePoint
                    .SetProperty(x => x.BasePointId, "PS11111")
                    .Value;

            var spec = new FindByEntitySalePosSpecification(term);

            Assert.False(spec.IsSatisfiedBy(value));
        }

        [Theory]
        [InlineData(" PS11111")]
        [InlineData("PS11111 ")]
        [InlineData(" PS11111 ")]
        public void ShouldSatisfy_IfTermCanBeTrimmed(string term)
        {
            var value = Default.UesBasePoint
                    .SetProperty(x => x.BasePointId, "PS11111")
                    .Value;

            var spec = new FindByEntitySalePosSpecification(term);

            Assert.True(spec.IsSatisfiedBy(value));
        }


        [Theory]
        [InlineData(" ")]
        [InlineData("    ")]
        [InlineData("")]
        [InlineData(null)]
        public void ShouldThrow_IfTermIsWhitespaceOrEmptyOrNull(string term)
        {
            Assert.Throws<ArgumentNullException>(() => new FindByEntitySalePosSpecification(term));
        }



        [Theory]
        [InlineData("Таким образом")]
        [InlineData("интересный эксперимент")]
        [InlineData("направлений развития.")]
        [InlineData(". Товарищи!")]
        [InlineData("задач")]
        public void ShouldSatisfy_IfTermContainsPartOfAddress(string term)
        {
            var value = Default.UesBasePoint
                    .SetProperty(x => x.Address, "Таким образом постоянное информационно-пропагандистское обеспечение нашей деятельности представляет собой интересный эксперимент проверки существенных финансовых и административных условий. Товарищи! консультация с широким активом в значительной степени обуславливает создание форм развития. С другой стороны начало повседневной работы по формированию позиции представляет собой интересный эксперимент проверки позиций, занимаемых участниками в отношении поставленных задач. Таким образом сложившаяся структура организации требуют определения и уточнения существенных финансовых и административных условий. Равным образом дальнейшее развитие различных форм деятельности влечет за собой процесс внедрения и модернизации дальнейших направлений развития.")
                    .Value;

            var spec = new FindByEntitySalePosSpecification(term);

            Assert.True(spec.IsSatisfiedBy(value));
        }


        [Theory]
        [InlineData("Lorem ipsum")]
        [InlineData("adipiscing elit")]
        [InlineData("est laborum.")]
        [InlineData(". Duis")]
        [InlineData("nisi")]
        public void ShouldSatisfy_IfTermContainsPartOfAddressLatin(string term)
        {
            var value = Default.UesBasePoint
                    .SetProperty(x => x.AddressLatin, "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.")
                    .Value;

            var spec = new FindByEntitySalePosSpecification(term);

            Assert.True(spec.IsSatisfiedBy(value));
        }

        [Theory]
        [InlineData("красная рыба")]
        [InlineData("брощ")]
        [InlineData("11111")]
        [InlineData("1")]
        [InlineData("and")]
        public void ShouldNotSatisfy_IfTermDoesntContainsPartOfAddressOrId(string term)
        {
            var value = Default.UesBasePoint
                    .SetProperty(x => x.BasePointId, "PS11111")
                    .SetProperty(x => x.Address, "Таким образом постоянное информационно-пропагандистское обеспечение нашей деятельности представляет собой интересный эксперимент проверки существенных финансовых и административных условий. Товарищи! консультация с широким активом в значительной степени обуславливает создание форм развития. С другой стороны начало повседневной работы по формированию позиции представляет собой интересный эксперимент проверки позиций, занимаемых участниками в отношении поставленных задач. Таким образом сложившаяся структура организации требуют определения и уточнения существенных финансовых и административных условий. Равным образом дальнейшее развитие различных форм деятельности влечет за собой процесс внедрения и модернизации дальнейших направлений развития.")
                    .SetProperty(x => x.AddressLatin, "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.")
                    .Value;

            var spec = new FindByEntitySalePosSpecification(term);

            Assert.False(spec.IsSatisfiedBy(value));
        }
    }
}
