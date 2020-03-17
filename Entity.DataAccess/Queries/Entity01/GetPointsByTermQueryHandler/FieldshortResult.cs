using Entity.ApplicationServices.Queries.Entity01;
using Entity.Domain;
using System;

namespace Entity.DataAccess.Queries.Entity01
{
    public class EntityhortResult: IEntityhortResult
    {
        public Guid Id { get; set; }
        public EntitySysType SysType { get; set; }
        public string Number { get; set; }
        public string[] Places { get; set; }
    }
}