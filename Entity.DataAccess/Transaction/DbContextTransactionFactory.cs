using System;

namespace Entity.DataAccess.Transaction
{
    public class DbContextTransactionFactory : ITransactionProvider
    {
        private readonly Lazy<EntityContext> _context;

        public DbContextTransactionFactory(Lazy<EntityContext> context)
        {
            _context = context;
        }

        public ITransactionDisposable Begin()
        {
            var tran = _context.Value.Database.BeginTransaction();
            return new TransactionDisposable(tran);
        }
    }
}
