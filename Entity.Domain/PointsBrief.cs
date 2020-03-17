using System;

namespace Entity.Domain
{
    public class EntityEntitySales
    {

        public EntityEntitySales(DataAccessModels.EntityEntitySalesDataAccessModel model)
        {
            Id = model.Id;
            BasePointId = model.BasePointId;
            Address = model.Address;

            if (model.EntityDeal != null)
            {
                Deal = new EntityDeal(model.EntityDeal);
            }

        }

        public EntityEntitySales(EntitySales pos)
        {
            Id = pos.Id;
            BasePointId = pos.BasePointId;
            Address = pos.Address;
            Deal = pos.Deal;
        }

        public Guid Id { get; set; }
        public string BasePointId { get; set; }
        public string Address { get; set; }
        public EntityDeal Deal { get; }
    }
}