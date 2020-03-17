using System;

namespace Entity.ApplicationServices
{
    public class EntityPlaceRevocationInfo
    {
        public EntityPlaceRevocationInfo(Guid posGuid, string Entity, string PlaceId)
        {
            if (posGuid == Guid.Empty)
                throw new ArgumentException(nameof(posGuid));

            EntitySalesEntityGuid = posGuid;
            EntityNumber = Entity ?? throw new ArgumentNullException(nameof(Entity));
            PlaceId = PlaceId ?? throw new ArgumentNullException(nameof(PlaceId));
        }

        public Guid EntitySalesEntityGuid { get; }
        public string EntityNumber { get; }
        public string PlaceId { get; }
    }
}