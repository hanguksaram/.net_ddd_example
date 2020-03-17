using FluentValidation;
using Entity.ApplicationServices.Services;
using Xunit;
using static Entity.Tests.Common.Default;

namespace Entity.ApplicationServices.Tests
{
    public class EntityValdatorsValidationTests
    {
        private readonly UesEtNumberEntity _UesEtEntity = new UesEtNumberEntity();
        private readonly ElioNumberEntity _ElioEntity = new ElioNumberEntity();
        private readonly SrnNumberEntity _SrnEntity = new SrnNumberEntity();

        [Fact]
        public void ShouldSuccessfullyValidateSabreEtEntity()
        {
            var val = EntityTransferModel
                .SetProperty(v => v.Systematic, Domain.EntitySystematicType.SabreET)
                .Value;
            
            Assert.Null(Record.Exception(() => _UesEtEntity.ValidateAndThrow(val)));
        }
        [Fact]
        public void ShouldSuccessfullyValidateElioEntity()
        {
            var val = EntityTransferModel
                .SetProperty(v => v.Systematic, Domain.EntitySystematicType.ElioET)
                .Value;
            Assert.Null(Record.Exception(() => _ElioEntity.ValidateAndThrow(val)));
        }
        [Fact]
        public void ShouldSuccessfullyValidateSrnEntity()
        {
            var val = EntityTransferModel
                .SetProperty(v => v.Systematic, Domain.EntitySystematicType.Srn)
                .Value;
            Assert.Null(Record.Exception(() => _SrnEntity.ValidateAndThrow(val)));
        }
        [Fact]
        public void ShouldThrowExceptionWhenSabreEtEntityInvalid()
        {
            var val = EntityTransferModel
                .SetProperty(v => v.Systematic, Domain.EntitySystematicType.SabreET)
                .SetProperty(v => v.Number, Data.DefaultEntityNumber)
                .Value;

            Assert.Throws<ValidationException>(() => _UesEtEntity.ValidateAndThrow(val));
        }
        [Fact]
        public void ShouldThrowExceptionWhenElioEntityInvalid()
        {
            var val = EntityTransferModel
                .SetProperty(v => v.Systematic, Domain.EntitySystematicType.ElioET)
                .SetProperty(v => v.Number, Data.DefaultEntityNumber)
                .Value;

            Assert.Throws<ValidationException>(() =>  _ElioEntity.ValidateAndThrow(val));
        }
        [Fact]
        public void ShouldThrowExceptionWhenSrnEntityInvalid()
        {
            var val = EntityTransferModel
                .SetProperty(v => v.Systematic, Domain.EntitySystematicType.Srn)
                .SetProperty(v => v.Number, Data.DefaultEntityNumber)
                .Value;

            Assert.Throws<ValidationException>(() =>  _SrnEntity.ValidateAndThrow(val));
        }
    }
}
