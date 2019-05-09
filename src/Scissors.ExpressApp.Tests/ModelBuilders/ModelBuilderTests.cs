using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.DC;
using FakeItEasy;
using Scissors.ExpressApp.ModelBuilders;
using Shouldly;
using Xunit;

namespace Scissors.ExpressApp.Tests.ModelBuilders
{
    public class ModelBuilderTests
    {
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
            public class EmptyCtorLessAttribute : Attribute {  }

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
    }
}
