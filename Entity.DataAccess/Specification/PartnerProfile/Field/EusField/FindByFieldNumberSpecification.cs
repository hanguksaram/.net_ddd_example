using System;
using System.Linq;
using System.Linq.Expressions;

namespace Entity.DataAccess.Specification.PartnerProfile
{
    public class FindByEntityEntityNumberSpecification : Specification<EntityExpiration>
    {
        private readonly string searchTerm;
        public FindByEntityEntityNumberSpecification(string searchTerm)
        {
            if (searchTerm == null)
                throw new ArgumentNullException(nameof(searchTerm));

            this.searchTerm = searchTerm.Trim();

            if (string.IsNullOrWhiteSpace(searchTerm))
                throw new ArgumentNullException(nameof(searchTerm));
        }

        public override Expression<Func<EntityExpiration, bool>> ToExpression() => x =>
            x.Entity.Number.Contains(searchTerm);
        
           
    }
    
    public class ActiveEntityNumberSpecification : Specification<EntityExpiration>
    {
        public override Expression<Func<EntityExpiration, bool>> ToExpression() => x => x.IsActive;           
    }

    public class FindByEusEntityNumberSpecification : Specification<EusPlace>
    {
        private readonly string searchTerm;
        public FindByEusEntityNumberSpecification(string searchTerm)
        {
            if (searchTerm == null)
                throw new ArgumentNullException(nameof(searchTerm));

            this.searchTerm = searchTerm.Trim();

            if (string.IsNullOrWhiteSpace(searchTerm))
                throw new ArgumentNullException(nameof(searchTerm));
        }

        public override Expression<Func<EusPlace, bool>> ToExpression() => x =>
            x.DefaultNumberValidity.Select(y => y.DefaultNumber.Number).Any(n => n.Contains(searchTerm)) 
        || x.UADefaultNumberValidity.Select(y => y.UADefaultNumber.Number).Any(n => n.Contains(searchTerm));



    }
}
