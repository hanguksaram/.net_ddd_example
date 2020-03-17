using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.PartnerProfile
{
    public class ForAgentOffcSpecification : Specification<EusPlace>
    {
        private readonly HashSet<string> agentCodes;
        public ForAgentOffcSpecification(HashSet<string> agentCodes)
        {
            if (agentCodes == null || !agentCodes.Any())
                throw new ArgumentNullException(nameof(agentCodes));

            this.agentCodes = agentCodes;
        }
        public override Expression<Func<EusPlace, bool>> ToExpression() =>
            sp => agentCodes.Contains(sp.AgentCode);
    }
}
