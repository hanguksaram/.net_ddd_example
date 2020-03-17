using System;
using System.Threading.Tasks;
using System.Transactions;
using Entity.ApplicationServices;
using Entity.ApplicationServices.Repositories;
using Entity.DataAccess.Repository;
using Entity.Domain;
using Xunit;

namespace Entity.DataAccess.Tests.Entity.Create
{
    public class CreateSrnEntityTests: IDisposable
    {
        internal EntityContext Context { get; }

        private readonly TransactionScope _scope;
        public CreateSrnEntityTests()
        {
            _scope = new TransactionScope(TransactionScopeOption.RequiresNew, TransactionScopeAsyncFlowOption.Enabled);
            Context = new EntityContext();
        }

        private IEntityEntityRepository GetRepository()
        {
            return new EntityEntityRepository(Context);
        }

        public void Dispose()
        {
            Context.Dispose();
            _scope.Dispose();
        }

        [Fact]
        public async Task Successfull_Creation_For_Authorized_Entity() 
        {
            var valRepo = GetRepository();

            var now = new DateTime(2020, 01, 11);
            var Entity = GetValidAuthorizedEntity(now);

            await valRepo.Create(Entity, PredefinedUsers.MockUser, now);
        }


        [Fact]
        public async Task Successfull_Creation_For_Not_Authorized_Entity()
        {
            var valRepo = GetRepository();

            var now = new DateTime(2020, 01, 11);
            var Entity = GetValidNotAuthorizedEntity(now);
            await valRepo.Create(Entity, PredefinedUsers.MockUser, now);
        }


        private EntityEntity GetValidAuthorizedEntity(DateTime now)
        {
            var val = GetValidNotAuthorizedEntity(now);
            val.SetSrnGroup(new SrnGroup("12ФЫВ", "4211234567"));
            val.AddTap(new SrnPool("ЫЫЫЫ12", now));

            return val;
        }

        private EntityEntity GetValidNotAuthorizedEntity(DateTime now)
        {
            return new EntityEntity(new EntitySales(new Guid("28ab8f55-2cdf-4a82-8cf9-8f324548ed7f"), "PS00750", "12001"), new EntityNumber("421-7848-5"), EntitySystematicType.Srn, now);
        }
    }
}
