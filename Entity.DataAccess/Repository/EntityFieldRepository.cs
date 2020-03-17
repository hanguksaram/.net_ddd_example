using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Entity.ApplicationServices.Repositories;
using Entity.CrossCutting;
using Entity.CrossCutting.Exceptions;
using Entity.Domain;
using Entity.Domain.DataAccessModels;

namespace Entity.DataAccess.Repository
{
    public class EntityEntityRepository : IEntityEntityRepository
    {
        private readonly EntityContext _context;
        public EntityEntityRepository(EntityContext ctx)
        {
            _context = ctx;
        }

        public async Task DeletePlace(Guid PlaceId, string user, DateTime now)
        {
            var Lt = await _context.Lt.FirstOrDefaultAsync(x => x.LtGuid == PlaceId);
            if (Lt == null)
                throw new NotFoundException($"Place {PlaceId} does not found in db for deleting");

            var isAnyOtherEntity = await _context.Lt.AnyAsync(x => x.MainPointGuid == Lt.MainPointGuid && x.LtGuid != PlaceId);
            if (!isAnyOtherEntity)
                throw new InvalidOperationException($"Place {PlaceId} is last one, delete Entity");

            if (Lt.LtSystem == LtSystems.Rn)
            {
                Lt.RnStatus = false;
                Lt.ModifiedBy = user;
                Lt.ModifiedDate = now;
            }
            else {
                _context.Lt.Remove(Lt);
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEntity(EntityEntity Entity, string user, DateTime now)
        {
            var EntityEntity = await _context
                .EntityExpiration
                .Include(v => v.EntityMainPoint)
                .FirstOrDefaultAsync(v => v.EntityExpirationGuid == Entity.Id);
            
            if (EntityEntity?.EntityMainPoint == null)
            {
                throw new NotFoundException($"Entity for MainPoint {Entity.Id} does not found in db for deleting");
            }

            if (EntityEntity.EntityMainPoint.RevokedByCorrelationId.HasValue)
                throw new InvalidOperationException($"Cannot delete Entity on revocation process with requset {EntityEntity.EntityMainPoint.RevokedByRequestNumber}");

            EntityEntity.ValidTo = EntityEntity.ValidFrom.Value.AddDays(-1);
            EntityEntity.ModifiedBy = user;
            EntityEntity.ModifiedDate = now;
            EntityEntity.EntityMainPoint.ModifiedDate = now;
            EntityEntity.EntityMainPoint.ModifiedBy = user;

            await _context.SaveChangesAsync();
        }

        public async Task<EntityEntity> GetEntityById(Guid id)
        {
            var EntityExpiration = await _context.EntityExpiration
                .Where(v => v.EntityExpirationGuid == id)
                .Include(x => x.Entity)
                .Include(x => x.EntityMainPoint)
                .Include(x => x.EntityMainPoint.EusMainPoint)
                .Include(x => x.EntityMainPoint.EusMainPoint.Organization)
                .Include(x => x.EntityMainPoint.Lts)
                .Include(x => x.EntityMainPoint.Lts.Select(c => c.RnTaps))
                .FirstOrDefaultAsync();

            var pos = EntityExpiration.EntityMainPoint.EusMainPoint;
            var model = new EntityEntityDataAccessModel
            {
                Id = EntityExpiration.EntityExpirationGuid,
                IsTest = EntityExpiration.EntityMainPoint.IsTest,
                Number = EntityExpiration.Entity.Number,
                SysType = EntityExpiration.EntityMainPoint.Sys.ToEntitySys(),
                ValidFrom = EntityExpiration.ValidFrom ?? DateTime.MinValue,
                ValidTo = EntityExpiration.ValidTo ?? DateTime.MaxValue,

                Places = EntityExpiration.EntityMainPoint.Sys == SysType.ElioET
                        || EntityExpiration.EntityMainPoint.Sys == SysType.EusET 
                        || EntityExpiration.EntityMainPoint.Sys == SysType.SabreET ? EntityExpiration.EntityMainPoint.Lts?.Select(p => new EntityPlaceDataAccessModel
                        {
                            Id = p.LtGuid,
                            Pcc = p.PlaceId,
                            AuthorizationDate = p.AutorizeDate
                        }) : Array.Empty<EntityPlaceDataAccessModel>(),

                RnEntity = EntityExpiration.EntityMainPoint.Sys == SysType.Rn
                        ? EntityExpiration.EntityMainPoint.Lts
                                .Where(x => x.LtSystem == LtSystems.Rn && x.RnStatus)
                                .Select(Lt => new RnEntityDataAccessModel
                                {
                                    Grp = Lt.GRP,
                                    Agn = Lt.AGN,
                                    RnTerminals = Lt.EntityMainPoint.Lts.Where(x => x.LtSystem == LtSystems.Rn && x.RnStatus)
                                            .Select(si => new RnTerminalDataAccessModel
                                            {
                                                Id = si.LtGuid,
                                                TerminalNumber = si.RnTaps.FirstOrDefault().Offc,
                                                AuthorizationDate = si.AutorizeDate
                                            }).ToList()
                                })
                                .FirstOrDefault()
                         : null,

                EntityEntitySales = pos == null ? null : new EntityEntitySalesDataAccessModel
                {
                    Id = pos.Id,
                    Address = pos.Address,
                    MainPointId = pos.MainPointId,
                    EntityAgent = pos.Organization == null ? null : new EntityAgentDataAccessModel 
                    {
                        Code = pos.Organization.Code,
                        Id = pos.Organization.PartnerGuid,
                        Name = pos.Organization.Name
                    }
                }
            };

            return new EntityEntity(model);
        }

        public async Task Create(EntityEntity newEntity, IPrincipal principal, DateTime now)
        {
            if (newEntity == null)
            {
                throw new ArgumentNullException(nameof(newEntity));
            }

            if (newEntity.EntitySales == null || newEntity.EntitySales.Id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(newEntity.EntitySales));
            }

            if (principal?.Identity == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            var MainPoint = await _context.EusMainPoint.FirstOrDefaultAsync(s => s.Id == newEntity.EntitySales.Id);

            if (MainPoint == null)
            {
                throw new NotFoundException($"MainPoint ({newEntity.EntitySales.Id}) не найден для создания валидатора");
            }


            var dbEntityNumber = await _context.Entity.FirstOrDefaultAsync(v => v.Number == newEntity.Number);

            if (dbEntityNumber == null)
            {
                throw new NotFoundException($"Entity с номером ({newEntity.Number}) не найден");
            }

            var MainPointType = _context.MainPointType.FirstOrDefault(st => st.Name == EntitySales._Test_MainPoint_Type);
 
            var EntityMainPointEntity = CreateEntityMainPoint(newEntity.Id,
                newEntity.Sys.ToSys(),
                MainPoint,
                newEntity.IsTest,
                now,
                principal.Identity.Name,
                dbEntityNumber.EntityGuid,
                MainPointType);

            UpdateEntityMainPointEntity(newEntity, EntityMainPointEntity, principal, now);            

            _context.MainPoint.Add(EntityMainPointEntity);

            await _context.SaveChangesAsync();
        }

        private MainPoint CreateEntityMainPoint(Guid id, SysType systemType, EusMainPoint asp, bool isTest, DateTime now, string userName, Guid EntityId, MainPointType MainPointType)
        {
            var sp = new MainPoint
            {
                MainPointGuid = Guid.NewGuid(),
                Sys = systemType,
                IsTest = isTest,
                AgentCode = asp.AgentCode,
                CityCode = asp.CityCode,
                Address = asp.Address ?? string.Empty,
                SalingType = asp.SaleType == (byte)EntitySales.SaleTypes.Own,
                IsBlock = false,
                Qaa = false,
                InterfaceOfArs = 0,
                EthereumIsConnected = false,
                IsCharter = false,
                CreatedDate = now,
                ModifiedDate = now,
                EusMainPointGuid = asp.Id,
                CreatedBy = userName,
                ModifiedBy = userName,
                MainPointType = MainPointType
            };

            sp.EntityExpirations.Add(new EntityExpiration
            {
                EntityExpirationGuid = id,
                ValidFrom = now.Date,
                ValidTo = new DateTime(2050, 1, 1),
                EntityGuid = EntityId,
                ModifiedDate = now,
                IsActive = true
            });

            return sp;
        }

        public async Task Update(EntityEntity EntityEntity, IPrincipal principal, DateTime now)
        {
            if (EntityEntity == null)
            {
                throw new ArgumentNullException(nameof(EntityEntity));
            }
            if (EntityEntity.Id == Guid.Empty)
                throw new ArgumentNullException(nameof(EntityEntity));

            var EntityMainPointEntity = await _context.EntityExpiration
                .Include(s => s.EntityMainPoint)
                .Include(s => s.EntityMainPoint.Lts)
                .Include(s => s.EntityMainPoint.Lts.Select(p => p.RnTaps))
                .Where(s => s.EntityExpirationGuid == EntityEntity.Id)
                .Select(s => s.EntityMainPoint)
                .FirstOrDefaultAsync();

            if (EntityMainPointEntity == null)
            {
                throw new NotFoundException($"Валидатор ({EntityEntity.Id}) не найден для обновления");
            }

            UpdateEntityMainPointEntity(EntityEntity, EntityMainPointEntity, principal, now);

            await _context.SaveChangesAsync();
        }

        private void UpdateEntityMainPointEntity(EntityEntity EntityEntity, MainPoint EntityMainPointEntity, IPrincipal principal, DateTime now)
        {
            switch (EntityEntity.Sys)
            {
                case EntitySysType.EusET:
                case EntitySysType.ElioET:
                case EntitySysType.SabreET:
                    {
                        var LtSystem = EntityMainPointEntity.Sys.ToLtSystem();
                        var PlacesInDb = EntityMainPointEntity.Lts.Where(x => x.LtSystem == LtSystem).ToDictionary(x => x.LtGuid);
                        var PlacesInModel = EntityEntity.EntityPlaces ?? Array.Empty<EntityPlace>();

                        if (PlacesInDb.Any() && !PlacesInModel.Any())
                            throw new InvalidOperationException($"Нельзя удалить все офисы на валидаторе ({EntityEntity.Id})");

                        if (PlacesInModel.Any())
                            EntityMainPointEntity.AuthorizationDate = PlacesInModel.Min(x => x.AuthorizationDate);

                        foreach (var Place in PlacesInModel)
                        {
                            if (PlacesInDb.TryGetValue(Place.Id, out var existed))
                            {
                                UpdateFromPlace(existed, Place);

                                PlacesInDb.Remove(Place.Id);
                            }
                            else
                            {
                                var newLt = CreateLt(Place.Id,
                                    EntityEntity.EntitySales.Agent.Code,
                                    EntityMainPointEntity.MainPointGuid,
                                    LtSystem,
                                    principal.Identity.Name,
                                    now);

                                UpdateFromPlace(newLt, Place);

                                EntityMainPointEntity.Lts.Add(newLt);
                            }
                        }
                        if (PlacesInDb.Any())
                            _context.Lt.RemoveRange(PlacesInDb.Values);


                        break;
                    }
                case EntitySysType.Rn:
                    {
                        var Rn = EntityMainPointEntity.Lts.Where(x => x.LtSystem == LtSystems.Rn && x.RnStatus && x.RnTaps.Any())
                            .GroupBy(x => new { x.AGN, x.GRP }).SingleOrDefault();

                        if (EntityEntity.RnGroup == null && Rn == null)
                            break;

                        if (EntityEntity.RnGroup == null && Rn != null)
                        {
                            throw new NotFoundException($"Rn терминал у пункта продаж ({EntityEntity.Id}) не найден для обновления");
                        }

                        if (!principal.IsAdmin() && Rn != null)
                        {
                            if (EntityEntity.RnGroup.Agn != Rn.Key.AGN
                                || EntityEntity.RnGroup.Grp != Rn.Key.GRP)
                                throw new InvalidOperationException($"Нельзя изменить ТКП и ГРП на авторизованном валидаторе ({EntityEntity.Id})");
                        }

                        var tapsInDb = Rn?.ToDictionary(x => x.LtGuid) ?? new Dictionary<Guid, Lt>();
                        var tapsInModel = EntityEntity.RnTerminals ?? Array.Empty<RnTerminal>();

                        if (tapsInDb.Any() && !tapsInModel.Any())
                            throw new InvalidOperationException($"Нельзя удалить все ТАП на валидаторе ({EntityEntity.Id})");

                        if (tapsInModel.Any())
                            EntityMainPointEntity.AuthorizationDate = tapsInModel.Min(x => x.AuthorizationDate);

                        foreach (var tap in tapsInModel)
                        {
                            if (tapsInDb.TryGetValue(tap.Id, out var existed))
                            {
                                UpdateFromTap(existed, EntityEntity.RnGroup, tap);

                                tapsInDb.Remove(tap.Id);
                            }
                            else
                            {
                                var newLt = CreateLt(tap.Id,
                                    EntityEntity.EntitySales.Agent.Code,
                                    EntityMainPointEntity.MainPointGuid,
                                    LtSystems.Rn,
                                    principal.Identity.Name,
                                    now);


                                UpdateFromTap(newLt, EntityEntity.RnGroup, tap);

                                EntityMainPointEntity.Lts.Add(newLt);
                            }
                        }
                        if (tapsInDb.Any())
                        {
                            foreach (var Lt in tapsInDb.Values)
                            {
                                Lt.ReleaseDate = now;
                                Lt.RnStatus = false;
                            }
                        }
                        break;
                    }
                case EntitySysType.Unknown when EntityMainPointEntity.AgentCode == "1488":
                    break;
                default:
                    throw new DomainException($"Reservation system for Entity ({EntityMainPointEntity.MainPointGuid}) is not recognized");
            }
#warning добавить изменение Location при аптейте связанной EusMainPoint в ETR.Test.Eus.BusinessLogic.MainPoint.UpdateMainPointHandler

            EntityMainPointEntity.Location = (EntityPlaceLocationTypes)EntityEntity.Location;
            EntityMainPointEntity.IsOnline = EntityEntity.Location == LocationTypes.Online;
            EntityMainPointEntity.Rto = EntityEntity.Location == LocationTypes.Rto;
            EntityMainPointEntity.SiteAudience = (EntityPlaceSiteAudienceTypes)EntityEntity.SiteAudience;
            EntityMainPointEntity.Comment = EntityEntity.Comment;

            //можно указывать только при создании
            //MainPoint.IsTest = cmd.IsTest;

            EntityMainPointEntity.ModifiedBy = principal.Identity.Name;
            EntityMainPointEntity.ModifiedDate = now;
        }

        private static void UpdateFromTap(Lt Lt, RnGroup group, RnTerminal tap)
        {
            Lt.RnStatus = true;
            Lt.AutorizeDate = tap.AuthorizationDate;
            Lt.ValidFrom = tap.AuthorizationDate;
            Lt.AGN = group.Agn;
            Lt.GRP = group.Grp;

            var RnTap = Lt.RnTaps.SingleOrDefault();

            if (RnTap == null)
            {
                RnTap = new RnOffc();
                Lt.RnTaps.Add(RnTap);
            }
            RnTap.Offc = tap.Tap;
        }

        private static void UpdateFromPlace(Lt Lt, EntityPlace Place)
        {
            Lt.PlaceId = Place.Pcc;
            Lt.AutorizeDate = Place.AuthorizationDate;
            Lt.ValidFrom = Place.AuthorizationDate;
        }

        private Lt CreateLt(Guid id, string code, Guid MainPointGuid, string LtSystem, string userName, DateTime now)
        {
            return new Lt
            {
                LtGuid = id,
                ASIACode = code,
                MainPointGuid = MainPointGuid,
                LtSystem = LtSystem,
                AK = "Test",
                EMD = false,
                Comment = string.Empty,
                OFFC = string.Empty,
                Status = true,
                CreatedBy = userName,
                ModifiedBy = userName,
                CreatedDate = now,
                ModifiedDate = now
            };
        }
    }
}
