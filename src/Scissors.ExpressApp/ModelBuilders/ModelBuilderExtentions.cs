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
    public static class ModelBuilderExtentions
    {
        public static IModelBuilder<T> HasCaption<T>(this IModelBuilder<T> builder, string caption)
            => builder.WithModelDefault(ModelDefaults.Caption, caption);
        
        public static IModelBuilder<T> WithModelDefault<T>(this IModelBuilder<T> builder, string modelDefaultPropertyName, string modelDefaultPropertyValue)
            => builder.WithAttribute(new ModelDefaultAttribute(modelDefaultPropertyName, modelDefaultPropertyValue));

        public static IModelBuilder<T> WithModelDefault<T>(this IModelBuilder<T> builder, string modelDefaultPropertyName, bool modelDefaultPropertyValue)
            => builder.WithModelDefault(modelDefaultPropertyName, modelDefaultPropertyValue.ToString());

        public static IModelBuilder<T> HasImage<T>(this IModelBuilder<T> builder, string imageName)
            => builder.WithAttribute(new ImageNameAttribute(imageName));

        public static IModelBuilder<T> HasDefaultProperty<T>(this IModelBuilder<T> builder, string propertyName)
            => builder.WithAttribute(new System.ComponentModel.DefaultPropertyAttribute(propertyName))
                      .WithAttribute(new DevExpress.ExpressApp.DC.XafDefaultPropertyAttribute(propertyName));

        public static IModelBuilder<T> HasDefaultProperty<T, TProp>(this IModelBuilder<T> builder, Expression<Func<T, TProp>> property)
            => builder.HasDefaultProperty(builder.Exp.Property(property));

        public static IModelBuilder<T> HasObjectCaptionFormat<T>(this IModelBuilder<T> builder, string formatString)
            => builder.WithAttribute(new ObjectCaptionFormatAttribute(formatString));
        
        public static IModelBuilder<T> HasObjectCaptionFormat<T, TProp>(this IModelBuilder<T> builder, Expression<Func<T, TProp>> property)
            => builder.HasObjectCaptionFormat($"{{0:{builder.Exp.Property(property)}}}");
        
        public static IModelBuilder<T> NotAllowingEdit<T>(this IModelBuilder<T> builder)
            => builder.WithModelDefault(ModelDefaults.AllowEdit, false);
        
        public static IModelBuilder<T> AllowingEdit<T>(this IModelBuilder<T> builder)
            => builder.WithModelDefault(ModelDefaults.AllowEdit, true);

        public static IModelBuilder<T> NotAllowingNew<T>(this IModelBuilder<T> builder)
            => builder.WithModelDefault(ModelDefaults.AllowNew, false);
         
        public static IModelBuilder<T> AllowingNew<T>(this IModelBuilder<T> builder)
            => builder.WithModelDefault(ModelDefaults.AllowNew, true);

        public static IModelBuilder<T> NotAllowingDelete<T>(this IModelBuilder<T> builder)
            => builder.WithModelDefault(ModelDefaults.AllowDelete, false);

        public static IModelBuilder<T> AllowingDelete<T>(this IModelBuilder<T> builder)
            => builder.WithModelDefault(ModelDefaults.AllowDelete, true);

        public static IModelBuilder<T> AllowingNothing<T>(this IModelBuilder<T> builder)
            => builder
                .NotAllowingDelete()
                .NotAllowingEdit()
                .NotAllowingNew();
        
        public static IModelBuilder<T> AllowingEverything<T>(this IModelBuilder<T> builder)
            => builder
                .AllowingDelete()
                .AllowingEdit()
                .AllowingNew();
        
        public static IModelBuilder<T> IsNotVisibleInReports<T>(this IModelBuilder<T> builder)
            => builder.WithAttribute(new VisibleInReportsAttribute(false));

        public static IModelBuilder<T> IsVisibleInReports<T>(this IModelBuilder<T> builder)
            => builder.WithAttribute(new VisibleInReportsAttribute(true));


        public static IModelBuilder<T> IsNotVisibleInDashboards<T>(this IModelBuilder<T> builder)
            => builder.WithAttribute(new VisibleInDashboardsAttribute(false));

        public static IModelBuilder<T> IsVisibleInDashboards<T>(this IModelBuilder<T> builder)
            => builder.WithAttribute(new VisibleInDashboardsAttribute(true));

        public static IModelBuilder<T> WithDefaultClassOptions<T>(this IModelBuilder<T> builder)
            => builder.WithAttribute(new DefaultClassOptionsAttribute());

        public static IModelBuilder<T> HasNavigationItem<T>(this IModelBuilder<T> builder, string navigationGroupName)
            => builder.WithAttribute(new NavigationItemAttribute(navigationGroupName));
    }

    public static class ModelDefaults
    {
        public const string Caption = nameof(Caption);
        public const string IsPassword = nameof(IsPassword);
        public const string ToolTip = nameof(ToolTip);
        public const string DisplayFormat = nameof(DisplayFormat);
        public const string PropertyEditorType = nameof(PropertyEditorType);
        public const string PredefinedValues = nameof(PredefinedValues);
        public const string LookupProperty = nameof(LookupProperty);

        public const string AllowEdit = nameof(AllowEdit);
        public const string AllowNew = nameof(AllowNew);
        public const string AllowDelete = nameof(AllowDelete);
    }
}
