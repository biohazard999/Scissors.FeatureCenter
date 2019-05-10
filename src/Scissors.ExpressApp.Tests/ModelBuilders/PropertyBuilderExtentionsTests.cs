using System;
using System.Linq;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using Scissors.ExpressApp.ModelBuilders;
using Shouldly;
using Xunit;

namespace Scissors.ExpressApp.Tests.ModelBuilders
{
    public static class PropertyBuilderExtentionsTestsAssertionExtentions
    {
        public static IPropertyBuilder<TProperty, TClass> AssertModelDefaultAttribute<TProperty, TClass>(this IPropertyBuilder<TProperty, TClass> builder, string propertyName, string propertyValue)
        {
            var attr = builder.MemberInfo.FindAttributes<ModelDefaultAttribute>().FirstOrDefault(a => a.PropertyName == propertyName);

            attr.ShouldSatisfyAllConditions
            (
                () => attr.ShouldNotBeNull(),
                () => attr.PropertyName.ShouldBe(propertyName),
                () => attr.PropertyValue.ShouldBe(propertyValue)
            );
            return builder;
        }

        public static IPropertyBuilder<TProperty, PropertyBuilderExtentionsTests> AssertAttribute<TAttribute, TProperty>(this IPropertyBuilder<TProperty, PropertyBuilderExtentionsTests> builder, Func<TAttribute, bool> assertion)
               where TAttribute : Attribute
        {
            var attr = builder.MemberInfo.FindAttribute<TAttribute>();

            attr.ShouldSatisfyAllConditions
            (
                () => attr.ShouldNotBeNull(),
                () => assertion.Invoke(attr).ShouldBe(true)
            );
            return builder;
        }
    }

    public class PropertyBuilderExtentionsTests
    {
        public string AProperty { get; set; }

        private IModelBuilder<PropertyBuilderExtentionsTests> CreateBuilder() => ModelBuilder.Create<PropertyBuilderExtentionsTests>(new TypesInfo());

        public class ModelDefaultAttribute : PropertyBuilderExtentionsTests
        {
            [Fact]
            public void ShouldBeFoo() => CreateBuilder()
                .For(p => p.AProperty)
                .WithModelDefault("Foo", "Bar")
                .AssertModelDefaultAttribute("Foo", "Bar");

            [Fact]
            public void ShouldBeTrue() => CreateBuilder()
                .For(p => p.AProperty)
                .WithModelDefault("Foo", true)
                .AssertModelDefaultAttribute("Foo", "True");
        }

        public class HasCaption : PropertyBuilderExtentionsTests
        {
            [Fact]
            public void ShouldBeBar() => CreateBuilder()
                .For(p => p.AProperty)
                .HasCaption("Bar")
                .AssertModelDefaultAttribute("Caption", "Bar");
        }

        public class IsPassword : PropertyBuilderExtentionsTests
        {
            [Fact]
            public void ShouldBeTrue() => CreateBuilder()
                .For(p => p.AProperty)
                .IsPassword()
                .AssertModelDefaultAttribute("IsPassword", "True");
        }

        public class WithPredefinedValues : PropertyBuilderExtentionsTests
        {
            [Fact]
            public void ShouldUseString() => CreateBuilder()
                .For(p => p.AProperty)
                .WithPredefinedValues("Foo;Bar")
                .AssertModelDefaultAttribute("PredefinedValues", "Foo;Bar");

            [Fact]
            public void ShouldUseListString() => CreateBuilder()
                .For(p => p.AProperty)
                .WithPredefinedValues(new[] { "Foo", "Bar", "Baz" })
                .AssertModelDefaultAttribute("PredefinedValues", "Foo;Bar;Baz");
        }

        public class HasTooltip : PropertyBuilderExtentionsTests
        {
            [Fact]
            public void ShouldHaveTooltip() => CreateBuilder()
                .For(p => p.AProperty)
                .HasTooltip("This is a tooltip")
                .AssertModelDefaultAttribute("ToolTip", "This is a tooltip");
        }

        public class HasDisplayFormat : PropertyBuilderExtentionsTests
        {
            [Fact]
            public void ShouldHaveDisplayFormat() => CreateBuilder()
                .For(p => p.AProperty)
                .HasDisplayFormat("{0:Foo}")
                .AssertModelDefaultAttribute("DisplayFormat", "{0:Foo}");
        }

        public class UsingPropertyEditor : PropertyBuilderExtentionsTests
        {
            public class FooEditor : PropertyEditor
            {
                public FooEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model) { }
                protected override object CreateControlCore() => throw new NotImplementedException();
                protected override object GetControlValueCore() => throw new NotImplementedException();
                protected override void ReadValueCore() => throw new NotImplementedException();

            }

            [Fact]
            public void ShouldHaveEditor() => CreateBuilder()
                .For(p => p.AProperty)
                .UsingPropertyEditor(typeof(UsingPropertyEditor))
                .AssertModelDefaultAttribute("PropertyEditorType", typeof(UsingPropertyEditor).FullName);

            [Fact]
            public void ShouldHaveEditorString() => CreateBuilder()
                .For(p => p.AProperty)
                .UsingPropertyEditor(typeof(UsingPropertyEditor).FullName)
                .AssertModelDefaultAttribute("PropertyEditorType", typeof(UsingPropertyEditor).FullName);
        }

        public class HasIndex : PropertyBuilderExtentionsTests
        {
            [Fact]
            public void ShouldHaveIndex1() => CreateBuilder()
                .For(p => p.AProperty)
                .HasIndex(1)
                .AssertAttribute<IndexAttribute, string>(a => a.Index == 1);
        }

