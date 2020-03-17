using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Entity.Common;

namespace Entity.CrossCutting
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Expression<Func<T, bool>> expr, bool canAddClause) where T : class
        {
            return !canAddClause ? source : source.Where(expr);
        }
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string ordering, bool desc = false) where T : class
        {
            var orderByStr = desc ? "OrderByDescending" : "OrderBy";
            MethodCallExpression resultExp = GenerateMethodCall<T>(source, orderByStr, ordering);
            return source.Provider.CreateQuery<T>(resultExp);
        }
        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> source, string propertyName, bool desc = false) where T : class
        {
            ParameterExpression parameter;
            LambdaExpression selector;
            CreatePropertySelector<T>(propertyName, out parameter, out selector);

            var orderByMethod = typeof(Enumerable).GetMethods()
                   .First(m =>
                   {
                       var parameters = m.GetParameters().ToList();
                       return desc ? m.Name == "OrderByDescending" : m.Name == "OrderBy" && m.IsGenericMethodDefinition &&
                             parameters.Count == 2;
                   });
            var genericMethod = orderByMethod.MakeGenericMethod(typeof(T));

            var orderFunc = Expression.Lambda(selector, new ParameterExpression[] { parameter });

            var newQuery = (IEnumerable<T>)genericMethod
              .Invoke(genericMethod, new object[] { source, orderFunc });

            return newQuery;

        }

        private static void CreatePropertySelector<T>(string propertyName, out ParameterExpression parameter, out LambdaExpression selector) where T : class
        {
            parameter = Expression.Parameter(typeof(T), "Entity");
            PropertyInfo property;

            Expression propertyAccess;

            if (propertyName.Contains('.'))
            {
                String[] childProperties = propertyName.Split('.');
                property = typeof(T).GetProperty(childProperties[0]);
                propertyAccess = Expression.MakeMemberAccess(parameter, property);
                for (int i = 1; i < childProperties.Length; i++)
                {
                    property = property.PropertyType.GetProperty(childProperties[i]);
                    propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                }
            }
            else
            {
                property = typeof(T).GetProperty(propertyName);
                propertyAccess = Expression.MakeMemberAccess(parameter, property);
            }

            selector = Expression.Lambda(propertyAccess, parameter);
        }

        public static IQueryable<TEntity> ApplyPaging<TEntity>(this IQueryable<TEntity> source, ISortable pagingData) where TEntity : class
        {
            if (!string.IsNullOrEmpty(pagingData.OrderBy))
            {
                source = source.OrderBy(pagingData.OrderBy, pagingData.OrderDesc);
            }
            else
            {
                source = source.OrderBy(x => x);
            }

            if (pagingData.Take != default)
            {
                source = source.Skip(pagingData.Skip).Take(pagingData.Take);
            };

            return source;
        }
        public static IEnumerable<TEntity> ApplyPaging<TEntity>(this IEnumerable<TEntity> source, ISortable pagingData) where TEntity : class, IComparable<TEntity>
        {
            if (!string.IsNullOrEmpty(pagingData.OrderBy))
            {
                source = source.OrderBy(pagingData.OrderBy, pagingData.OrderDesc);
            }
            else
            {
                source = source.OrderBy(x => x);
            }

            if (pagingData.Take != default)
            {
                source = source.Skip(pagingData.Skip).Take(pagingData.Take);
            };

            return source;
        }

        #region privates
        private static LambdaExpression GenerateSelector<TEntity>(String propertyName, out Type resultType) where TEntity : class
        {
            // Create a parameter to pass into the Lambda expression (Entity => Entity.OrderByField).
            var parameter = Expression.Parameter(typeof(TEntity), "Entity");
            //  create the selector part, but support child properties
            PropertyInfo property;
            Expression propertyAccess;
            if (propertyName.Contains('.'))
            {
                // support to be sorted on child fields.
                String[] childProperties = propertyName.Split('.');
                property = typeof(TEntity).GetProperty(childProperties[0]);
                propertyAccess = Expression.MakeMemberAccess(parameter, property);
                for (int i = 1; i < childProperties.Length; i++)
                {
                    property = property.PropertyType.GetProperty(childProperties[i]);
                    propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                }
            }
            else
            {
                property = typeof(TEntity).GetProperty(propertyName);
                propertyAccess = Expression.MakeMemberAccess(parameter, property);
            }
            resultType = property.PropertyType;
            // Create the order by expression.
            return Expression.Lambda(propertyAccess, parameter);
        }

        private static MethodCallExpression GenerateMethodCall<TEntity>(IQueryable<TEntity> source, string methodName, String fieldName) where TEntity : class
        {
            Type type = typeof(TEntity);
            Type selectorResultType;
            LambdaExpression selector = GenerateSelector<TEntity>(fieldName, out selectorResultType);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName,
                                            new Type[] { type, selectorResultType },
                                            source.Expression, Expression.Quote(selector));
            return resultExp;
        }
        #endregion
    }
}
