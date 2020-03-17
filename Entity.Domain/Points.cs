using Entity.CrossCutting.Exceptions;
using Entity.Domain.DataAccessModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entity.Domain
{
    public sealed class EntitySales
    {
        private readonly ICollection<EntityEntity> _activeEntity;

        //remove this constructor
        public EntitySales(Guid id, string BasePointId, string DealCode)
        {
            Id = id;
            BasePointId = BasePointId;
            _activeEntity = new List<EntityEntity>();
            if (DealCode != null)
            {
                Deal = new EntityDeal(new EntityDealDataAccessModel { Code = DealCode }); ;
            }
        }

        public EntitySales(EntitySalesDataAccessModel model)
        {
            Id = model.Id;
            BasePointId = model.BasePointId;
            Address = model.Address;
            if (model.EntityDeal != null)
            {
                Deal = new EntityDeal(model.EntityDeal); ;
            }
            DateForActiveEntity = model.DateForActiveEntity;
            _activeEntity = model.ActiveEntity?.Select(v => new EntityEntity(v)).ToList() ?? new List<EntityEntity>();
        }

        public Guid Id { get; private set; }
        public string BasePointId { get; private set; }
        public SaleTypes SaleType { get; private set; }
        /// <summary>
        /// Город привязки
        /// </summary>
        public Town LightTown { get; private set; }
        public SubDeal SubDeal { get; set; }
        public LocationTypes Location { get; private set; }
        public DateTime? DateForActiveEntity { get; private set; }
        public IEnumerable<EntityEntity> ActiveEntity => _activeEntity.AsEnumerable();

        public string Address { get; }
        public EntityDeal Deal { get; }

        public void CreateNewPlaceForEntity(Guid EntityId, EntityPlace Place)
        {
            var Entity = _activeEntity.FirstOrDefault(v => v.Id == EntityId);
            if (Entity == null)
            {
                throw new DomainException($"Для EntitySales({Id}) не найден валидатор({EntityId}) активный на ({DateForActiveEntity})");
            }

            Entity.AddPlace(Place);
        }
        public void AttachEntityEntity(EntityEntity model) => _activeEntity.Add(model);

        public enum SaleTypes : byte
        {
            Own = 0,
            SubDeal = 1
        }
        public enum LocationTypes : byte
        {
            Place = 0,
            Town = 1,
            Online = 2
        }

        public bool IsValid()
        {
            bool validState = true;
         
            if (SaleType == SaleTypes.SubDeal)
                validState &= SubDeal != null;

            return validState;
        }

        public const string _test_BasePoint_Type = "test";

    }
}
