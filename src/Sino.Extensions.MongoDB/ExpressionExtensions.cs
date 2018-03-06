using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.Linq.Expressions
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> SelfOr<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2, ParameterExpression param = null)
        {
            var p = param == null ? Expression.Parameter(typeof(T), "t") : param;
            var newExpr1 = ChangeMemberType(expr1, p);
            var newExpr2 = ChangeMemberType(expr2, p);
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(newExpr1, newExpr2), p);
        }

        public static Expression<Func<T, bool>> SelfAnd<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2, ParameterExpression param = null)
        {
            var p = param == null ? expr1.Parameters.First() : param;
            var newExpr2 = ChangeMemberType(expr2.Body, p);
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, newExpr2), p);
        }

        private static Expression ChangeMemberType(Expression body, Expression param)
        {
            if (body is MethodCallExpression || body is BinaryExpression)
            {
                var method = body as MethodCallExpression;
                if (method != null)
                {
                    var member = method.Object as MemberExpression;

                    var newMember = member.Update(param);
                    var newMethod = method.Update(newMember, method.Arguments);
                    return newMethod;
                }
                else
                {
                    var binary = body as BinaryExpression;

                    var member = binary.Left as MemberExpression;
                    if (member == null)
                    {
                        var unary = binary.Left as UnaryExpression;
                        member = unary.Operand as MemberExpression;
                        var newMember = member.Update(param);
                        var newUnary = unary.Update(newMember);
                        var newBinary = binary.Update(newUnary, binary.Conversion, binary.Right);
                        return newBinary;
                    }
                    else
                    {
                        var newMember = member.Update(param);
                        var newBinary = binary.Update(newMember, binary.Conversion, binary.Right);
                        return newBinary;
                    }
                }
            }
            return body;
        }

        public static Expression<Func<T, bool>> WhereAnd<T>(this IEnumerable<Expression<Func<T, bool>>> exprs)
        {
            var p = Expression.Parameter(typeof(T), "p");
            Expression<Func<T, bool>> FinalQuery = f => true;
            foreach (var expr in exprs)
            {
                FinalQuery = FinalQuery.SelfAnd(expr, p);
            }
            return FinalQuery;
        }

        public static Expression<Func<T, bool>> WhereOr<T>(this IEnumerable<Expression<Func<T, bool>>> exprs)
        {
            var p = Expression.Parameter(typeof(T), "p");
            Expression<Func<T, bool>> FinalQuery = t => false;
            foreach (var expr in exprs)
            {
                FinalQuery = FinalQuery.SelfOr(expr, p);
            }
            return FinalQuery;
        }
    }
}
