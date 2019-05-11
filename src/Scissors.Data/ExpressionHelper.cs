using System;
using System.Linq;
using System.Linq.Expressions;
using DevExpress.Data.Filtering;
using Scissors.Utils;

namespace Scissors.Data
{
    /// <summary>
    /// Helper-Class to generate strongly typed [Operands](https://documentation.devexpress.com/CoreLibraries/DevExpress.Data.Filtering.OperandProperty.members) and PropertyPaths
    /// </summary>
    /// <typeparam name="TObj"></typeparam>
    public class ExpressionHelper<TObj>
    {
        /// <summary>
        /// Static Factory method to create
        /// </summary>
        public static ExpressionHelper<TObj> Create() => new ExpressionHelper<TObj>();

        /// <summary>
        /// Returns a PropertyPath. Multiple nestings are allowed.
        /// `Exp.Property(p => p.A.B.C)` returns `A.B.C`
        /// </summary>
        public string Property<TRet>(Expression<Func<TObj, TRet>> expr)
            => GetPropertyPath(expr);

        /// <summary>
        /// Returns an [OperandProperty](https://documentation.devexpress.com/CoreLibraries/DevExpress.Data.Filtering.OperandProperty.members). Multiple nestings are allowed.
        /// `Exp.Property(p => p.A.B.C)` returns `new OperandProperty(A.B.C)`
        /// </summary>
        public OperandProperty Operand<TRet>(Expression<Func<TObj, TRet>> expr)
            => GetOperand(expr);
        
        private static string GetPropertyPath<TRet>(Expression<Func<TObj, TRet>> expr)
            => ExpressionHelper.GetPropertyPath(expr);

        private static OperandProperty GetOperand<TRet>(Expression<Func<TObj, TRet>> expr)
            => new OperandProperty(ExpressionHelper.GetPropertyPath(expr));
    }
}
