using System;
using System.Linq.Expressions;
using System.Text;

namespace Scissors.Utils
{
    public static class ExpressionHelper
    {
        public static MemberExpression GetMemberExpression(Expression expression)
        {
            if(expression is MemberExpression)
            {
                return (MemberExpression)expression;
            }

            if(expression is LambdaExpression)
            {
                var lambdaExpression = expression as LambdaExpression;
                if(lambdaExpression.Body is MemberExpression)
                {
                    return (MemberExpression)lambdaExpression.Body;
                }
                if(lambdaExpression.Body is UnaryExpression)
                {
                    return ((MemberExpression)((UnaryExpression)lambdaExpression.Body).Operand);
                }
            }

            return null;
        }

        public static string GetPropertyPath(Expression expr)
        {
            var path = new StringBuilder();
            var memberExpression = GetMemberExpression(expr);

            do
            {
                path.Insert(0, $".{memberExpression.Member.Name}");

                if(memberExpression.Expression is UnaryExpression ue)
                {
                    memberExpression = GetMemberExpression(ue.Operand);
                }
                else
                {
                    memberExpression = GetMemberExpression(memberExpression.Expression);
                }
            }
            while(memberExpression != null);

            path.Remove(0, 1);
            return path.ToString();
        }
    }
}
