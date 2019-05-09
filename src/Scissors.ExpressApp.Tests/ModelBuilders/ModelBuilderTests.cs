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
        public IList ListThing { get; }

        public class ModelBuilderFactory
        {
            public class Create
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
                    var typesInfo = A.Fake<ITypesInfo>();
                    var typeInfo = A.Fake<ITypeInfo>();
                    A.CallTo(() => typesInfo.FindTypeInfo(typeof(ModelBuilderTests))).Returns(typeInfo);
                    var builder = ModelBuilder.Create<ModelBuilderTests>(typesInfo);
                    builder.TypeInfo.ShouldBe(typeInfo);
                }
            }
        }

        public class Common
        {
            [Fact]
            public void TypeIsCorrect()
            {
                var builder = ModelBuilder.Create<ModelBuilderTests>(A.Fake<ITypesInfo>());
                builder.TargetType.ShouldBe(typeof(ModelBuilderTests));
            }
        }

        public class ViewIds
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
    }
}
