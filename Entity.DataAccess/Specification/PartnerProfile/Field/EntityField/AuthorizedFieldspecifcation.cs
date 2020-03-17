using System;
using System.Linq;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.PartnerProfile
{
    public class AuthorizedEntitypecifcation : Specification<EntityExpiration>
    {
        private readonly Guid? _validityId;

        public AuthorizedEntitypecifcation(Guid? validityId = null)
        {
            if (validityId.HasValue)
            {
                _validityId = validityId;
            }
        }
        public override Expression<Func<EntityExpiration, bool>> ToExpression() => val => _validityId != null ?
            val.EntityExpirationGuid == _validityId && val.EntityMainPoint.Lts.Any() : val.EntityMainPoint.Lts.Any();


    }
}
