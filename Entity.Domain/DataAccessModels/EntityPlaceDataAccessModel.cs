using System;

namespace Entity.Domain.DataAccessModels
{
    public class EntityPlaceDataAccessModel
    {
        public Guid Id { get; set; }
        public string Pcc { get; set; }
        public DateTime AuthorizationDate { get; set; }
    }
}
