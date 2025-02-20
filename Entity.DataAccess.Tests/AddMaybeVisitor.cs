﻿using System;
using System.Linq.Expressions;

namespace Entity.DataAccess.Tests
{
    public class AddMaybeVisitor : ExpressionVisitor
    {
        //Этот метод мы будем вызывать над выражением, которое нужно преобразовать.
        public Expression<Func<T1, T2>> Modify<T1, T2>(Expression<Func<T1, T2>> expression)
        {
            return (Expression<Func<T1, T2>>)Visit(expression);
        }

        // Этот метод вызывается в том случае, если узлом дерева выражений является обращение к свойству или полю, как раз то, что нам и требуется.
        protected override Expression VisitMember(MemberExpression node)
        {
            Visit(node.Expression);

            var expressionType = node.Expression.Type;
            var memberType = node.Type;

            var withMethodinfo = typeof(AddMaybeVisitor)
                .GetMethod("With")
                .MakeGenericMethod(expressionType, memberType);

            var p = Expression.Parameter(expressionType);
            var l = Expression.Lambda(Expression.MakeMemberAccess(p, node.Member), p);

            return Expression.Call(withMethodinfo,
                node.Expression,
                Expression.Constant(l.Compile(), typeof(Func<,>).MakeGenericType(expressionType, memberType))
                );
        }

        public static TResult With<TSource, TResult>(TSource source, Func<TSource, TResult> action) where TSource : class
        {
            if (source != default(TSource))
                return action(source);
            return default;
        }
    }
}
