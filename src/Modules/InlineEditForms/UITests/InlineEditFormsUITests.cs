using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using Scissors.Utils.Testing.XUnit;
using Shouldly;
using System;
using System.Linq;
using System.Threading;
using Xunit;

namespace Scissors.ExpressApp.InlineEditForms.UITests
{
    public class InlineEditFormsFixture : IDisposable
    {
        private const string windowsApplicationDriverUrl = "http://127.0.0.1:4723";

#if DEBUG
        private const string featureCenterPath = @"C:\F\git\Scissors.FeatureCenter\Scissors.FeatureCenter.Win\bin\Debug\Scissors.FeatureCenter.Win.exe";
#else
        private const string featureCenterPath = @"C:\F\git\Scissors.FeatureCenter\Scissors.FeatureCenter.Win\bin\Release\Scissors.FeatureCenter.Win.exe";
#endif

        public WindowsDriver<WindowsElement> Session { get; private set; }
        public InlineEditFormsFixture()
        {
            var appCapabilities = new AppiumOptions();
            appCapabilities.AddAdditionalCapability("app", featureCenterPath);
            Session = new WindowsDriver<WindowsElement>(new Uri(windowsApplicationDriverUrl), appCapabilities);
            Session.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(180);
            Session.ShouldNotBeNull();
            Session.SessionId.ShouldNotBeNull();

            var currentWindowHandle = Session.CurrentWindowHandle;

            // Wait for 5 seconds or however long it is needed for the right window to appear 
            // and for the splash screen to be dismissed. You can replace this with a more intelligent way to
            // determine if the new main window finally appears.
            Thread.Sleep(2000);

            // Return all window handles associated with this process/application.
            // At this point hopefully you have one to pick from. Otherwise you can
            // simply iterate through them to identify the one you want.
            var allWindowHandles = Session.WindowHandles;

            // Assuming you only have only one window entry in allWindowHandles and it is in fact the correct one,
            // switch the session to that window as follows. You can repeat this logic with any top window with the
            // same process id (any entry of allWindowHandles)
            Session.SwitchTo().Window(allWindowHandles[0]);
        }

        public void Dispose()
        {
            Session?.Close();

            Session?.Quit();

            Session?.Dispose();
            Session = null;
        }
    }

    [CollectionDefinition(nameof(InlineEditorFormsCollection))]
    public class InlineEditorFormsCollection : ICollectionFixture<InlineEditFormsFixture>
    {
    }

    [Collection(nameof(InlineEditorFormsCollection))]
    public class InlineEditFormsUITests : IDisposable
    {
        readonly InlineEditFormsFixture fixture;

        public InlineEditFormsUITests(InlineEditFormsFixture fixture)
        {
            this.fixture = fixture;
            Setup();
        }

        void Setup()
        {
            fixture.Session
                .FindElementsByName("Label Demo Model")
                .FirstOrDefault(m => m.TagName == "ControlType.HyperLink").Click();

            fixture.Session
                .FindElementsByName("Neu")
                .FirstOrDefault(m => m.TagName == "ControlType.ToolBar").Click();

            fixture.Session.FindElementByAccessibilityId("Html(5)")
                .SendKeys("<b>BOLD<");

            new Actions(fixture.Session)
                .KeyDown(Keys.Shift)
                .SendKeys("7")
                .KeyUp(Keys.Shift)
                .Build().Perform();

            fixture.Session.FindElementByAccessibilityId("Html(5)")
                .SendKeys("b>");

            new Actions(fixture.Session)
                .KeyDown(Keys.Control)
                .SendKeys(Keys.Enter)
                .KeyUp(Keys.Control)
                .Build().Perform();
        }

        public void Dispose()
        {
            fixture.Session.FindElementByName("Text row 0")
                  .SendKeys(Keys.Control + "a");

            fixture.Session
                .FindElementsByName("Bearbeiten")
                .FirstOrDefault(m => m.TagName == "ControlType.ToolBar")
                .Click();

            fixture.Session
                .FindElementByName("&Ja")
                .Click();

            fixture.Session
                .FindElementsByName("Schließen")
                .FirstOrDefault(m => m.TagName == "ControlType.ToolBar").Click();
        }

        [Fact]
        [UITest]
        public void Success()
        {
            fixture.Session.FindElementByName("Text row 0")
                .Text.ShouldBe("BOLD");

            fixture.Session.FindElementByName("Text row 0")
                .Click();

            fixture.Session.FindElementByName("Text row 0")
                .SendKeys(Keys.F2);

            fixture.Session.FindElementByName("Html:")
                .Click();

            fixture.Session.FindElementByName("Html:")
                .SendKeys(Keys.Control + "a");

            fixture.Session.FindElementByAccessibilityId("Html(5)")
                .SendKeys("THIS IS A TEST");

            fixture.Session.FindElementByAccessibilityId("Html(5)")
                .SendKeys(Keys.Control + Keys.Enter);

            fixture.Session.FindElementByName("Text row 0")
                .Text.ShouldBe("THIS IS A TEST");
        }

        [Fact]
        [UITest]
        public void Cancel()
        {
            fixture.Session.FindElementByName("Text row 0")
                .Text.ShouldBe("BOLD");

            fixture.Session.FindElementByName("Text row 0")
                .Click();

            fixture.Session.FindElementByName("Text row 0")
                .SendKeys(Keys.F2);

            fixture.Session.FindElementByName("Html:")
                .Click();

            fixture.Session.FindElementByName("Html:")
                .SendKeys(Keys.Control + "a");

            fixture.Session.FindElementByAccessibilityId("Html(5)")
                .SendKeys("THIS IS A TEST");

            fixture.Session.FindElementByAccessibilityId("Html(5)")
                .SendKeys(Keys.Escape);

            fixture.Session.FindElementByName("Text row 0")
                .Text.ShouldBe("BOLD");
        }
    }
}
