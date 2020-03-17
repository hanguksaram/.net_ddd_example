using FluentValidation;

namespace Entity.ApplicationServices.Services
{
    public class ElioNumberEntity : AbstractEntity<EntityEntityDataTransferModel>
    {
        public ElioNumberEntity()
        {
            RuleFor(v => v.Number)
                .NotEmpty()
                .LastNumberRule();
                                
        }
    }
}
