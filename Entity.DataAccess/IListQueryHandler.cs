using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Entity.Common;

namespace Entity.DataAccess
{
    public interface IListQueryHandler
    {
        Task<TResult[]> HandleAsync<TEntity, TResult>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TResult>> selector)
            where TEntity:class;

        Task<TResult[]> HandleAsync<TEntity, TResult>(Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TResult>> selector,
            ISortable pagingData
            )
            where TEntity : class
            where TResult : class;

        Task<TResult[]> HandleAsync<TEntity, TResult>(Expression<Func<TEntity, bool>> filter,
           Expression<Func<TEntity, IEnumerable<TResult>>> selector,
            ISortable pagingData
           )
           where TEntity : class
           where TResult : class;     
    }
}