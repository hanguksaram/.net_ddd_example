using FluentValidation;

namespace Entity.ApplicationServices.Services
{
    public class UesEtNumberEntity : AbstractEntity<EntityEntityDataTransferModel>
    {
        public UesEtNumberEntity()
        {
            RuleFor(v => v.Number)
                .NotEmpty()
                .LastNumberRule();
        }    
    }
}
