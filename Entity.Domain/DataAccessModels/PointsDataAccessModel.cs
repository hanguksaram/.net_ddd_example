using System;
using System.Collections.Generic;

namespace Entity.Domain.DataAccessModels
{
    public class EntitySalesDataAccessModel
    {
        public Guid Id { get; set; }
        public string BasePointId { get; set; }
        public DateTime DateForActiveEntity { get; set; }
        public IEnumerable<EntityEntityDataAccessModel> ActiveEntity { get; set; }
        public EntityDealDataAccessModel EntityDeal { get; set; }
        public string Address { get; set; }
    }
}
