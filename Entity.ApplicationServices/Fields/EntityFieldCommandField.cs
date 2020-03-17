using FluentValidation;
using Entity.ApplicationServices.Queries.PartnerProfile;
using Entity.ApplicationServices.Queries.PartnerProfile.Models;
using Entity.ApplicationServices.Services;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Entity.ApplicationServices.Entity
{
    public class CreateEntityEntityCommandEntity : AbstractEntity<CreateEntityEntityCommandDto>
    {
        public CreateEntityEntityCommandEntity()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(v => v.UesEntitySaleId)
                .NotNull()
                .Must(v => v != Guid.Empty);

            RuleFor(v => v.EntityNumber)
                .NotNull()
                .NotEmpty();

            RuleFor(v => v.EntityNumber)
                .LastNumberRule()
                    .When(s => s.Systematic != SystematicTypesDto.SabreET);

            RuleFor(v => v.Systematic)
                .Must(s => s != SystematicTypesDto.Default && s != SystematicTypesDto.Uestest)
                    .When(s => s.Systematic.HasValue);

            RuleForEach(v => v.EntityPlaces)
                .EntityPlaceValidationRule()
                    .When(v => v.EntityPlaces?.Any() == true);

            //RuleFor(v => v.SiteAudience)
            //    .Must(s => s != SiteAudienceTypesDto.Default)
            //        .When(s => s.Location == LocationTypesDto.Online);


            //todo agn grp taps???
        }
    }
    public class UpdateEntityEntityCommandEntity : AbstractEntity<UpdateEntityEntityCommandDto>
    {
        private readonly IGetSystematicForEntityQueryHandler _SystematicQualifier;
        private readonly IGetEntityAuthorizationsStatusQueryHandler _authorizationStatusQualifier;

        public UpdateEntityEntityCommandEntity(
            IGetSystematicForEntityQueryHandler SystematicQualifier,
            IGetEntityAuthorizationsStatusQueryHandler authorizationStatusQualifier
            )
        {
            _SystematicQualifier = SystematicQualifier;
            _authorizationStatusQualifier = authorizationStatusQualifier;

            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(v => v.ValidityId)
                .NotNull()
                .Must(v => v != Guid.Empty);

            //RuleFor(v => v.SiteAudience)
            //    .Must(s => s != SiteAudienceTypesDto.Default)
            //        .When(s => s.Location == LocationTypesDto.Online);

          
          RuleFor(v => v.EntityPlaces)
              .NotNull()
              .NotEmpty()
                  .WhenAsync(EnableEmptyPlacesOnlyForNotAuthorizedEntityPridicate);

            RuleForEach(v => v.EntityPlaces)
                .EntityPlaceValidationRule()
                    .When(v => v.EntityPlaces?.Any() == true);

            RuleFor(v => v.SrnEntity)
                 .SrnPoolValidationRule()
                     .When(v => v.SrnEntity != null);

            RuleFor(v => v.SrnEntity)
                 .Must(v => v == null)
                 .WhenAsync(EnableEditSystematicOnlyForNotAuthorizedEntityPridicate);

            RuleFor(v => v.EntityPlaces)
                .Must(v => v == null && !v.Any())
                .WhenAsync(EnableEditSystematicOnlyForNotAuthorizedEntityPridicate);


            //todo agn grp taps???
        }



        private Func<UpdateEntityEntityCommandDto, CancellationToken, Task<bool>> EnableEditSystematicOnlyForNotAuthorizedEntityPridicate =>
            async (v, token) => {
                var authorizationStatus = await _authorizationStatusQualifier.Handle(v.ValidityId);
                var Systematic = await _SystematicQualifier.Handle(v.ValidityId);
                return Systematic != v.Systematic && authorizationStatus == EntityEntityAuthorizationStatus.NotAuthorized;
            };

        private Func<UpdateEntityEntityCommandDto, CancellationToken, Task<bool>> EnableEmptyPlacesOnlyForNotAuthorizedEntityPridicate =>
            async (v, token) => {
                var authorizationStatus = await _authorizationStatusQualifier.Handle(v.ValidityId);
                return authorizationStatus == EntityEntityAuthorizationStatus.Authorized
                 && (v.Systematic == SystematicTypesDto.ElioET || v.Systematic == SystematicTypesDto.UesET || v.Systematic == SystematicTypesDto.SabreET);
            };
    }
}
