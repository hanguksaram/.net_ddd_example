using Entity.CrossCutting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Entity.Common;

namespace Entity.DataAccess
{
    public sealed class ListQueryHandler : IListQueryHandler
    {
        private readonly Lazy<EntityContext> _context;

        public ListQueryHandler(Lazy<EntityContext> context)
        {
            _context = context;
        }

        public Task<TResult[]> HandleAsync<TEntity, TResult>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TResult>> selector)
            where TEntity : class
        {
            return _context.Value.Set<TEntity>().Where(filter).Select(selector).ToArrayAsync();
        }
      
        public Task<TResult[]> HandleAsync<TEntity, TResult>(Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TResult>> selector,
            ISortable pagingData
            )
            where TEntity : class
            where TResult : class
            => _context.Value.Set<TEntity>().Where(filter)
                .Select(selector)
                .Distinct()
                .ApplyPaging(pagingData)
                .ToArrayAsync();
        
        public Task<TResult[]> HandleAsync<TEntity, TResult>(Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, IEnumerable<TResult>>> selector,
            ISortable pagingData
            )
            where TEntity : class
            where TResult : class
            
            => _context.Value.Set<TEntity>().Where(filter)
                .Select(selector)
                //чтобы стала доступной сортировка по коллекциям коллекций сортировака не работает
                .SelectMany(c => c)
                .Distinct()
                .ApplyPaging(pagingData)
                .ToArrayAsync();




    }
}
