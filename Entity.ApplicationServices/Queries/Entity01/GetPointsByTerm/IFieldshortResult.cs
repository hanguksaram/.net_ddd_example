using Entity.Domain;
using System;

namespace Entity.ApplicationServices.Queries.Entity01
{
    public interface IEntityhortResult
    {
        Guid Id { get; }
        EntitySystematicType SystematicType { get; }
        string Number { get; }
        string[] Places { get; }
    }
}