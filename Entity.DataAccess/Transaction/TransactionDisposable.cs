using System.Data.Entity;

namespace Entity.DataAccess.Transaction
{
    public class TransactionDisposable : ITransactionDisposable
    {
        private readonly DbContextTransaction tran;

        public TransactionDisposable(DbContextTransaction tran)
        {
            this.tran = tran;
        }

        public void Commit()
        {
            tran.Commit();
        }

        public void Dispose()
        {
            tran.Dispose();
        }
    }
}
