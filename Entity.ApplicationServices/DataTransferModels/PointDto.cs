using System;

namespace Entity.ApplicationServices
{
    public sealed class EntitySaleDto
    {
        public Guid Id { get; set; }
        public string Town { get; set; }
        public string Address { get; set; }
    }
}