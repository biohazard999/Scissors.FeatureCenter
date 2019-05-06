using System;
using System.Linq;
using System.Linq.Expressions;
using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using Scissors.Utils;

namespace Scissors.Xpo
{
    public class ExpressionHelper<TObj>
    {
        public string Property<TRet>(Expression<Func<TObj, TRet>> expr)
            => GetPropertyPath(expr);

        public OperandProperty Operand<TRet>(Expression<Func<TObj, TRet>> expr)
            => GetOperand(expr);

        public OperandProperty TypeOperand<TRet>(Expression<Func<TObj, TRet>> expr)
            => new OperandProperty($"{ExpressionHelper.GetPropertyPath(expr)}.{XPObjectType.ObjectTypePropertyName}.TypeName");

        public BinaryOperator IsType<TRet>(Expression<Func<TObj, TRet>> expr, Type t)
            => TypeOperand(expr) == t.FullName;

        private static string GetPropertyPath<TRet>(Expression<Func<TObj, TRet>> expr)
            => ExpressionHelper.GetPropertyPath(expr);

        private static OperandProperty GetOperand<TRet>(Expression<Func<TObj, TRet>> expr)
            => new OperandProperty(ExpressionHelper.GetPropertyPath(expr));

        public static BinaryOperator GetObjectTypeOperator<TRet>(Expression<Func<TObj, TRet>> expr, Type objectType)
            => new OperandProperty($"{ExpressionHelper.GetPropertyPath(expr)}.{XPObjectType.ObjectTypePropertyName}") == objectType.FullName;

        public static BinaryOperator GetObjectTypeOperator()
            => new OperandProperty(XPObjectType.ObjectTypePropertyName) == typeof(TObj).FullName;
    }
}
