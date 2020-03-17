using System.Threading.Tasks;

namespace Entity.ApplicationServices.Queries.Entity01
{
    public interface IGetEntitySalesByTermQueryHandler
    {
        Task<IGetEntitySalesByTermQueryResult> Handle(GetEntitySalesByTermQuery query);
    }
}
