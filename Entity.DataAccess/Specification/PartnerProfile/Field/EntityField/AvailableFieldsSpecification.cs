using System;
using System.Linq;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.PartnerProfile
{
    public class AvailableEntitySpecification : Specification<Entity>
    {
        private const int Test_DAYS_TO_BE_FREE = 180;
        private const string Entitytarts = "421";
        private readonly DateTime _EntityFreeDate;
        
        public AvailableEntitySpecification(DateTime now)
        {
            if (now == default)
                throw new ArgumentOutOfRangeException();
            
            _EntityFreeDate = now.Date.AddDays(0 - Test_DAYS_TO_BE_FREE); 
        }


        public override Expression<Func<Entity, bool>> ToExpression() => v
            => v.Number.StartsWith(Entitytarts) && (!v.EntityExpirations.Any() || !v.EntityExpirations.Any(ve => ve.ValidTo >= _EntityFreeDate));

    }
}
