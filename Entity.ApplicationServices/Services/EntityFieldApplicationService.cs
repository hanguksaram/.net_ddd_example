using FluentValidation;
using System;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Entity.ApplicationServices.Notifications;
using Entity.ApplicationServices.Repositories;
using Entity.ApplicationServices.Entity;
using Entity.CrossCutting;
using Entity.CrossCutting.Exceptions;
using Entity.Domain;
using Entity.Domain.DataAccessModels;

namespace Entity.ApplicationServices.Services
{
    public class EntityEntityApplicationService
    {
        private readonly IEntityEntityRepository _repo;
        private readonly IEntitySalesRepository _EntitySalesRepository;
        private readonly IDatetimeProvider _dateTimeProvider;
        private readonly ISendSrnRevocationMailHandler _notification;
        private readonly UesEtNumberEntity _UesValidation;
        private readonly UpdateEntityEntityCommandEntity _updateValidation;
        private readonly CreateEntityEntityCommandEntity _createValidation;
        private readonly SrnNumberEntity _SrnValidation;
        private readonly ElioNumberEntity _ElioValidation;
        private readonly ISendSrnRevocationProcessMessageHandler _SrnRevocationMessage;

        public EntityEntityApplicationService(
            UesEtNumberEntity UesEntity,
            SrnNumberEntity SrnEntity,
            ElioNumberEntity galEntity,
            IEntitySalesRepository EntitySalesRepository,
            IEntityEntityRepository repo,
            IDatetimeProvider dateTimeProvider,
            ISendSrnRevocationMailHandler notification,
            UpdateEntityEntityCommandEntity updateValidation,
            CreateEntityEntityCommandEntity createValidation,
            ISendSrnRevocationProcessMessageHandler SrnRevocationMessage
            )
        {
            _repo = repo;
            _dateTimeProvider = dateTimeProvider;
            _notification = notification;
            _updateValidation = updateValidation;
            _createValidation = createValidation;
            _SrnRevocationMessage = SrnRevocationMessage;
            _UesValidation = UesEntity;
            _SrnValidation = SrnEntity;
            _ElioValidation = galEntity;
            _EntitySalesRepository = EntitySalesRepository;
        }

        public bool ValidateEntityEntityNumber(EntityEntityDataTransferModel Entity)
        {
            switch (Entity.Systematic)
            {
                case EntitySystematicType.UesET:
                    return _UesValidation.Validate(Entity).IsValid;
                    
                case EntitySystematicType.ElioET:
                    return _ElioValidation.Validate(Entity).IsValid;
                    
                case EntitySystematicType.Srn:
                   return _SrnValidation.Validate(Entity).IsValid;
                    
                default:
                    throw new InvalidOperationException();
            }
        }

        public async Task DeleteEntity(Guid validityId, IPrincipal principal)
        {
            if (principal?.Identity?.Name == null)
            {
                throw new ArgumentNullException($"Unauthenticated user access");
            }

            var Entity = await _repo.GetEntityById(validityId);

            if (Entity == null)
            {
                throw new NotFoundException($"Гдс валидатор ({validityId}) не найден для удаления");
            }

            var now = _dateTimeProvider.GetLocalNow();

            await _repo.DeleteEntity(Entity, principal.Identity.Name, now);

            if (Entity.SrnGroup != null)//todo Srn
            {
                await _notification.Send(Entity);
                await _SrnRevocationMessage.Send(Entity, principal.Identity.Name);
            }

        }
        //todo fix ddd
        public async Task DeleteEntityPlace(Guid PlaceId, IPrincipal principal)
        {         

            var now = _dateTimeProvider.GetLocalNow();


            await _repo.DeletePlace(PlaceId, principal.Identity.Name, now);
        }

        public async Task CreateNewEntity(CreateEntityEntityCommandDto cmd, IPrincipal principal)
        {
            _createValidation.ValidateAndThrow(cmd);

            var now = _dateTimeProvider.GetLocalNow();

            var EntitySales = await _EntitySalesRepository.GetById(cmd.UesEntitySaleId);

            if (EntitySales == null)
            {
                throw new NotFoundException($"EntitySales({cmd.UesEntitySaleId}) не найден");
            }

            var EntityEntity = new EntityEntity(EntitySales,
                new EntityNumber(cmd.EntityNumber),
                cmd.Systematic.GetValueOrDefault().ToEntitySystematic(),
                now.Date,
                isTest: cmd.IsTest);

            UpdateEntity(cmd, principal, EntityEntity);

            await _repo.Create(EntityEntity, principal, now);
        }

