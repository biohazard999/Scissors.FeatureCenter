using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using Scissors.Data;
using Scissors.Utils;

namespace Scissors.Xpo
{
   public static class ExpressionHelperExtentions
    {
        public static OperandProperty TypeOperand<TObj, TRet>(this ExpressionHelper<TObj> e, Expression<Func<TObj, TRet>> expr)
          => new OperandProperty($"{ExpressionHelper.GetPropertyPath(expr)}.{XPObjectType.ObjectTypePropertyName}.TypeName");

        public static BinaryOperator IsType<TObj, TRet>(this ExpressionHelper<TObj> e, Expression<Func<TObj, TRet>> expr, Type t)
            => e.TypeOperand(expr) == t.FullName;

        public static BinaryOperator GetObjectTypeOperator<TObj, TRet>(this ExpressionHelper<TObj> e, Expression<Func<TObj, TRet>> expr, Type objectType)
            => new OperandProperty($"{ExpressionHelper.GetPropertyPath(expr)}.{XPObjectType.ObjectTypePropertyName}") == objectType.FullName;

        public static BinaryOperator GetObjectTypeOperator<TObj>(this ExpressionHelper<TObj> e)
            => new OperandProperty(XPObjectType.ObjectTypePropertyName) == typeof(TObj).FullName;
    }
}
