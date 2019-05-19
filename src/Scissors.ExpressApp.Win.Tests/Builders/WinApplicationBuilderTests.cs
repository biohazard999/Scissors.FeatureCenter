using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Win.Core;
using FakeItEasy;
using Scissors.ExpressApp.Win.Builders;
using Shouldly;
using Xunit;

namespace Scissors.ExpressApp.Win.Tests.Builders
{
    public class WinApplicationBuilderTests
    {
        public class WithSplashScreen
        {
            [Fact]
            public void HasSplashScreen()
            {
                var splash = A.Fake<ISplash>();
                var application = new WinApplicationBuilder()
                    .WithSplashScreen(splash)
                    .Build();

                application.SplashScreen.ShouldBe(splash);
            }

            [Fact]
            public void UsesSplashScreen()
            {
                var splash = A.Fake<ISplash>();
                var application = new WinApplicationBuilder()
                    .WithSplashScreen(splash)
                    .Build();

                application.StartSplash();
                application.StopSplash();

                A.CallTo(() => splash.Start()).MustHaveHappened()
                    .Then(A.CallTo(() => splash.Stop()).MustHaveHappened());

            }
        }
    }
}
