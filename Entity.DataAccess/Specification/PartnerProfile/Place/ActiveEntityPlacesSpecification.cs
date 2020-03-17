using System;
using System.Linq;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.PartnerProfile
{
    public class ActiveSrnPoolSpecification : Specification<SrnOffc>
    {
        public override Expression<Func<SrnOffc, bool>> ToExpression() => o => o.Ep.SrnStatus == true && o.Ep.EpSystem == EpSystems.Srn;
    }
    namespace Entity
    {
        public class NotOwnedByEntitypecification : Specification<Ep>
        {
            private readonly Guid _validityId;
            public NotOwnedByEntitypecification(Guid validityId)
            {
                if (validityId == Guid.Empty)
                {
                    throw new ArgumentNullException();
                }

                _validityId = validityId;
            }
            public override Expression<Func<Ep, bool>> ToExpression() =>
                Ep => Ep.EntityBasePoint.EntityExpirations.All(v => v.EntityExpirationGuid != _validityId);
        }
    }

    namespace Srn
    {
        public class NotOwnedByEntitypecification : Specification<SrnOffc>
        {
            private readonly Guid _validityId;
            public NotOwnedByEntitypecification(Guid validityId)
            {
                if (validityId == Guid.Empty)
                {
                    throw new ArgumentNullException();
                }

                _validityId = validityId;
            }

            public override Expression<Func<SrnOffc, bool>> ToExpression() =>
                Pool => Pool.Ep.EntityBasePoint.EntityExpirations.All(v => v.EntityExpirationGuid != _validityId);
        }

    }

}
