using System;
using System.Collections.Generic;

namespace Entity.Domain.DataAccessModels
{
    public class EntityEntityDataAccessModel
    {
        public Guid Id { get; set; }
        public EntitySystematicType SystematicType { get; set; }
        public string Number { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public IEnumerable<EntityPlaceDataAccessModel> Places { get; set; } 
        public SrnEntityDataAccessModel SrnEntity { get; set; }
        public EntityEntitySalesDataAccessModel EntityEntitySales { get; set; }
        public bool IsTest { get; set; }
        public string Comment { get; set; }

        public SiteAudienceTypes SiteAudience { get; set; }
        public LocationTypes Location { get; set; }
    }
}
