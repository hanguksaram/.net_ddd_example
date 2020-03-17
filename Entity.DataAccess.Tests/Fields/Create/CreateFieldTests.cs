using System;
using System.Threading.Tasks;
using System.Transactions;
using Entity.ApplicationServices;
using Entity.ApplicationServices.Repositories;
using Entity.CrossCutting.Exceptions;
using Entity.DataAccess.Repository;
using Entity.Domain;
using Entity.Domain.DataAccessModels;
using Xunit;

namespace Entity.DataAccess.Tests.Entity.Create
{
    public class CreateEntityTests : IDisposable
    {
        internal EntityContext Context { get; }

        private readonly TransactionScope _scope;
        public CreateEntityTests()
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



        //[Fact]
        //public async Task Exception_For_Empty_PosID()
        //{
        //    var valRepo = GetRepository();
        //
        //    var now = new DateTime(2020, 01, 11);
        //ЭКСЕПШОН УЖЕ ТУТ
        //    var Entity = GetValidEntity(Guid.Empty, now);
        //
        //    await Assert.ThrowsAsync<ArgumentNullException>(() => valRepo.Create(Entity, PredefinedUsers.MockUser, now));
        //}

        [Fact]
        public async Task Exception_For_Not_Existing_PosID()
        {
            var valRepo = GetRepository();

            var now = new DateTime(2020, 01, 11);
            var Entity = GetValidEntity(Guid.NewGuid(), now);

            await Assert.ThrowsAsync<NotFoundException>(() => valRepo.Create(Entity, PredefinedUsers.MockUser, now));
        }

        private EntityEntity GetValidEntity(Guid spId, DateTime now)
        {
            return new EntityEntity(new EntitySales(spId,"", "12001"), new EntityNumber("421-7848-5"), EntitySystematicType.Srn, now);
        }
    }
}
