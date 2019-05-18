using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scissors.ExpressApp.LabelEditor.Contracts;

namespace Scissors.ExpressApp.ModelBuilders
{
    /// <summary>
    /// 
    /// </summary>
    public static class LabelEditorPropertyBuilderExtentions
    {
        /// <summary>
        /// Usings the label property editor.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IPropertyBuilder<TProperty, TType> UsingLabelPropertyEditor<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
            => builder.UsingEditorAlias(LabelEditorAliases.LabelStringEditor);
    }
}
