using Entity.ApplicationServices.Queries.PartnerProfile;
using System.Runtime.Serialization;

namespace Entity.DataAccess.Queries.PartnerProfile
{
    public class GetPlaceIdsByTermQueryResult : IGetPlaceIdsByTermQueryResult
    {
        public string[] PlaceIds { get; }

        public static IGetPlaceIdsByTermQueryResult Empty => new GetPlaceIdsByTermQueryResult(new string[0]);
        public GetPlaceIdsByTermQueryResult(string[] PlaceIds)
        {
            PlaceIds = PlaceIds;
        }

        string[] IGetPlaceIdsByTermQueryResult.PlaceIds => PlaceIds;
    }
}
