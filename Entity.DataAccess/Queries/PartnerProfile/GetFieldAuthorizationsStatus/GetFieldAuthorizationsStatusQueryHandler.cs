using DataAccess;
using Entity.ApplicationServices.Queries.PartnerProfile;
using Entity.ApplicationServices.Queries.PartnerProfile.Models;
using Entity.DataAccess.Specification.PartnerProfile;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Entity.DataAccess.Queries.PartnerProfile
{
    public class GetEntityAuthorizationsStatusQueryHandler : IGetEntityAuthorizationsStatusQueryHandler
    {
        private readonly IListQueryHandler _queryHandler;
        public GetEntityAuthorizationsStatusQueryHandler(
            IListQueryHandler queryHandler)
        {
            _queryHandler = queryHandler;
        }
        public async Task<EntityEntityAuthorizationStatus> Handle(Guid validityId)
        {
            if (validityId == Guid.Empty)
            {
                throw new ArgumentNullException();
            }

            var specification = new AuthorizedEntitypecifcation(validityId);

            var queryResult = await _queryHandler.HandleAsync(specification.ToExpression(), v => v);

            var isAuthorized = queryResult.Any();

            return isAuthorized ? EntityEntityAuthorizationStatus.Authorized : EntityEntityAuthorizationStatus.NotAuthorized; 
        }
    }
}