        public async Task ChangeExistingEntity(UpdateEntityEntityCommandDto cmd, IPrincipal principal)
        {

            if (principal?.Identity?.Name == null)
            {
                throw new ArgumentNullException($"Unauthenticated user access");
            }

            _updateValidation.ValidateAndThrow(cmd);

            var now = _dateTimeProvider.GetLocalNow();

            var EntityEntity = await _repo.GetEntityById(cmd.ValidityId);

            if (EntityEntity == null)
            {
                throw new NotFoundException($"Валидатор ({cmd.ValidityId}) не найден для редактирования");
            }

            if (EntityEntity.Systematic != cmd.Systematic.GetValueOrDefault().ToEntitySystematic())
                throw new InvalidOperationException($"Нельзя обновить систему бронирования валидатору ({cmd.ValidityId})");
            
            UpdateEntity(cmd, principal, EntityEntity);

            await _repo.Update(EntityEntity, principal, now);
        }

        private static void UpdateEntity(IEntityEntityDto cmd, IPrincipal principal, EntityEntity EntityEntity)
        {
            EntityEntity.Comment = cmd.Comment;
            EntityEntity.SetLocation(cmd.Location.ToDomainType(), cmd.SiteAudience.ToDomainType());

            switch (EntityEntity.Systematic)
            {
                case EntitySystematicType.UesET:
                case EntitySystematicType.ElioET:
                case EntitySystematicType.SabreET:
                    {
                        if (cmd.EntityPlaces == null || !cmd.EntityPlaces.Any())
                        {
                            if (EntityEntity.EntityPlaces.Any())
                                throw new InvalidOperationException($"Нельзя удалить все офисы на валидаторе");
                            else
                                break;
                        }

                        var EntityPlaces = EntityEntity.EntityPlaces.ToDictionary(x => x.Id);

                        foreach (var Place in cmd.EntityPlaces)
                        {
                            if (Place.Id.HasValue && EntityPlaces.TryGetValue(Place.Id.Value, out var existed))
                            {
                                existed.Pcc = Place.Pcc;
                                existed.AuthorizationDate = Place.AuthorizationDate;

                                EntityPlaces.Remove(Place.Id.Value);
                            }
                            else
                                EntityEntity.AddPlace(new EntityPlace(Place.Pcc, Place.AuthorizationDate));
                        }

                        foreach (var Place in EntityPlaces.Values)
                            EntityEntity.RemovePlace(Place);

                        break;
                    }
                case EntitySystematicType.Srn:
                    {
                        if (cmd.SrnEntity == null)
                        {
                            if (EntityEntity.SrnGroup != null)
                                throw new InvalidOperationException($"Нельзя удалить информацию о подключении к СБ Сирена на валидаторе");
                            else
                                break;
                        }

                        if (cmd.SrnEntity.SrnPools == null || !cmd.SrnEntity.SrnPools.Any())
                            throw new InvalidOperationException("Cannot create Srn Entity without taps");

                        if (EntityEntity.SrnGroup == null)
                        {
                            var val = new SrnGroup(cmd.SrnEntity.Agn, cmd.SrnEntity.Grp);
                            EntityEntity.SetSrnGroup(val);

                            foreach (var item in cmd.SrnEntity.SrnPools.Select(x => new SrnPool(x.PoolNumber, x.AuthorizationDate)))
                            {
                                EntityEntity.AddTap(item);
                            }
                        }
                        else
                        {
                            var val = new SrnGroup(cmd.SrnEntity.Agn, cmd.SrnEntity.Grp);
                            if (EntityEntity.SrnGroup != val)
                            {
                                if (principal.IsAdmin())
                                    EntityEntity.SetSrnGroup(val);
                                else
                                    throw new InvalidOperationException("Нельзя обновить информацию о подключении к СБ Сирена на валидаторе");
                            }

                            var EntityTaps = EntityEntity.SrnPools.ToDictionary(x => x.Id);

                            foreach (var tap in cmd.SrnEntity.SrnPools)
                            {
                                if (tap.Id.HasValue && EntityTaps.TryGetValue(tap.Id.Value, out var existed))
                                {
                                    existed.Tap = tap.PoolNumber;
                                    existed.AuthorizationDate = tap.AuthorizationDate;

                                    EntityTaps.Remove(tap.Id.Value);
                                }
                                else
                                    EntityEntity.AddTap(new SrnPool(tap.PoolNumber, tap.AuthorizationDate));
                            }

                            foreach (var tap in EntityTaps.Values)
                                EntityEntity.RemoveTap(tap);
                        }
                        break;
                    }
                case EntitySystematicType.Unknown:
                    if (IsRg(EntityEntity.EntitySales.Deal))
                        break;
                    else
                        goto default;
                default:
                    throw new InvalidOperationException("Невозможно создать валидатор без системы бронирования");
            }
        }

        private static bool IsRg(EntityDeal Deal)
        {
            return Deal.Code == "1488";
        }
    }
}
