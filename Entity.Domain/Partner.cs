using System;

namespace Entity.Domain
{
    public class EntityDeal
    {
        public EntityDeal(DataAccessModels.EntityDealDataAccessModel model)
        {
            Id = model.Id;
            Code = model.Code;
            Name = model.Name;
        }

        public EntityDeal(string code, string name, Guid id)
        {
            Code = code;
            Name = name;
            Id = id;
        }

        public Guid Id { get; }
        public string Code { get; }
        public string Name { get; }
    }
}