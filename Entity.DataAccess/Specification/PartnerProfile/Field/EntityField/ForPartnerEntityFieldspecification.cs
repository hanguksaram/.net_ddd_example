using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.PartnerProfile
{
    public class ForAgentEntityEntitypecification : Specification<EntityExpiration>
    {
        private readonly HashSet<string> agentCodes;

        public ForAgentEntityEntitypecification(HashSet<string> agentCodes)
        {
            if (agentCodes == null || !agentCodes.Any())
                throw new ArgumentNullException(nameof(agentCodes));

            this.agentCodes = agentCodes;
        }
        public override Expression<Func<EntityExpiration, bool>> ToExpression() => x =>
            agentCodes.Contains(x.EntityMainPoint.EusMainPoint.AgentCode);
    }
}
