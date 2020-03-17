using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Threading.Tasks;
using Entity.ApplicationServices.Repositories;
using Entity.CrossCutting.Exceptions;
using Entity.Domain;
using Entity.Domain.DataAccessModels;

namespace Entity.DataAccess.Repository
{
    public class EntitySalesRepository : IEntitySalesRepository
    {

        private readonly EntityContext _context;

        public EntitySalesRepository(

            EntityContext context)
        {
            _context = context;
        }

        public async Task<EntitySales> GetByIdWithEntityActiveOnDate(Guid id, DateTime date)
        {
            var MainPoint = await _context.EusMainPoint.FirstOrDefaultAsync(s => s.Id == id);

            if (MainPoint == null)
            {
                return null;
            }

            var activeEntity = await _context.EntityExpiration
                .Where(v => v.EntityMainPoint.EusMainPointGuid == id)
                .Where(v => v.ValidFrom <= date.Date && date.Date <= v.ValidTo && v.IsActive)
                .Include(x => x.Entity)
                .Include(x => x.EntityMainPoint)
                .Include(x => x.EntityMainPoint.EusMainPoint)
                .Include(x => x.EntityMainPoint.EusMainPoint.Organization)
                .Include(x => x.EntityMainPoint.Lts)
                .ToListAsync();

            var model = new EntitySalesDataAccessModel
            {
                DateForActiveEntity = date,
                Id = MainPoint.Id,
                MainPointId = MainPoint.MainPointId,
                Address = activeEntity.Select(v => v.EntityMainPoint.EusMainPoint.Address).FirstOrDefault(),
                EntityAgent = activeEntity.Select(v => new EntityAgentDataAccessModel 
                { 
                    Id = v.EntityMainPoint.EusMainPoint.Organization.PartnerGuid, 
                    Code = v.EntityMainPoint.EusMainPoint.Organization.Code, 
                    Name = v.EntityMainPoint.EusMainPoint.Organization.Name
                }).FirstOrDefault(),
                ActiveEntity = activeEntity.Select(v => new EntityEntityDataAccessModel
                {
                    Id = v.EntityExpirationGuid,
                    Number = v.Entity.Number,
                    SysType = v.EntityMainPoint.Sys.ToEntitySys(),
                    ValidFrom = v.ValidFrom ?? DateTime.MinValue,
                    ValidTo = v.ValidTo ?? DateTime.MaxValue,
                    IsTest = v.EntityMainPoint.IsTest,
                    Places = v.EntityMainPoint.Sys == SysType.ElioET
                        || v.EntityMainPoint.Sys == SysType.EusET 
                        || v.EntityMainPoint.Sys == SysType.SabreET ? v.EntityMainPoint.Lts.Select(p => new EntityPlaceDataAccessModel
                        {
                            Id = p.LtGuid,
                            AuthorizationDate = p.AutorizeDate,
                            Pcc = p.PlaceId
                        }) : Array.Empty<EntityPlaceDataAccessModel>(),

                    RnEntity = v.EntityMainPoint.Sys == SysType.Rn
                        ? v.EntityMainPoint.Lts
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
                }).ToList()
            };


            return new EntitySales(model);
        }
        public async Task<EntitySales> GetByEntityId(Guid id)
        {
            var EntityExpiration = await _context.EntityExpiration
                .Where(v => v.EntityExpirationGuid == id)
                .Include(x => x.Entity)
                .Include(x => x.EntityMainPoint)
                .Include(c => c.EntityMainPoint.EusMainPoint)
                .Include(c => c.EntityMainPoint.EusMainPoint.Organization)
                .Include(x => x.EntityMainPoint.Lts)
                .Include(x => x.EntityMainPoint.Lts.Select(c => c.RnTaps))
                .FirstOrDefaultAsync();

            var MainPoint = EntityExpiration?.EntityMainPoint?.EusMainPoint;

            if (MainPoint == null)
            {
                return null;
            }

            var model = new EntitySalesDataAccessModel
            {
                Id = MainPoint.Id,
                MainPointId = MainPoint.MainPointId,
                Address = MainPoint.Address,
                EntityAgent = MainPoint.Organization == null ? null : new EntityAgentDataAccessModel() { Id = MainPoint.Organization.PartnerGuid, Code = MainPoint.Organization.Code, Name = MainPoint.Organization.Name },
                ActiveEntity = new List<EntityEntityDataAccessModel>() {
                    new EntityEntityDataAccessModel
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
                            AuthorizationDate = p.AutorizeDate,
                            Pcc = p.PlaceId
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
                         : null
                    }
                }
            };


