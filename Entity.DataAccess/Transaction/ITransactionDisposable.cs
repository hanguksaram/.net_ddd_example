using System;

namespace Entity.DataAccess.Transaction
{
    public interface ITransactionDisposable: IDisposable
    {
        void Commit();
    }
}