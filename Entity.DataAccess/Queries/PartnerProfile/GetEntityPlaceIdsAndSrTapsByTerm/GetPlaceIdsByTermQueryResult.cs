using Entity.ApplicationServices.Queries.PartnerProfile;

namespace Entity.DataAccess.Queries.PartnerProfile
{
    public class GetEntityPlaceIdsAndRnTapsByTermQueryResult : IGetEntityPlaceIdsAndRnTapsByTermQueryResult
    {
        public string[] PlaceIds { get; }

        public static IGetEntityPlaceIdsAndRnTapsByTermQueryResult Empty => new GetEntityPlaceIdsAndRnTapsByTermQueryResult(new string[0]);
        public GetEntityPlaceIdsAndRnTapsByTermQueryResult(string[] PlaceIds)
        {
            PlaceIds = PlaceIds;
        }

        string[] IGetEntityPlaceIdsAndRnTapsByTermQueryResult.PlaceIds => PlaceIds;
    }
}
