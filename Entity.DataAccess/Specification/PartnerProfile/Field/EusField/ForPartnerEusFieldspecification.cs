using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.PartnerProfile
{
    public class ForAgentEusEntitypecification : Specification<EusPlace>
    {
        private readonly HashSet<string> agentCodes;

        public ForAgentEusEntitypecification(HashSet<string> agentCodes)
        {
            if (agentCodes == null || !agentCodes.Any())
                throw new ArgumentNullException(nameof(agentCodes));

            this.agentCodes = agentCodes;
        }
        public override Expression<Func<EusPlace, bool>> ToExpression() => x =>
            agentCodes.Contains(x.EusMainPoint.AgentCode);
    }
}