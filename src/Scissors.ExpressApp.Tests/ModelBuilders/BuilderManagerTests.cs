using System;
using System.Linq;
using FakeItEasy;
using Scissors.ExpressApp.ModelBuilders;
using Xunit;

namespace Scissors.ExpressApp.Tests.ModelBuilders
{
    public class BuilderManagerTests
    {
        public class AddBuilderBuild
        {
            [Fact]
            public void DoesCallFirstBuilder()
            {
                IBuilderManager sut = new BuilderManager();
                var builder = A.Fake<IBuilder>();

                sut.AddBuilder(builder)
                    .Build();

                A.CallTo(() => builder.Build()).MustHaveHappenedOnceExactly();
            }

            [Fact]
            public void DoesCallInOrder()
            {
                IBuilderManager sut = new BuilderManager();
                var builderA = A.Fake<IBuilder>();
                var builderB = A.Fake<IBuilder>();

                sut
                    .AddBuilder(builderA)
                    .AddBuilder(builderB)
                    .Build();

                A.CallTo(() => builderA.Build()).MustHaveHappenedOnceExactly()
                    .Then(A.CallTo(() => builderB.Build()).MustHaveHappenedOnceExactly());
            }
        }
    }
}
