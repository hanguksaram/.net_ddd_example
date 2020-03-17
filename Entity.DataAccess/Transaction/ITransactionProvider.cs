namespace Entity.DataAccess.Transaction
{
    public interface ITransactionProvider
    {
        ITransactionDisposable Begin();
    }
}