using Entity.ApplicationServices.Queries.PartnerProfile;

namespace Entity.DataAccess.Queries.PartnerProfile
{
    public class GetEntityByTermQueryResult : IGetEntityByTermQueryResult
    {
        public EntityItem[] Entity { get; }
        public static IGetEntityByTermQueryResult Empty => new GetEntityByTermQueryResult(System.Array.Empty<EntityItem>());

        public GetEntityByTermQueryResult(EntityItem[] Entity)
        {
            Entity = Entity;
        }

        EntityItem[] IGetEntityByTermQueryResult.Entity => Entity;
    }
}
