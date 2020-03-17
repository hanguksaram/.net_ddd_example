using System;

namespace Entity.Domain.DataAccessModels
{
    public class EntityEntitySalesDataAccessModel
    {
        public Guid Id { get; set; }
        public string BasePointId { get; set; }
        public string Address { get; set; }
        public EntityDealDataAccessModel EntityDeal { get; set; }
    }
}