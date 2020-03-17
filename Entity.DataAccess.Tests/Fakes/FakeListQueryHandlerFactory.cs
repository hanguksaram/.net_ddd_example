using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Entity.Common;
using Entity.CrossCutting;

namespace Entity.DataAccess.Tests.Fakes
{
    internal class FakeListQueryHandler : IListQueryHandler
    {
        public HashSet<object> QuerySource = new HashSet<object>();
        private IQueryable<object> _querySource => QuerySource.AsQueryable();

        public void Dispose()
        {
        }

        public Task<TResult[]> HandleAsync<TEntity, TResult>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TResult>> selector) where TEntity : class
        {
            var modifiedWhere = new AddMaybeVisitor().Modify(filter);
            //return null instead NullRefEx on b=>b.Author.Name if b.Author is null
            var modifiedSelector = new AddMaybeVisitor().Modify(selector);
            return Task.FromResult(_querySource.OfType<TEntity>().Where(modifiedWhere).Select(modifiedSelector).ToArray());
        }

        public Task<TResult[]> HandleAsync<TEntity, TResult>(Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TResult>> selector, ISortable pagingData)
            where TEntity : class where TResult : class
        {
            var modifiedWhere = new AddMaybeVisitor().Modify(filter);
            var modifiedSelector = new AddMaybeVisitor().Modify(selector);
            return Task.FromResult(_querySource.OfType<TEntity>()
                .Where(modifiedWhere)
                .Select(modifiedSelector)
                .ApplyPaging(pagingData)
                .ToArray());
        }


        public Task<TResult[]> HandleAsync<TEntity, TResult>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, IEnumerable<TResult>>> selector, ISortable pagingData)
            where TEntity : class
            where TResult : class
        {
            var modifiedWhere = new AddMaybeVisitor().Modify(filter);
            //return null instead NullRefEx on b=>b.Author.Name if b.Author is null
            var modifiedSelector = new AddMaybeVisitor().Modify(selector);
            return Task.FromResult(_querySource.OfType<TEntity>().Where(modifiedWhere).SelectMany(modifiedSelector).ToArray());
        }
    }
}
