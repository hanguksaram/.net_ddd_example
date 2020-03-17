using Entity.ApplicationServices.Queries.Entity01;

namespace Entity.DataAccess.Queries.Entity01
{
    public class GetEntitySalesByTermQueryResult: IGetEntitySalesByTermQueryResult
    {
        public static IGetEntitySalesByTermQueryResult Empty => new GetEntitySalesByTermQueryResult(new EntitySalesShortResult[0]);


        public GetEntitySalesByTermQueryResult(EntitySalesShortResult[] result)
        {
            EntitySales = result;
        }

        public EntitySalesShortResult[] EntitySales { get; }

        IEntitySaleShortResult[] IGetEntitySalesByTermQueryResult.EntitySales => EntitySales;
    }
}
