using System.Collections.Generic;
using DevExpress.ExpressApp.DC;
using FakeItEasy;
using Scissors.ExpressApp.ModelBuilders;
using Xunit;

namespace Scissors.ExpressApp.Tests.ModelBuilders
{
    public class XafBuilderManagerTests
    {
        public class AddBuilderBuild
        {
            [Fact]
            public void CallsRefreshAfterBuild()
            {
                var typesInfo = A.Fake<ITypesInfo>();

                IBuilderManager sut = new XafBuilderManager(typesInfo);
                var builderA = A.Fake<IBuilder>(o => o.Implements<ITypeInfoProvider>());
                var builderB = A.Fake<IBuilder>(o => o.Implements<ITypeInfoProvider>());

                var typeInfoA = A.Fake<ITypeInfo>();
                var typeInfoB = A.Fake<ITypeInfo>();

                A.CallTo(() => ((ITypeInfoProvider)builderA).TypeInfo).Returns(typeInfoA);
                A.CallTo(() => ((ITypeInfoProvider)builderB).TypeInfo).Returns(typeInfoB);

                sut
                    .AddBuilder(builderA)
                    .AddBuilder(builderB)
                    .Build();

                A.CallTo(() => builderA.Build()).MustHaveHappenedOnceExactly()
                    .Then(A.CallTo(() => typesInfo.RefreshInfo(typeInfoA)).MustHaveHappenedOnceExactly())
                    .Then(A.CallTo(() => builderB.Build()).MustHaveHappenedOnceExactly())
                    .Then(A.CallTo(() => typesInfo.RefreshInfo(typeInfoB)).MustHaveHappenedOnceExactly());
            }
        }

        public class AddBuilders
        {
            public class TestXafBuilderManager : XafBuilderManager
            {
                public TestXafBuilderManager(ITypesInfo typesInfo) : base(typesInfo) { }

                public IBuilder Builder = A.Fake<IBuilder>();

                protected override IEnumerable<IBuilder> CreateBuilders() => new[]
                {
                    Builder
                };
            }

            [Fact]
            public void WasCalled()
            {
                var sut = new TestXafBuilderManager(A.Fake<ITypesInfo>());

                sut.Build();

                A.CallTo(() => sut.Builder.Build()).MustHaveHappened();
            }
        }
    }
}
