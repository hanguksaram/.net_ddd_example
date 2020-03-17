using System;
using System.Collections.Generic;

namespace Entity.Domain.DataAccessModels
{
    public class SrnEntityDataAccessModel
    {
        public string Agn { get; set; }
        public string Grp { get; set; }
        public ICollection<SrnPoolDataAccessModel> SrnPools { get; set; }
    }
    public class SrnPoolDataAccessModel
    {
        public Guid Id { get; set; }
        public string PoolNumber { get; set; }
        public DateTime AuthorizationDate { get; set; }
    }
}
