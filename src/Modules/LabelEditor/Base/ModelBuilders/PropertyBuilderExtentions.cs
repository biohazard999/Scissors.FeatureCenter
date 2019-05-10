using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scissors.ExpressApp.LabelEditor.Contracts;

namespace Scissors.ExpressApp.ModelBuilders
{
    public static class PropertyBuilderExtentions
    {
        public static IPropertyBuilder<TProperty, TType> UsingLabelPropertyEditor<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
            => builder.UsingEditorAlias(LabelEditorAliases.LabelStringEditor);
    }
}
