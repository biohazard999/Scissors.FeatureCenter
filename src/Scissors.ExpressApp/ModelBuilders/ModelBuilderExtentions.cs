using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;

namespace Scissors.ExpressApp.ModelBuilders
{
    /// <summary>
    /// 
    /// </summary>
    public static class ModelBuilderExtentions
    {
        /// <summary>
        /// Determines whether the specified caption has caption.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="caption">The caption.</param>
        /// <returns></returns>
        public static IModelBuilder<T> HasCaption<T>(this IModelBuilder<T> builder, string caption)
            => builder.WithModelDefault(ModelDefaults.Caption, caption);

        /// <summary>
        /// Withes the model default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="modelDefaultPropertyName">Name of the model default property.</param>
        /// <param name="modelDefaultPropertyValue">The model default property value.</param>
        /// <returns></returns>
        public static IModelBuilder<T> WithModelDefault<T>(this IModelBuilder<T> builder, string modelDefaultPropertyName, string modelDefaultPropertyValue)
            => builder.WithAttribute(new ModelDefaultAttribute(modelDefaultPropertyName, modelDefaultPropertyValue));

        /// <summary>
        /// Withes the model default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="modelDefaultPropertyName">Name of the model default property.</param>
        /// <param name="modelDefaultPropertyValue">if set to <c>true</c> [model default property value].</param>
        /// <returns></returns>
        public static IModelBuilder<T> WithModelDefault<T>(this IModelBuilder<T> builder, string modelDefaultPropertyName, bool modelDefaultPropertyValue)
            => builder.WithModelDefault(modelDefaultPropertyName, modelDefaultPropertyValue.ToString());

        /// <summary>
        /// Determines whether the specified image name has image.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="imageName">Name of the image.</param>
        /// <returns></returns>
        public static IModelBuilder<T> HasImage<T>(this IModelBuilder<T> builder, string imageName)
            => builder.WithAttribute(new ImageNameAttribute(imageName));

        /// <summary>
        /// Determines whether [has default property] [the specified property name].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static IModelBuilder<T> HasDefaultProperty<T>(this IModelBuilder<T> builder, string propertyName)
            => builder.WithAttribute(new System.ComponentModel.DefaultPropertyAttribute(propertyName))
                      .WithAttribute(new DevExpress.ExpressApp.DC.XafDefaultPropertyAttribute(propertyName));

        /// <summary>
        /// Determines whether [has default property] [the specified property].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProp">The type of the property.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public static IModelBuilder<T> HasDefaultProperty<T, TProp>(this IModelBuilder<T> builder, Expression<Func<T, TProp>> property)
            => builder.HasDefaultProperty(builder.Exp.Property(property));

        /// <summary>
        /// Determines whether [has object caption format] [the specified format string].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="formatString">The format string.</param>
        /// <returns></returns>
        public static IModelBuilder<T> HasObjectCaptionFormat<T>(this IModelBuilder<T> builder, string formatString)
            => builder.WithAttribute(new ObjectCaptionFormatAttribute(formatString));

        /// <summary>
        /// Determines whether [has object caption format] [the specified property].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProp">The type of the property.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public static IModelBuilder<T> HasObjectCaptionFormat<T, TProp>(this IModelBuilder<T> builder, Expression<Func<T, TProp>> property)
            => builder.HasObjectCaptionFormat($"{{0:{builder.Exp.Property(property)}}}");

        /// <summary>
        /// Nots the allowing edit.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IModelBuilder<T> NotAllowingEdit<T>(this IModelBuilder<T> builder)
            => builder.WithModelDefault(ModelDefaults.AllowEdit, false);

        /// <summary>
        /// Allowings the edit.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IModelBuilder<T> AllowingEdit<T>(this IModelBuilder<T> builder)
            => builder.WithModelDefault(ModelDefaults.AllowEdit, true);

        /// <summary>
        /// Nots the allowing new.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IModelBuilder<T> NotAllowingNew<T>(this IModelBuilder<T> builder)
            => builder.WithModelDefault(ModelDefaults.AllowNew, false);

