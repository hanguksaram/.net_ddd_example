using Entity.DataAccess;
using System;
using System.Collections.Generic;

namespace DataAccess.Projections.Entity01
{
    internal class EntityhortResultProjection
    {
        public Guid Id { get; set; }
        public Guid? PosId { get; set; }
        public string Number { get; set; }
        public IEnumerable<string> Places { get; set; }
        public bool IsAuthorized { get; set; }
        public SysType Sys { get; set; }
    }
}