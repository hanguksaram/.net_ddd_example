using FluentValidation;

namespace Entity.ApplicationServices.Services
{
    public class SrnNumberEntity : AbstractEntity<EntityEntityDataTransferModel>
    {
        public SrnNumberEntity()
        {
#warning согласовать с Младой валидационное сообщение
            RuleFor(v => v.Number)
                .NotEmpty()
                .LastNumberRule();

        }
    }
}
