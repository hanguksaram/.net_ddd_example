namespace Entity.ApplicationServices.Queries.Entity01
{
    public class GetEntitySalesByTermQuery
    {
        public GetEntitySalesByTermQuery(string code, string term, EntitySalesByTermQueryType searchType)
        {
            DealCode = code;
            Term = term;
            SearchType = searchType;
        }

        public string Term { get; }
        public EntitySalesByTermQueryType SearchType { get; }
        public string DealCode { get; }
    }
}
