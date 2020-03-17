using System;

namespace Entity.ApplicationServices
{
    public interface IEntityEntityDto
    {
        LocationTypesDto Location { get; set; }
        SiteAudienceTypesDto SiteAudience { get; set; }
        SystematicTypesDto? Systematic { get; set; }
        string Comment { get; set; }
        EntityPlaceDto[] EntityPlaces { get; set; }
        SrnEntityDto SrnEntity { get; set; }
    }

    public sealed class CreateEntityEntityCommandDto : IEntityEntityDto
    {
        public Guid UesEntitySaleId { get; set; }
        public string EntityNumber { get; set; }
        public bool IsTest { get; set; }

        public LocationTypesDto Location { get; set; }
        public SiteAudienceTypesDto SiteAudience { get; set; }
        public SystematicTypesDto? Systematic { get; set; }
        public string Comment { get; set; }
        public EntityPlaceDto[] EntityPlaces { get; set; }
        public SrnEntityDto SrnEntity { get; set; }
    }

    public sealed class UpdateEntityEntityCommandDto : IEntityEntityDto
    {
        public Guid ValidityId { get; set; }

        public LocationTypesDto Location { get; set; }
        public SiteAudienceTypesDto SiteAudience { get; set; }
        public SystematicTypesDto? Systematic { get; set; }
        public string Comment { get; set; }
        public EntityPlaceDto[] EntityPlaces { get; set; }
        public SrnEntityDto SrnEntity { get; set; }
    }
}

