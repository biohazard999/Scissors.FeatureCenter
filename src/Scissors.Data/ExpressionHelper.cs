using System;
using System.Linq;
using System.Linq.Expressions;
using DevExpress.Data.Filtering;
using Scissors.Utils;

namespace Scissors.Data
{
    public class ExpressionHelper<TObj>
    {
        public static ExpressionHelper<TObj> Create() => new ExpressionHelper<TObj>();

        public string Property<TRet>(Expression<Func<TObj, TRet>> expr)
            => GetPropertyPath(expr);

        public OperandProperty Operand<TRet>(Expression<Func<TObj, TRet>> expr)
            => GetOperand(expr);
        
        private static string GetPropertyPath<TRet>(Expression<Func<TObj, TRet>> expr)
            => ExpressionHelper.GetPropertyPath(expr);

        private static OperandProperty GetOperand<TRet>(Expression<Func<TObj, TRet>> expr)
            => new OperandProperty(ExpressionHelper.GetPropertyPath(expr));
    }
}
