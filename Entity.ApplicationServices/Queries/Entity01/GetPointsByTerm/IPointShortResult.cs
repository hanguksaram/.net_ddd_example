using System;

namespace Entity.ApplicationServices.Queries.Entity01
{
    public interface IEntitySaleShortResult
    {
        Guid Id { get; }
        string BasePointId { get; }
        string Address { get; }
        string AddressLatin { get; }
        IEntityhortResult[] Entity  { get; }
    }
}