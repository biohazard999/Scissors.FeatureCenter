using System;
using System.Collections;
using System.Linq;
using DevExpress.ExpressApp.DC;
using FakeItEasy;
using Scissors.ExpressApp.ModelBuilders;
using Shouldly;
using Xunit;

namespace Scissors.ExpressApp.Tests.ModelBuilders
{
    public class ModelBuilderTests
    {
        public class EmptyCtorLessAttribute : Attribute
        {
            public string AttributeProperty { get; set; }
        }

        protected (ModelBuilder<ModelBuilderTests>, ITypeInfo) CreateBuilder()
        {
            var typesInfo = A.Fake<ITypesInfo>();
            var typeInfo = A.Fake<ITypeInfo>();
            A.CallTo(() => typesInfo.FindTypeInfo(typeof(ModelBuilderTests))).Returns(typeInfo);
            var builder = ModelBuilder.Create<ModelBuilderTests>(typesInfo);
            return (builder, typeInfo);
        }

        public IList ListThing { get; }

        public class ModelBuilderFactory : ModelBuilderTests
        {
            public class Create : ModelBuilderFactory
            {
                [Fact]
                public void ShouldBeInstanceOfModelBuilder()
                {
                    var builder = ModelBuilder.Create<ModelBuilderTests>(A.Fake<ITypesInfo>());
                    builder.ShouldBeOfType(typeof(ModelBuilder<ModelBuilderTests>));
                }

                [Fact]
                public void TypeInfoIsCorrect()
                {
                    var (builder, typeInfo) = CreateBuilder();
                    builder.TypeInfo.ShouldBe(typeInfo);
                }
            }
        }

        public class Common : ModelBuilderTests
        {
            [Fact]
            public void TypeIsCorrect()
            {
                var builder = ModelBuilder.Create<ModelBuilderTests>(A.Fake<ITypesInfo>());
                builder.TargetType.ShouldBe(typeof(ModelBuilderTests));
            }
        }

        public class ViewIds : ModelBuilderTests
        {
            [Fact]
            public void DefaultDetailView()
            {
                var builder = ModelBuilder.Create<ModelBuilderTests>(A.Fake<ITypesInfo>());
                builder.DefaultDetailView.ShouldBe("ModelBuilderTests_DetailView");
            }

            [Fact]
            public void DefaultListView()
            {
                var builder = ModelBuilder.Create<ModelBuilderTests>(A.Fake<ITypesInfo>());
                builder.DefaultListView.ShouldBe("ModelBuilderTests_ListView");
            }

            [Fact]
            public void DefaultLookupListView()
            {
                var builder = ModelBuilder.Create<ModelBuilderTests>(A.Fake<ITypesInfo>());
                builder.DefaultLookupListView.ShouldBe("ModelBuilderTests_LookupListView");
            }

            [Fact]
            public void NestedListViewId()
            {
                var builder = ModelBuilder.Create<ModelBuilderTests>(A.Fake<ITypesInfo>());
                builder.NestedListViewId(p => p.ListThing).ShouldBe("ModelBuilderTests_ListThing_ListView");
            }
        }

        public class AddAttribute : ModelBuilderTests
        {
            [Fact]
            public void ShouldAdd()
            {
                var (builder, typeInfo) = CreateBuilder();

                builder.WithAttribute<EmptyCtorLessAttribute>();

                A.CallTo(() => typeInfo.AddAttribute(
                    A<Attribute>.That.Matches(
                        a => a.GetType() == typeof(EmptyCtorLessAttribute))
                    )
                ).MustHaveHappened();
            }
        }

        public class RemoveAttribute : ModelBuilderTests
        {
            [Fact]
            public void ShouldRemoveOfT()
            {
                var typesInfo = new TypesInfo();
                var typeInfo = typesInfo.FindTypeInfo(typeof(ModelBuilderTests));
                var builder = ModelBuilder.Create<ModelBuilderTests>(typesInfo);

                builder.WithAttribute<EmptyCtorLessAttribute>();

                typeInfo
                    .FindAttribute<EmptyCtorLessAttribute>()
                    .ShouldNotBeNull();

                builder.RemoveAttribute<EmptyCtorLessAttribute>();

                typeInfo
                    .FindAttribute<EmptyCtorLessAttribute>()
                    .ShouldBeNull();
            }

            [Fact]
            public void ShouldRemove()
            {
                var typesInfo = new TypesInfo();
                var typeInfo = typesInfo.FindTypeInfo(typeof(ModelBuilderTests));
                var builder = ModelBuilder.Create<ModelBuilderTests>(typesInfo);

                builder.WithAttribute<EmptyCtorLessAttribute>();

                typeInfo
                    .FindAttribute<EmptyCtorLessAttribute>()
                    .ShouldNotBeNull();

                builder.RemoveAttribute(typeof(EmptyCtorLessAttribute));

                typeInfo
                    .FindAttribute<EmptyCtorLessAttribute>()
                    .ShouldBeNull();
            }
        }

        public class ConfigureAttribute : ModelBuilderTests
        {
            [Fact]
            public void ShouldConfigure()
            {
                var typesInfo = new TypesInfo();
                var typeInfo = typesInfo.FindTypeInfo(typeof(ModelBuilderTests));
                var builder = ModelBuilder.Create<ModelBuilderTests>(typesInfo);

                builder.WithAttribute<EmptyCtorLessAttribute>();

                builder.ConfigureAttribute<EmptyCtorLessAttribute>(a => a.AttributeProperty = nameof(ConfigureAttribute));
                
                typeInfo
                    .FindAttribute<EmptyCtorLessAttribute>()
                    .AttributeProperty.ShouldBe(nameof(ConfigureAttribute));
            }
        }

        public class For : ModelBuilderTests
        {
            [Fact]
            public void ShouldAddAttribute()
            {
                var (builder, typeInfo) = CreateBuilder();

                var memberInfo = A.Fake<IMemberInfo>();
                A.CallTo(() => typeInfo.FindMember(nameof(ListThing)))
                    .Returns(memberInfo);

                builder.For(m => m.ListThing)
                    .WithAttribute<EmptyCtorLessAttribute>();

                A.CallTo(() =>
                    memberInfo.AddAttribute(
                        A<Attribute>.That.Matches(a =>
                            a.GetType() == typeof(EmptyCtorLessAttribute)
                        )
                )).MustHaveHappenedOnceExactly();
            }

            [Fact]
            public void PropertyNameIsCorrect()
            {
                var (builder, typeInfo) = CreateBuilder();

                var memberInfo = A.Fake<IMemberInfo>();
                A.CallTo(() => typeInfo.FindMember(nameof(ListThing)))
                        .Returns(memberInfo);

                A.CallTo(() => memberInfo.Name).Returns(nameof(ListThing));

                builder.For(m => m.ListThing)
                        .PropertyName.ShouldBe(nameof(ListThing));
            }
        }
    }
}