        public class IsVisibleInDetailView : PropertyBuilderExtentionsTests
        {
            [Fact]
            public void ShouldBeTrue() => CreateBuilder()
                .For(p => p.AProperty)
                .IsVisibleInDetailView()
                .AssertAttribute<VisibleInDetailViewAttribute, string>(a => ((bool)a.Value) == true);

            [Fact]
            public void ShouldBeFalse() => CreateBuilder()
                .For(p => p.AProperty)
                .IsNotVisibleInDetailView()
                .AssertAttribute<VisibleInDetailViewAttribute, string>(a => ((bool)a.Value) == false);
        }

        public class IsVisibleInListView : PropertyBuilderExtentionsTests
        {
            [Fact]
            public void ShouldBeTrue() => CreateBuilder()
                .For(p => p.AProperty)
                .IsVisibleInListView()
                .AssertAttribute<VisibleInListViewAttribute, string>(a => ((bool)a.Value) == true);

            [Fact]
            public void ShouldBeFalse() => CreateBuilder()
                .For(p => p.AProperty)
                .IsNotVisibleInListView()
                .AssertAttribute<VisibleInListViewAttribute, string>(a => ((bool)a.Value) == false);
        }

        public class IsVisibleInLookupListView : PropertyBuilderExtentionsTests
        {
            [Fact]
            public void ShouldBeTrue() => CreateBuilder()
                .For(p => p.AProperty)
                .IsVisibleInLookupListView()
                .AssertAttribute<VisibleInLookupListViewAttribute, string>(a => ((bool)a.Value) == true);

            [Fact]
            public void ShouldBeFalse() => CreateBuilder()
                .For(p => p.AProperty)
                .IsNotVisibleInLookupListView()
                .AssertAttribute<VisibleInLookupListViewAttribute, string>(a => ((bool)a.Value) == false);
        }

        public class IsVisibleInAnyView : PropertyBuilderExtentionsTests
        {
            [Fact]
            public void ShouldBeTrue() => CreateBuilder()
                .For(p => p.AProperty)
                .IsVisibleInAnyView()
                .AssertAttribute<VisibleInLookupListViewAttribute, string>(a => ((bool)a.Value) == true)
                .AssertAttribute<VisibleInDetailViewAttribute, string>(a => ((bool)a.Value) == true)
                .AssertAttribute<VisibleInListViewAttribute, string>(a => ((bool)a.Value) == true);

            [Fact]
            public void ShouldBeFalse() => CreateBuilder()
                .For(p => p.AProperty)
                .IsNotVisibleInAnyView()
                .AssertAttribute<VisibleInLookupListViewAttribute, string>(a => ((bool)a.Value) == false)
                .AssertAttribute<VisibleInDetailViewAttribute, string>(a => ((bool)a.Value) == false)
                .AssertAttribute<VisibleInListViewAttribute, string>(a => ((bool)a.Value) == false);
        }

        public class UsingEditorAlias : PropertyBuilderExtentionsTests
        {
            [Fact]
            public void ShouldHaveEditorAlias() => CreateBuilder()
                .For(p => p.AProperty)
                .UsingEditorAlias("EditorAlias")
                .AssertAttribute<EditorAliasAttribute, string>(a => a.Alias == "EditorAlias");
        }

        public class ImmediatePostsData : PropertyBuilderExtentionsTests
        {
            [Fact]
            public void ShouldNotBeNull() => CreateBuilder()
                .For(p => p.AProperty)
                .ImmediatePostsData()
                .AssertAttribute<ImmediatePostDataAttribute, string>(a => a != null);
        }

        public class AllowEdit : PropertyBuilderExtentionsTests
        {
            [Fact]
            public void ShouldBeTrue() => CreateBuilder()
                .For(p => p.AProperty)
                .AllowingEdit()
                .AssertModelDefaultAttribute("AllowEdit", "True");

            [Fact]
            public void ShouldBeFalse() => CreateBuilder()
                .For(p => p.AProperty)
                .NotAllowingEdit()
                .AssertModelDefaultAttribute("AllowEdit", "False");
        }

        public class AllowNew : PropertyBuilderExtentionsTests
        {
            [Fact]
            public void ShouldBeTrue() => CreateBuilder()
                .For(p => p.AProperty)
                .AllowingNew()
                .AssertModelDefaultAttribute("AllowNew", "True");

            [Fact]
            public void ShouldBeFalse() => CreateBuilder()
                .For(p => p.AProperty)
                .NotAllowingNew()
                .AssertModelDefaultAttribute("AllowNew", "False");
        }

        public class AllowDelete : PropertyBuilderExtentionsTests
        {
            [Fact]
            public void ShouldBeTrue() => CreateBuilder()
                .For(p => p.AProperty)
                .AllowingDelete()
                .AssertModelDefaultAttribute("AllowDelete", "True");

            [Fact]
            public void ShouldBeFalse() => CreateBuilder()
                .For(p => p.AProperty)
                .NotAllowingDelete()
                .AssertModelDefaultAttribute("AllowDelete", "False");
        }

        public class AllowEverything : PropertyBuilderExtentionsTests
        {
            [Fact]
            public void ShouldBeTrue() => CreateBuilder()
                .For(p => p.AProperty)
                .AllowingEverything()
                .AssertModelDefaultAttribute("AllowNew", "True")
                .AssertModelDefaultAttribute("AllowEdit", "True")
                .AssertModelDefaultAttribute("AllowDelete", "True");

            [Fact]
            public void ShouldBeFalse() => CreateBuilder()
                .For(p => p.AProperty)
                .AllowingNothing()
                .AssertModelDefaultAttribute("AllowNew", "False")
                .AssertModelDefaultAttribute("AllowEdit", "False")
                .AssertModelDefaultAttribute("AllowDelete", "False");
        }
    }
}
