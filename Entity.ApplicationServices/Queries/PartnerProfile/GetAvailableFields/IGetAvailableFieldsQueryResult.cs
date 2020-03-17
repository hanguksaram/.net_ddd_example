using System;

namespace Entity.ApplicationServices.Queries.PartnerProfile
{
    public interface IGetAvailableEntityQueryResult
    {
        EntityModel[] Entity { get;}
    }
    public class EntityModel
    {       
        public Guid? EntityId { get; set; }    
        public string Number { get; set; }

        public object Last()
        {
            throw new NotImplementedException();
        }
    }

}