            return new EntitySales(model);

        }
        public async Task<EntitySales> GetById(Guid id)
        {
            var MainPoint = await _context.EusMainPoint
                .Include(x => x.Organization)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (MainPoint == null)
            {
                return null;
            }

            var model = new EntitySalesDataAccessModel
            {
                Id = MainPoint.Id,
                MainPointId = MainPoint.MainPointId,
                Address = MainPoint.Address,
                EntityAgent = MainPoint.Organization == null ? null : new EntityAgentDataAccessModel() { Id = MainPoint.Organization.PartnerGuid, Code = MainPoint.Organization.Code, Name = MainPoint.Organization.Name }
            };

            return new EntitySales(model);
        }

#warning перенести в Entity Entity repository изменить тип параметра PoinOfSales на EntityEntity
        public async Task UpdateEntitySale(EntitySales EntitySales, IPrincipal user, DateTime updatedAt)
        {
            if (EntitySales == null)
            {
                throw new ArgumentNullException(nameof(EntitySales));
            }

            if (user?.Identity == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var MainPoint = await _context.EusMainPoint.FirstOrDefaultAsync(s => s.Id == EntitySales.Id);

            if (MainPoint == null)
            {
                throw new NotFoundException($"MainPoint({EntitySales.Id}) не найден для обновления");
            }

#warning В целях экономии времени в данный момент реализовано только добавление новых EntityPlace-ов(они же пульты, они же терминалы) для БП Entity01.
            var EntityPlaces = EntitySales.ActiveEntity.SelectMany(v => v.EntityPlaces).ToList();
            
            var RnTerminals = EntitySales.ActiveEntity.Select(v => v.RnGroup).ToList();

            var EntityPlacesMissingInDatabase = EntityPlaces.Where(Place => _context.Lt.Any(Lt => Lt.LtGuid == Place.Id) == false).ToList();

            var activeEntityIds = EntitySales.ActiveEntity.Select(x => x.Id).ToList();
            var activeDbExpirationEntity = await _context
                .EntityExpiration
                .Where(ve => activeEntityIds.Contains(ve.EntityExpirationGuid))
                .ToListAsync();


            var dataForNewPlaces = new List<MapPlaceEntityEntityExpiration>();
            foreach (var Place in EntityPlacesMissingInDatabase)
            {
                var EntityForPlace = EntitySales.ActiveEntity.First(v => v.EntityPlaces.Any(o => o.Id == Place.Id));
                var expirationForPlace = activeDbExpirationEntity.First(v => v.EntityExpirationGuid == EntityForPlace.Id);
                var map = new MapPlaceEntityEntityExpiration(Place, EntityForPlace, expirationForPlace);
                dataForNewPlaces.Add(map);
            }


            var newLts = dataForNewPlaces.Select(data => new Lt
            {
                LtGuid = data.Place.Id,
                ASIACode = EntitySales.Agent.Code,
                MainPointGuid = data.EntityExpiration.MainPointGuid,
                PlaceId = data.Place.Pcc,
                LtSystem = data.Entity.Sys.ToLtSystem(),
                AutorizeDate = data.Place.AuthorizationDate,
                AK = "Test",
                EMD = false,
#warning Обязательное поле в БД, изменить при рефакторинге
                Comment = string.Empty,
                OFFC = string.Empty,
                Status = true,
                CreatedBy = user.Identity.Name,
                ModifiedBy = user.Identity.Name,
                CreatedDate = updatedAt,
                ModifiedDate = updatedAt
            });

            _context.Lt.AddRange(newLts);
            await _context.SaveChangesAsync();
        }
        
        private class MapPlaceEntityEntityExpiration
        {
            public MapPlaceEntityEntityExpiration(EntityPlace Place, EntityEntity Entity, EntityExpiration EntityExpiration)
            {
                Place = Place;
                Entity = Entity;
                EntityExpiration = EntityExpiration;
            }

            public EntityPlace Place { get; }
            public EntityEntity Entity { get; }
            public EntityExpiration EntityExpiration { get; }
        }
    }


    internal static class MappingExtensions
    {
        public static string ToLtSystem(this EntitySysType SysType)
        {
            switch (SysType)
            {
                case EntitySysType.EusET:
                    return LtSystems.Eus;

                case EntitySysType.ElioET:
                    return LtSystems.Elio;

                case EntitySysType.SabreET:
                    return LtSystems.Sabre;

                default:
                    throw new ArgumentOutOfRangeException(nameof(SysType), SysType.ToString(), null);
            }
        }
        public static string ToLtSystem(this SysType SysType)
        {
            switch (SysType)
            {
                case SysType.EusET:
                    return LtSystems.Eus;

                case SysType.ElioET:
                    return LtSystems.Elio;

                case SysType.SabreET:
                    return LtSystems.Sabre;
                case SysType.Rn:
                    return LtSystems.Rn;

                default:
                    throw new ArgumentOutOfRangeException(nameof(SysType), SysType.ToString(), null);
            }
        }

        public static EntitySysType ToEntitySys(this SysType dataAccessRsystem)
        {
            switch (dataAccessRsystem)
            {
                case SysType.EusET:
                    return EntitySysType.EusET;
                case SysType.ElioET:
                    return EntitySysType.ElioET;
                case SysType.SabreET:
                    return EntitySysType.SabreET;
                case SysType.Rn:
                    return EntitySysType.Rn;
                default:
                    return EntitySysType.Unknown;
            }
        }
        public static SysType ToSys(this EntitySysType dataAccessRsystem)
        {
            switch (dataAccessRsystem)
            {
                case EntitySysType.EusET:
                    return SysType.EusET;
                case EntitySysType.SabreET:
                    return SysType.SabreET;
                case EntitySysType.ElioET:
                    return SysType.ElioET;
                case EntitySysType.Rn:
                    return SysType.Rn;
                default:
                    return SysType.Default;
            }
        }
    }
}