        /// <summary>
        /// Allowings the new.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IModelBuilder<T> AllowingNew<T>(this IModelBuilder<T> builder)
            => builder.WithModelDefault(ModelDefaults.AllowNew, true);

        /// <summary>
        /// Nots the allowing delete.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IModelBuilder<T> NotAllowingDelete<T>(this IModelBuilder<T> builder)
            => builder.WithModelDefault(ModelDefaults.AllowDelete, false);

        /// <summary>
        /// Allowings the delete.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IModelBuilder<T> AllowingDelete<T>(this IModelBuilder<T> builder)
            => builder.WithModelDefault(ModelDefaults.AllowDelete, true);

        /// <summary>
        /// Allowings the nothing.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IModelBuilder<T> AllowingNothing<T>(this IModelBuilder<T> builder)
            => builder
                .NotAllowingDelete()
                .NotAllowingEdit()
                .NotAllowingNew();

        /// <summary>
        /// Allowings the everything.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IModelBuilder<T> AllowingEverything<T>(this IModelBuilder<T> builder)
            => builder
                .AllowingDelete()
                .AllowingEdit()
                .AllowingNew();

        /// <summary>
        /// Determines whether [is not visible in reports].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IModelBuilder<T> IsNotVisibleInReports<T>(this IModelBuilder<T> builder)
            => builder.WithAttribute(new VisibleInReportsAttribute(false));

        /// <summary>
        /// Determines whether [is visible in reports].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IModelBuilder<T> IsVisibleInReports<T>(this IModelBuilder<T> builder)
            => builder.WithAttribute(new VisibleInReportsAttribute(true));

        /// <summary>
        /// Determines whether [is not visible in dashboards].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IModelBuilder<T> IsNotVisibleInDashboards<T>(this IModelBuilder<T> builder)
            => builder.WithAttribute(new VisibleInDashboardsAttribute(false));

        /// <summary>
        /// Determines whether [is visible in dashboards].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IModelBuilder<T> IsVisibleInDashboards<T>(this IModelBuilder<T> builder)
            => builder.WithAttribute(new VisibleInDashboardsAttribute(true));

        /// <summary>
        /// Withes the default class options.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IModelBuilder<T> WithDefaultClassOptions<T>(this IModelBuilder<T> builder)
            => builder.WithAttribute(new DefaultClassOptionsAttribute());

        /// <summary>
        /// Determines whether [has navigation item] [the specified navigation group name].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="navigationGroupName">Name of the navigation group.</param>
        /// <returns></returns>
        public static IModelBuilder<T> HasNavigationItem<T>(this IModelBuilder<T> builder, string navigationGroupName)
            => builder.WithAttribute(new NavigationItemAttribute(navigationGroupName));
    }

    /// <summary>
    /// Collection if ModelDefault constants
    /// </summary>
    public static class ModelDefaults
    {
        /// <summary>
        /// The caption
        /// </summary>
        public const string Caption = nameof(Caption);
        /// <summary>
        /// The is password
        /// </summary>
        public const string IsPassword = nameof(IsPassword);
        /// <summary>
        /// The tool tip
        /// </summary>
        public const string ToolTip = nameof(ToolTip);
        /// <summary>
        /// The display format
        /// </summary>
        public const string DisplayFormat = nameof(DisplayFormat);
        /// <summary>
        /// The property editor type
        /// </summary>
        public const string PropertyEditorType = nameof(PropertyEditorType);
        /// <summary>
        /// The predefined values
        /// </summary>
        public const string PredefinedValues = nameof(PredefinedValues);
        /// <summary>
        /// The lookup property
        /// </summary>
        public const string LookupProperty = nameof(LookupProperty);

        /// <summary>
        /// The allow edit
        /// </summary>
        public const string AllowEdit = nameof(AllowEdit);
        /// <summary>
        /// The allow new
        /// </summary>
        public const string AllowNew = nameof(AllowNew);
        /// <summary>
        /// The allow delete
        /// </summary>
        public const string AllowDelete = nameof(AllowDelete);
    }
}
