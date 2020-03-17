using System;

namespace Entity.ApplicationServices.Queries.PartnerProfile
{
    public interface IGetEntityByTermQueryResult
    {
        EntityItem[] Entity { get; }
    }

    public class EntityItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
