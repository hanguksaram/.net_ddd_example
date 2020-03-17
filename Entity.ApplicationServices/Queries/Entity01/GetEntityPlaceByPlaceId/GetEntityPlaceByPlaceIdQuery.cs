namespace Entity.ApplicationServices.Queries.Entity01
{

    public class GetEntityPlaceByPlaceIdQuery
    {
        public GetEntityPlaceByPlaceIdQuery(string PlaceId)
        {
            PlaceId = PlaceId;
        }

        public string PlaceId { get; }
    }
}
