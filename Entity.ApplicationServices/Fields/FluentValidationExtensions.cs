using FluentValidation;
using Entity.Domain;
using System.Linq;
using System.Text.RegularExpressions;

namespace Entity.ApplicationServices.Services
{
    public static class FluentValidationExtensions
    {
        public static IRuleBuilderOptions<EntityEntityDataTransferModel, string> LastNumberRule(this IRuleBuilder<EntityEntityDataTransferModel, string> Entity)
            => Entity.Must(n => EntityNumber.TryParse(n, out var v) && v.IsControlNumberValid());

        public static IRuleBuilderOptions<CreateEntityEntityCommandDto, string> LastNumberRule(this IRuleBuilder<CreateEntityEntityCommandDto, string> Entity)
            => Entity.Must(n => EntityNumber.TryParse(n, out var v) && v.IsControlNumberValid());
        
        public static IRuleBuilderOptions<CreateEntityEntityCommandDto, SrnEntityDto> SrnPoolValidationRule(this IRuleBuilder<CreateEntityEntityCommandDto, SrnEntityDto> Pool)
            => Pool.Must(n => Regex.IsMatch(n.Agn, @"^\d{2}[А-я]{3}$", RegexOptions.Compiled))
                       .Must(n => Regex.IsMatch(n.Grp, @"^421\d{7}$", RegexOptions.Compiled))
                       .Must(n => n.SrnPools.Aggregate(true, (acc, t) => acc &= Regex.IsMatch(t.PoolNumber, @"^[А-я]{4}\d{2}$", RegexOptions.Compiled)));

        public static IRuleBuilderOptions<CreateEntityEntityCommandDto, EntityPlaceDto> EntityPlaceValidationRule(this IRuleBuilder<CreateEntityEntityCommandDto, EntityPlaceDto> Entity)
            => Entity.Must(n => Regex.IsMatch(n.Pcc, @"^[A-Z0-9]{4}$", RegexOptions.Compiled) || Regex.IsMatch(n.Pcc, @"^[A-Z0-9]{9}$", RegexOptions.Compiled));

        public static IRuleBuilderOptions<UpdateEntityEntityCommandDto, EntityPlaceDto> EntityPlaceValidationRule(this IRuleBuilder<UpdateEntityEntityCommandDto, EntityPlaceDto> Entity)
                    => Entity.Must(n => Regex.IsMatch(n.Pcc, @"^[A-Z0-9]{4}$", RegexOptions.Compiled) || Regex.IsMatch(n.Pcc, @"^[A-Z0-9]{9}$", RegexOptions.Compiled));

        public static IRuleBuilderOptions<UpdateEntityEntityCommandDto, SrnEntityDto> SrnPoolValidationRule(this IRuleBuilder<UpdateEntityEntityCommandDto, SrnEntityDto> Pool)
            => Pool.Must(n => Regex.IsMatch(n.Agn, @"^\d{2}[А-я]{3}$", RegexOptions.Compiled))
                       .Must(n => Regex.IsMatch(n.Grp, @"^421\d{7}$", RegexOptions.Compiled))
                       .Must(n => n.SrnPools.Aggregate(true, (acc, t) => acc &= Regex.IsMatch(t.PoolNumber, @"^[А-я]{4}\d{2}$", RegexOptions.Compiled)));
    }

}
