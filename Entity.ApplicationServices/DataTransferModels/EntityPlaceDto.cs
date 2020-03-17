using System;

namespace Entity.ApplicationServices
{
    public class EntityPlaceDto
    {
        public Guid? Id { get; set; }
        public string Pcc { get; set; }
        public DateTime AuthorizationDate { get; set; }
    }
}