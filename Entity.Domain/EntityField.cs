using Entity.Domain.DataAccessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Entity.CrossCutting.Exceptions;

namespace Entity.Domain
{
    //разделить валидаторы сирена и Entity?
    public sealed partial class EntityEntity
    {
        private readonly ICollection<EntityPlace> _Places;
        private readonly ICollection<SrnPool> _SrnPools;
        private string _comment;
        private static readonly DateTime DefaultDateTo = new DateTime(2050,1,1);

        public EntityEntity(EntitySales pos, EntityNumber Entity, EntitySystematicType Systematic, DateTime validFrom, DateTime? validTo = null, bool isTest = false)
        {
            Id = Guid.NewGuid();
            if (pos == null) throw new ArgumentNullException(nameof(pos));
            if (pos.Id == Guid.Empty) throw new InvalidOperationException("Пустой идентификатор пункта продажи");
            if (pos.Deal == null) throw new InvalidOperationException("Пустая информация об агенте");
            if (pos.Deal.Code == null) throw new InvalidOperationException("Пустой код агента");

            EntitySales = new EntityEntitySales(pos);

            Number = Entity.NumberAsIs;
            Systematic = Systematic;
            ValidFrom = validFrom;
            ValidTo = validTo ?? DefaultDateTo;
            IsTest = isTest;
            _Places = new List<EntityPlace>();
            _SrnPools = new List<SrnPool>();
        }

        public EntityEntity(EntityEntityDataAccessModel model)
        {
            Id = model.Id;
            Systematic = model.SystematicType;
            Number = model.Number;
            IsTest = model.IsTest;
            Comment = model.Comment;
            ValidFrom = model.ValidFrom;
            ValidTo = model.ValidTo;
            _Places = model.Places?.Select(o => new EntityPlace(o)).ToList() ?? new List<EntityPlace>();
            _SrnPools = model.SrnEntity?.SrnPools?.Select(t => new SrnPool(t)).ToList() ?? new List<SrnPool>();
            if (model.SrnEntity != null)
            {
                SrnGroup = new SrnGroup(model.SrnEntity);
            }


            if (model.EntityEntitySales != null)
            {
                EntitySales = new EntityEntitySales(model.EntityEntitySales);
            }
        }

        public Guid Id { get; }
        public EntitySystematicType Systematic { get; }
        public string Number { get; }
        public bool IsTest { get; }

        //todo value type
        public SiteAudienceTypes SiteAudience { get; private set; }
        public LocationTypes Location { get; private set; }

        public bool IsAuthorized() => (_Places?.Any() ?? false) || (SrnGroup != null && _SrnPools.Any());

        public DateTime ValidFrom { get; private set; }
        public DateTime ValidTo { get; private set; }


        public IEnumerable<SrnPool> SrnPools => _SrnPools;
        public IEnumerable<EntityPlace> EntityPlaces => _Places;
        public SrnGroup SrnGroup { get; private set; }

        public string Comment
        {
            get => _comment;
            set { _comment = string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim(); }
        }

        public EntityEntitySales EntitySales { get; }

        public void AddPlace(EntityPlace Place)
        {
            if (Systematic != EntitySystematicType.SabreET
                && Systematic != EntitySystematicType.ElioET
                && Systematic != EntitySystematicType.UesET)
                throw new DomainEntityInvariantException($"Нельзя добавить офис на валидатор для СБ {Systematic}");

            _Places.Add(Place);
        }

        public void RemovePlace(EntityPlace Place)
        {
            if (Systematic != EntitySystematicType.SabreET
                && Systematic != EntitySystematicType.ElioET
                && Systematic != EntitySystematicType.UesET)
                throw new DomainEntityInvariantException($"Нельзя удалить офис с валидатора для СБ {Systematic}");

            if(!_Places.Contains(Place))
                throw new DomainEntityInvariantException($"Нельзя удалить несуществующий офис с валидатора");

            if (_Places.Count == 1)
                throw new DomainEntityInvariantException($"Нельзя удалить единственный офис с валидатора");

            _Places.Remove(Place);
        }

        public void AddTap(SrnPool tap)
        {
            if (Systematic != EntitySystematicType.Srn)
                throw new DomainEntityInvariantException($"Нельзя добавить ТАП на валидатор для СБ {Systematic}");

            if (SrnGroup == null)
                throw new DomainEntityInvariantException($"Нельзя добавить ТАП на валидатор без информации о группе Srn");
            
            _SrnPools.Add(tap);
        }

        public void RemoveTap(SrnPool tap)
        {
            if (Systematic != EntitySystematicType.Srn)
                throw new DomainEntityInvariantException($"Нельзя удалить ТАП с валидатора для СБ {Systematic}");

            if (!_SrnPools.Contains(tap))
                throw new DomainEntityInvariantException($"Нельзя удалить несуществующий ТАП с валидатора");

            if (_SrnPools.Count == 1)
                throw new DomainEntityInvariantException($"Нельзя удалить единственный ТАП с валидатора");

            _SrnPools.Remove(tap);
        }

        public void SetSrnGroup(SrnGroup SrnGroup)
        {
            SrnGroup = SrnGroup ?? throw new ArgumentNullException(nameof(SrnGroup));
        }

        public void SetLocation(LocationTypes location, SiteAudienceTypes siteAudience)
        {
            //временно отключена валидация, восстановить после того, как будут отработаны данные по онлайн офисам
            //if (location == LocationTypes.Online && siteAudience == SiteAudienceTypes.Default)
            //    throw new DomainEntityInvariantException("Для онлайн валидатора необходимо указать аудиторию сайта");

            //if (location != LocationTypes.Online && siteAudience != SiteAudienceTypes.Default)
            //    throw new DomainEntityInvariantException("Аудитория сайта не должна быть указана для оффлайн валидатора");

            Location = location;
            SiteAudience = siteAudience;
        }
    }
}
