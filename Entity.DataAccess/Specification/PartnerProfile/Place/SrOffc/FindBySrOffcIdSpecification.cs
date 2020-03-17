using System;
using System.Linq;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.PartnerProfile
{
    public class FindByRnOffcIdSpecification : Specification<RnOffc>
    {
        private readonly string searchTerm;
        public FindByRnOffcIdSpecification(string searchTerm)
        {
            if (searchTerm == null)
                throw new ArgumentNullException(nameof(searchTerm));

            this.searchTerm = searchTerm.Trim();

            if (string.IsNullOrWhiteSpace(searchTerm))
                throw new ArgumentNullException(nameof(searchTerm));
        }

        public override Expression<Func<RnOffc, bool>> ToExpression() =>
            sp => sp.Offc.Contains(searchTerm);
    }

    public class NotEmptyRnOffcSpecification : Specification<RnOffc>
    {
        public override Expression<Func<RnOffc, bool>> ToExpression() =>
            sp => sp.Offc != null && sp.Offc != "" && sp.Lt.GRP != null && sp.Lt.GRP != "" && sp.Lt.RnStatus == true && sp.Lt.LtSystem == LtSystems.Rn;
    }

    public class NotEmptyPlaceIdLtSpecification : Specification<Lt>
    {
        string[] types = new[] { LtSystems.Sabre, LtSystems.Elio, LtSystems.Eus };
        public override Expression<Func<Lt, bool>> ToExpression() =>
            sp => sp.PlaceId != null && sp.PlaceId != "" && sp.LtSystem == LtSystems.Rn;
    }

    public class NotEmptyGrpLtSpecification : Specification<Lt>
    {
        public override Expression<Func<Lt, bool>> ToExpression() =>
            sp => sp.GRP != null && sp.GRP != "" && sp.RnTaps.Any() && sp.RnStatus == true && sp.LtSystem == LtSystems.Rn;
    }
}
