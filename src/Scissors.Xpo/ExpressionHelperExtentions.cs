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
    /// <summary>
    /// 
    /// </summary>
    public static class ExpressionHelperExtentions
    {
        /// <summary>
        /// Types the operand.
        /// </summary>
        /// <typeparam name="TObj">The type of the object.</typeparam>
        /// <typeparam name="TRet">The type of the ret.</typeparam>
        /// <param name="e">The e.</param>
        /// <param name="expr">The expr.</param>
        /// <returns></returns>
        public static OperandProperty TypeOperand<TObj, TRet>(this ExpressionHelper<TObj> e, Expression<Func<TObj, TRet>> expr)
          => new OperandProperty($"{ExpressionHelper.GetPropertyPath(expr)}.{XPObjectType.ObjectTypePropertyName}.TypeName");

        /// <summary>
        /// Determines whether the specified expr is type.
        /// </summary>
        /// <typeparam name="TObj">The type of the object.</typeparam>
        /// <typeparam name="TRet">The type of the ret.</typeparam>
        /// <param name="e">The e.</param>
        /// <param name="expr">The expr.</param>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        public static BinaryOperator IsType<TObj, TRet>(this ExpressionHelper<TObj> e, Expression<Func<TObj, TRet>> expr, Type t)
            => e.TypeOperand(expr) == t.FullName;

        /// <summary>
        /// Gets the object type operator.
        /// </summary>
        /// <typeparam name="TObj">The type of the object.</typeparam>
        /// <typeparam name="TRet">The type of the ret.</typeparam>
        /// <param name="e">The e.</param>
        /// <param name="expr">The expr.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <returns></returns>
        public static BinaryOperator GetObjectTypeOperator<TObj, TRet>(this ExpressionHelper<TObj> e, Expression<Func<TObj, TRet>> expr, Type objectType)
            => new OperandProperty($"{ExpressionHelper.GetPropertyPath(expr)}.{XPObjectType.ObjectTypePropertyName}") == objectType.FullName;

        /// <summary>
        /// Gets the object type operator.
        /// </summary>
        /// <typeparam name="TObj">The type of the object.</typeparam>
        /// <param name="e">The e.</param>
        /// <returns></returns>
        public static BinaryOperator GetObjectTypeOperator<TObj>(this ExpressionHelper<TObj> e)
            => new OperandProperty(XPObjectType.ObjectTypePropertyName) == typeof(TObj).FullName;
    }
}
