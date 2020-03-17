using System;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.PartnerProfile
{
    public class SrnTermianlExactTapMatchingSpecification : Specification<SrnOffc>
    {
        private readonly string _identificator;
        public SrnTermianlExactTapMatchingSpecification(string identificator)
        {
            if (string.IsNullOrEmpty(identificator))
            {
                throw new ArgumentNullException();
            }

            _identificator = identificator.Trim();

            if (string.IsNullOrWhiteSpace(_identificator))
                throw new ArgumentNullException();
        }
        public override Expression<Func<SrnOffc, bool>> ToExpression() => v => v.Offc.Equals(_identificator);

    }
}
