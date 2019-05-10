using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using Scissors.ExpressApp.ModelBuilders;
using Shouldly;
using Xunit;

namespace Scissors.ExpressApp.Tests.ModelBuilders
{
    public static class ModelBuilderExtentionsTestsAssertionExtentions
    {
        public static IModelBuilder<T> AssertModelDefaultAttribute<T>(this IModelBuilder<T> builder, string propertyName, string propertyValue)
        {
            var attr = builder.TypeInfo.FindAttributes<ModelDefaultAttribute>().FirstOrDefault(a => a.PropertyName == propertyName);

            attr.ShouldSatisfyAllConditions
            (
                () => attr.ShouldNotBeNull(),
                () => attr.PropertyName.ShouldBe(propertyName),
                () => attr.PropertyValue.ShouldBe(propertyValue)
            );
            return builder;
        }

        public static IModelBuilder<ModelBuilderExtentionsTests> AssertAttribute<TAttribute>(this IModelBuilder<ModelBuilderExtentionsTests> builder, Func<TAttribute, bool> assertion)
               where TAttribute : Attribute
        {
            var attr = builder.TypeInfo.FindAttribute<TAttribute>();

            attr.ShouldSatisfyAllConditions
            (
                () => attr.ShouldNotBeNull(),
                () => assertion.Invoke(attr).ShouldBe(true)
            );
            return builder;
        }
    }

    public class ModelBuilderExtentionsTests
    {
        public string Default { get; set; }

        private IModelBuilder<ModelBuilderExtentionsTests> CreateBuilder() => ModelBuilder.Create<ModelBuilderExtentionsTests>(new TypesInfo());

        public class WithModelDefault : ModelBuilderExtentionsTests
        {
            [Fact]
            public void ShouldHaveStringAttribute() => CreateBuilder()
                .WithModelDefault("Foo", nameof(ShouldHaveStringAttribute))
                .AssertModelDefaultAttribute("Foo", nameof(ShouldHaveStringAttribute));

            [Fact]
            public void ShouldHaveBooleanAttribute() => CreateBuilder()
                .WithModelDefault("Foo", true)
                .AssertModelDefaultAttribute("Foo", "True");
        }

        public class HasCaption : ModelBuilderExtentionsTests
        {
            [Fact]
            public void ShouldHaveCaption() => CreateBuilder()
                .HasCaption(nameof(ShouldHaveCaption))
                .AssertModelDefaultAttribute("Caption", nameof(ShouldHaveCaption));
        }

        public class AllowEdit : ModelBuilderExtentionsTests
        {
            [Fact]
            public void ShouldBeTrue() => CreateBuilder()
                .AllowingEdit()
                .AssertModelDefaultAttribute("AllowEdit", "True");

            [Fact]
            public void ShouldBeFalse() => CreateBuilder()
                .NotAllowingEdit()
                .AssertModelDefaultAttribute("AllowEdit", "False");
        }

        public class AllowNew : ModelBuilderExtentionsTests
        {
            [Fact]
            public void ShouldBeTrue() => CreateBuilder()
                .AllowingNew()
                .AssertModelDefaultAttribute("AllowNew", "True");

            [Fact]
            public void ShouldBeFalse() => CreateBuilder()
                .NotAllowingNew()
                .AssertModelDefaultAttribute("AllowNew", "False");
        }

        public class AllowDelete : ModelBuilderExtentionsTests
        {
            [Fact]
            public void ShouldBeTrue() => CreateBuilder()
                .AllowingDelete()
                .AssertModelDefaultAttribute("AllowDelete", "True");

            [Fact]
            public void ShouldBeFalse() => CreateBuilder()
                .NotAllowingDelete()
                .AssertModelDefaultAttribute("AllowDelete", "False");
        }

        public class AllowEverything : ModelBuilderExtentionsTests
        {
            [Fact]
            public void ShouldBeTrue() => CreateBuilder()
                .AllowingEverything()
                .AssertModelDefaultAttribute("AllowNew", "True")
                .AssertModelDefaultAttribute("AllowEdit", "True")
                .AssertModelDefaultAttribute("AllowDelete", "True");

            [Fact]
            public void ShouldBeFalse() => CreateBuilder()
                .AllowingNothing()
                .AssertModelDefaultAttribute("AllowNew", "False")
                .AssertModelDefaultAttribute("AllowEdit", "False")
                .AssertModelDefaultAttribute("AllowDelete", "False");
        }

        public class VisibleInReports : ModelBuilderExtentionsTests
        {
            [Fact]
            public void ShouldBeTrue() => CreateBuilder()
                .IsVisibleInReports()                
                .AssertAttribute<VisibleInReportsAttribute>(a => a.IsVisible == true);

            [Fact]
            public void ShouldBeFalse() => CreateBuilder()
                .IsNotVisibleInReports()
                .AssertAttribute<VisibleInReportsAttribute>(a => a.IsVisible == false);
        }

        public class VisibleInDashboards : ModelBuilderExtentionsTests
        {
            [Fact]
            public void ShouldBeTrue() => CreateBuilder()
                .IsVisibleInDashboards()
                .AssertAttribute<VisibleInDashboardsAttribute>(a => a.IsVisible == true);

            [Fact]
            public void ShouldBeFalse() => CreateBuilder()
                .IsNotVisibleInDashboards()
                .AssertAttribute<VisibleInDashboardsAttribute>(a => a.IsVisible == false);
        }

        public class DefaultClassOptions : ModelBuilderExtentionsTests
        {
            [Fact]
            public void ShouldNotBeNull() => CreateBuilder()
                .WithDefaultClassOptions()
                .AssertAttribute<DefaultClassOptionsAttribute>(a => a != null);
        }

        public class NavigationItem : ModelBuilderExtentionsTests
        {
            [Fact]
            public void ShouldBeFoo() => CreateBuilder()
                .HasNavigationItem("Foo")
                .AssertAttribute<NavigationItemAttribute>(a => a.GroupName == "Foo");
        }

        public class ImageName : ModelBuilderExtentionsTests
        {
            [Fact]
            public void ShouldBeImg() => CreateBuilder()
                .HasImage("Img")
                .AssertAttribute<ImageNameAttribute>(a => a.ImageName == "Img");
        }

        public class DefaultProperty : ModelBuilderExtentionsTests
        {
            [Fact]
            public void ShouldBeFoo() => CreateBuilder()
                .HasDefaultProperty("Foo")
                .AssertAttribute<System.ComponentModel.DefaultPropertyAttribute>(a => a.Name == "Foo")
                .AssertAttribute<XafDefaultPropertyAttribute>(a => a.Name == "Foo");

            [Fact]
            public void ShouldBeProperty() => CreateBuilder()
                .HasDefaultProperty(p => p.Default)
                .AssertAttribute<System.ComponentModel.DefaultPropertyAttribute>(a => a.Name == nameof(Default))
                .AssertAttribute<XafDefaultPropertyAttribute>(a => a.Name == nameof(Default));
        }

        public class ObjectCaptionFormat : ModelBuilderExtentionsTests
        { 
            [Fact]
            public void ShouldBeFoo() => CreateBuilder()
                .HasObjectCaptionFormat("{0:Foo}")
                .AssertAttribute<ObjectCaptionFormatAttribute>(a => a.FormatString == "{0:Foo}");

            [Fact]
            public void ShouldBeProperty() => CreateBuilder()
                .HasObjectCaptionFormat(p => p.Default)
                .AssertAttribute<ObjectCaptionFormatAttribute>(a => a.FormatString == "{0:Default}");
        }
    }
}
