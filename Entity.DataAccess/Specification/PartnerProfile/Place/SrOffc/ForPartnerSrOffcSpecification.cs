using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.PartnerProfile
{
    public class ForAgentRnOffcSpecification : Specification<RnOffc>
    {
        private readonly HashSet<string> agentCodes;
        public ForAgentRnOffcSpecification(HashSet<string> agentCodes)
        {
            if (agentCodes == null || !agentCodes.Any())
                throw new ArgumentNullException(nameof(agentCodes));

            this.agentCodes = agentCodes;
        }
        public override Expression<Func<RnOffc, bool>> ToExpression() =>
            sp => agentCodes.Contains(sp.Lt.Agent.Code);

    }
}
