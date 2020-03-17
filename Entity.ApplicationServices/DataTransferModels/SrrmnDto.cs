using System;

namespace Entity.ApplicationServices
{
    public class SrnEntityDto
    {
        public string Agn { get; set; }
        public string Grp { get;  set; }
        public Guid? Id { get; set; }
        public SrnPoolDto[] SrnPools { get; set; }
    }
    public class SrnPoolDto
    {
        public DateTime AuthorizationDate { get; set; }
        public string PoolNumber { get; set; }
        public Guid? Id { get; set; }
    }
}