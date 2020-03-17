using System;

namespace Entity.DataAccess.Projections.PartnerProfile
{
    public class EntityhortResultProjection : IComparable<EntityhortResultProjection>
    {
        public string Number { get; set; }
        public Guid Id { get; set; }

        public int CompareTo(EntityhortResultProjection other) => Number.CompareTo(other.Number);
        
    }
}
