using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.PartnerProfile
{
    public class ForAgentLtSpecification : Specification<Lt>
    {
        private readonly HashSet<string> agentCodes;
        public ForAgentLtSpecification(HashSet<string> agentCodes)
        {
            if (agentCodes == null || !agentCodes.Any())
                throw new ArgumentNullException(nameof(agentCodes));

            this.agentCodes = agentCodes;
        }
        public override Expression<Func<Lt, bool>> ToExpression() =>
            sp => agentCodes.Contains(sp.EntityMainPoint.AgentCode);

    }
}
