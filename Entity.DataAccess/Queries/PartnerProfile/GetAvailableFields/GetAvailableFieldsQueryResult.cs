using Entity.ApplicationServices.Queries.PartnerProfile;


namespace Entity.DataAccess.Queries.PartnerProfile
{
    public class GetAvailableEntityQueryResult : IGetAvailableEntityQueryResult
    {
        public EntityModel[] Entity { get; }
        EntityModel[] IGetAvailableEntityQueryResult.Entity => Entity;
        public GetAvailableEntityQueryResult(EntityModel[] Entity)
        {
            Entity = Entity;
        }
        public static IGetAvailableEntityQueryResult Empty => new GetAvailableEntityQueryResult(System.Array.Empty<EntityModel>());

    }
}
