using System;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.Entity01
{
    public class ForAgentPosSpecification : Specification<EusMainPoint>
    {
        private readonly string agentCode;

        public ForAgentPosSpecification(string agentCode)
        {
            this.agentCode = agentCode 
                ?? throw new ArgumentNullException(nameof(agentCode));
        }

        public override Expression<Func<EusMainPoint, bool>> ToExpression() => x => x.AgentCode == agentCode;
    }
}
