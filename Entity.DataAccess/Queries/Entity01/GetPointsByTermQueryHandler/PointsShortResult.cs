using Entity.ApplicationServices.Queries.Entity01;
using System;

namespace Entity.DataAccess.Queries.Entity01
{
    public class EntitySalesShortResult: IEntitySaleShortResult
    {
        public Guid Id { get; set; }
        public string MainPointId { get; set; }
        public string Address { get; set; }
        public string AddressLatin { get; set; }
        public EntityhortResult[] Entity { get; set; }

        IEntityhortResult[] IEntitySaleShortResult.Entity => Entity;
    }
}