using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;

namespace Scissors.ExpressApp.ModelBuilders
{
    /// <summary>
    /// 
    /// </summary>
    public static class PropertyBuilderExtentions
    {
        /// <summary>
        /// Withes the model default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyValue">The property value.</param>
        /// <returns></returns>
        public static IPropertyBuilder<T, TType> WithModelDefault<T, TType>(this IPropertyBuilder<T, TType> builder, string propertyName, string propertyValue)
            => builder.WithAttribute(new ModelDefaultAttribute(propertyName, propertyValue));

        /// <summary>
        /// Adds an ModelDefaultAttribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyValue">if set to <c>true</c> [property value].</param>
        /// <returns></returns>
        public static IPropertyBuilder<T, TType> WithModelDefault<T, TType>(this IPropertyBuilder<T, TType> builder, string propertyName, bool propertyValue)
            => builder.WithModelDefault(propertyName, propertyValue.ToString());

        /// <summary>
        /// Determines whether the specified caption has caption.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="caption">The caption.</param>
        /// <returns></returns>
        public static IPropertyBuilder<TProperty, TType> HasCaption<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder, string caption)
            => builder.WithModelDefault(ModelDefaults.Caption, caption);

        /// <summary>
        /// Determines whether this instance is password.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IPropertyBuilder<TProperty, TType> IsPassword<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
            => builder.WithModelDefault(ModelDefaults.IsPassword, true);

        /// <summary>
        /// Withes the predefined values.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IPropertyBuilder<TProperty, TType> WithPredefinedValues<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder, string value)
            => builder.WithModelDefault(ModelDefaults.PredefinedValues, value);

        /// <summary>
        /// Withes the predefined values.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static IPropertyBuilder<TProperty, TType> WithPredefinedValues<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder, params object[] values)
            => builder.WithPredefinedValues(string.Join(";", values));

        /// <summary>
        /// Determines whether the specified tooltip has tooltip.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="tooltip">The tooltip.</param>
        /// <returns></returns>
        public static IPropertyBuilder<TProperty, TType> HasTooltip<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder, string tooltip)
            => builder.WithModelDefault(ModelDefaults.ToolTip, tooltip);

        /// <summary>
        /// Determines whether [has display format] [the specified display format].
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="displayFormat">The display format.</param>
        /// <returns></returns>
        public static IPropertyBuilder<TProperty, TType> HasDisplayFormat<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder, string displayFormat)
            => builder.WithModelDefault(ModelDefaults.DisplayFormat, displayFormat);

        /// <summary>
        /// Usings the property editor.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="propertyEditorTypeName">Name of the property editor type.</param>
        /// <returns></returns>
        public static IPropertyBuilder<TProperty, TType> UsingPropertyEditor<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder, string propertyEditorTypeName)
            => builder.WithModelDefault(ModelDefaults.PropertyEditorType, propertyEditorTypeName);

        /// <summary>
        /// Usings the property editor.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="propertyEditorType">Type of the property editor.</param>
        /// <returns></returns>
        public static IPropertyBuilder<TProperty, TType> UsingPropertyEditor<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder, Type propertyEditorType)
            => builder.UsingPropertyEditor(propertyEditorType.FullName);

        /// <summary>
        /// Determines whether the specified index has index.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public static IPropertyBuilder<TProperty, TType> HasIndex<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder, int index)
            => builder.WithAttribute(new IndexAttribute(index));

        /// <summary>
        /// Determines whether [is visible in detail view] [the specified builder].
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IPropertyBuilder<TProperty, TType> IsVisibleInDetailView<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
                    => builder.WithAttribute(new VisibleInDetailViewAttribute(true));

        /// <summary>
        /// Determines whether [is not visible in detail view] [the specified builder].
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IPropertyBuilder<TProperty, TType> IsNotVisibleInDetailView<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
                    => builder.WithAttribute(new VisibleInDetailViewAttribute(false));

        /// <summary>
        /// Determines whether [is visible in ListView].
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IPropertyBuilder<TProperty, TType> IsVisibleInListView<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
         => builder.WithAttribute(new VisibleInListViewAttribute(true));

        /// <summary>
        /// Determines whether [is not visible in ListView].
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IPropertyBuilder<TProperty, TType> IsNotVisibleInListView<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
            => builder.WithAttribute(new VisibleInListViewAttribute(false));

        /// <summary>
        /// Determines whether [is visible in lookup ListView].
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IPropertyBuilder<TProperty, TType> IsVisibleInLookupListView<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
            => builder.WithAttribute(new VisibleInLookupListViewAttribute(true));

        /// <summary>
        /// Determines whether [is not visible in lookup ListView].
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IPropertyBuilder<TProperty, TType> IsNotVisibleInLookupListView<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
            => builder.WithAttribute(new VisibleInLookupListViewAttribute(false));

        /// <summary>
        /// Determines whether [is visible in any view].
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IPropertyBuilder<TProperty, TType> IsVisibleInAnyView<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
           => builder
                .IsVisibleInDetailView()
                .IsVisibleInListView()
                .IsVisibleInLookupListView();

        /// <summary>
        /// Determines whether [is not visible in any view].
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IPropertyBuilder<TProperty, TType> IsNotVisibleInAnyView<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
              => builder
                   .IsNotVisibleInDetailView()
                   .IsNotVisibleInListView()
                   .IsNotVisibleInLookupListView();

        /// <summary>
        /// Usings the editor alias.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="editorAlias">The editor alias.</param>
        /// <returns></returns>
        public static IPropertyBuilder<TProperty, TType> UsingEditorAlias<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder, string editorAlias)
           => builder.WithAttribute(new EditorAliasAttribute(editorAlias));

        /// <summary>
        /// Immediates the posts data.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IPropertyBuilder<TProperty, TType> ImmediatePostsData<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
           => builder.WithAttribute<ImmediatePostDataAttribute>();

        /// <summary>
        /// Allowings the edit.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IPropertyBuilder<TProperty, TType> AllowingEdit<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
            => builder.WithModelDefault(ModelDefaults.AllowEdit, true);

        /// <summary>
        /// Nots the allowing edit.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IPropertyBuilder<TProperty, TType> NotAllowingEdit<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
            => builder.WithModelDefault(ModelDefaults.AllowEdit, false);

        /// <summary>
        /// Allowings the new.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IPropertyBuilder<TProperty, TType> AllowingNew<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
            => builder.WithModelDefault(ModelDefaults.AllowNew, true);

        /// <summary>
        /// Nots the allowing new.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IPropertyBuilder<TProperty, TType> NotAllowingNew<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
            => builder.WithModelDefault(ModelDefaults.AllowNew, false);

        /// <summary>
        /// Allowings the delete.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IPropertyBuilder<TProperty, TType> AllowingDelete<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
            => builder.WithModelDefault(ModelDefaults.AllowDelete, true);

        /// <summary>
        /// Nots the allowing delete.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IPropertyBuilder<TProperty, TType> NotAllowingDelete<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
            => builder.WithModelDefault(ModelDefaults.AllowDelete, false);

        /// <summary>
        /// Allowings the everything.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IPropertyBuilder<TProperty, TType> AllowingEverything<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
          => builder
                .AllowingDelete()
                .AllowingEdit()
                .AllowingNew();

        /// <summary>
        /// Allowings the nothing.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IPropertyBuilder<TProperty, TType> AllowingNothing<TProperty, TType>(this IPropertyBuilder<TProperty, TType> builder)
          => builder
                .NotAllowingDelete()
                .NotAllowingEdit()
                .NotAllowingNew();
    }
}
