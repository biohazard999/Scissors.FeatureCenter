using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;

namespace Scissors.ExpressApp.ModelBuilders
{
    public static class PropertyBuilderExtentions
    {
        public static IPropertyBuilder<T, TType> WithModelDefault<T, TType>(this IPropertyBuilder<T, TType> builder, string propertyName, string propertyValue)
            => builder.WithAttribute(new ModelDefaultAttribute(propertyName, propertyValue));

        public static IPropertyBuilder<T, TType> WithModelDefault<T, TType>(this IPropertyBuilder<T, TType> builder, string propertyName, bool propertyValue)
            => builder.WithModelDefault(propertyName, propertyValue.ToString());

        public static IPropertyBuilder<TProperty, TType> HasCaption<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder, string caption)
            => builder.WithModelDefault(ModelDefaults.Caption, caption);

        public static IPropertyBuilder<TProperty, TType> IsPassword<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
            => builder.WithModelDefault(ModelDefaults.IsPassword, true);

        public static IPropertyBuilder<TProperty, TType> WithPredefinedValues<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder, string value)
            => builder.WithModelDefault(ModelDefaults.PredefinedValues, value);

        public static IPropertyBuilder<TProperty, TType> WithPredefinedValues<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder, params object[] values)
            => builder.WithPredefinedValues(string.Join(";", values));

        public static IPropertyBuilder<TProperty, TType> HasTooltip<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder, string tooltip)
            => builder.WithModelDefault(ModelDefaults.ToolTip, tooltip);

        public static IPropertyBuilder<TProperty, TType> HasDisplayFormat<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder, string displayFormat)
            => builder.WithModelDefault(ModelDefaults.DisplayFormat, displayFormat);
        
        public static IPropertyBuilder<TProperty, TType> UsingPropertyEditor<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder, string propertyEditorTypeName)
            => builder.WithModelDefault(ModelDefaults.PropertyEditorType, propertyEditorTypeName);

        public static IPropertyBuilder<TProperty, TType> UsingPropertyEditor<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder, Type propertyEditorType)
            => builder.UsingPropertyEditor(propertyEditorType.FullName);

        public static IPropertyBuilder<TProperty, TType> HasIndex<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder, int index)
            => builder.WithAttribute(new IndexAttribute(index));

        public static IPropertyBuilder<TProperty, TType> IsVisibleInDetailView<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
            => builder.WithAttribute(new VisibleInDetailViewAttribute(true));

        public static IPropertyBuilder<TProperty, TType> IsNotVisibleInDetailView<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
            => builder.WithAttribute(new VisibleInDetailViewAttribute(false));

        public static IPropertyBuilder<TProperty, TType> IsVisibleInListView<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
         => builder.WithAttribute(new VisibleInListViewAttribute(true));

        public static IPropertyBuilder<TProperty, TType> IsNotVisibleInListView<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
            => builder.WithAttribute(new VisibleInListViewAttribute(false));

        public static IPropertyBuilder<TProperty, TType> IsVisibleInLookupListView<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
            => builder.WithAttribute(new VisibleInLookupListViewAttribute(true));

        public static IPropertyBuilder<TProperty, TType> IsNotVisibleInLookupListView<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
            => builder.WithAttribute(new VisibleInLookupListViewAttribute(false));

        public static IPropertyBuilder<TProperty, TType> IsVisibleInAnyView<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
           => builder
                .IsVisibleInDetailView()
                .IsVisibleInListView()
                .IsVisibleInLookupListView();

        public static IPropertyBuilder<TProperty, TType> IsNotVisibleInAnyView<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
              => builder
                   .IsNotVisibleInDetailView()
                   .IsNotVisibleInListView()
                   .IsNotVisibleInLookupListView();

        public static IPropertyBuilder<TProperty, TType> UsingEditorAlias<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder, string editorAlias)
           => builder.WithAttribute(new EditorAliasAttribute(editorAlias));

        public static IPropertyBuilder<TProperty, TType> ImmediatePostsData<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
           => builder.WithAttribute<ImmediatePostDataAttribute>();

        public static IPropertyBuilder<TProperty, TType> AllowingEdit<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
            => builder.WithModelDefault(ModelDefaults.AllowEdit, true);

        public static IPropertyBuilder<TProperty, TType> NotAllowingEdit<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
            => builder.WithModelDefault(ModelDefaults.AllowEdit, false);

        public static IPropertyBuilder<TProperty, TType> AllowingNew<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
            => builder.WithModelDefault(ModelDefaults.AllowNew, true);

        public static IPropertyBuilder<TProperty, TType> NotAllowingNew<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
            => builder.WithModelDefault(ModelDefaults.AllowNew, false);

        public static IPropertyBuilder<TProperty, TType> AllowingDelete<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
            => builder.WithModelDefault(ModelDefaults.AllowDelete, true);

        public static IPropertyBuilder<TProperty, TType> NotAllowingDelete<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
            => builder.WithModelDefault(ModelDefaults.AllowDelete, false);

        public static IPropertyBuilder<TProperty, TType> AllowingEverything<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
          => builder
                .AllowingDelete()
                .AllowingEdit()
                .AllowingNew();

        public static IPropertyBuilder<TProperty, TType> AllowingNothing<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
          => builder
                .NotAllowingDelete()
                .NotAllowingEdit()
                .NotAllowingNew();
    }
}
