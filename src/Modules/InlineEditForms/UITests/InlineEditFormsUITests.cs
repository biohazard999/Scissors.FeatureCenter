using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium;
using Xunit;
using Shouldly;
using System.Threading;
using OpenQA.Selenium.Interactions;

namespace Scissors.ExpressApp.InlineEditForms.UITests
{
    public class InlineEditFormsFixture : IDisposable
    {
        public const string windowsApplicationDriverUrl = "http://127.0.0.1:4723";
        private const string featureCenterPath = @"C:\F\git\Scissors.FeatureCenter\Scissors.FeatureCenter.Win\bin\Release\Scissors.FeatureCenter.Win.exe";
        public const string featureCenterAppId = "Scissors.FeatureCenter.Win_ncze720tdpmp2!App";

        public WindowsDriver<WindowsElement> Session { get; private set; }
        public InlineEditFormsFixture()
        {
            var appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", featureCenterAppId);
            Session = new WindowsDriver<WindowsElement>(new Uri(windowsApplicationDriverUrl), appCapabilities);
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

    public class TestTest : IDisposable
    {
        public WindowsDriver<WindowsElement> Session { get; private set; }
        [Fact]
        public void TryToAccess()
        {
            var appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", InlineEditFormsFixture.featureCenterAppId);
            Session = new WindowsDriver<WindowsElement>(new Uri(InlineEditFormsFixture.windowsApplicationDriverUrl), appCapabilities);
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

    [Collection(nameof(InlineEditorFormsCollection))]
    public class InlineEditFormsUITests : IDisposable
    {
        readonly InlineEditFormsFixture _Fixture;

        public InlineEditFormsUITests(InlineEditFormsFixture fixture)
        {
            _Fixture = fixture;
            Setup();
        }

        void Setup()
        {
            _Fixture.Session
                .FindElementsByName("Label Demo Model")
                .FirstOrDefault(m => m.TagName == "ControlType.HyperLink").Click();

            _Fixture.Session
                .FindElementsByName("Neu")
                .FirstOrDefault(m => m.TagName == "ControlType.ToolBar").Click();

            _Fixture.Session.FindElementByAccessibilityId("Html(5)")
                .SendKeys("<b>BOLD<");

            new Actions(_Fixture.Session)
                .KeyDown(Keys.Shift)
                .SendKeys("7")
                .KeyUp(Keys.Shift)
                .Build().Perform();

            _Fixture.Session.FindElementByAccessibilityId("Html(5)")
                .SendKeys("b>");

            new Actions(_Fixture.Session)
                .KeyDown(Keys.Control)
                .SendKeys(Keys.Enter)
                .KeyUp(Keys.Control)
                .Build().Perform();
        }

        public void Dispose()
        {
            _Fixture.Session.FindElementByName("Text row 0")
                  .SendKeys(Keys.Control + "a");

            _Fixture.Session
                .FindElementsByName("Bearbeiten")
                .FirstOrDefault(m => m.TagName == "ControlType.ToolBar")
                .Click();

            _Fixture.Session
                .FindElementByName("&Ja")
                .Click();

            _Fixture.Session
                .FindElementsByName("Schließen")
                .FirstOrDefault(m => m.TagName == "ControlType.ToolBar").Click();
        }

        [Fact]
        public void Success()
        {
            _Fixture.Session.FindElementByName("Text row 0")
                .Text.ShouldBe("BOLD");

            _Fixture.Session.FindElementByName("Text row 0")
                .Click();

            _Fixture.Session.FindElementByName("Text row 0")
                .SendKeys(Keys.F2);

            _Fixture.Session.FindElementByName("Html:")
                .Click();

            _Fixture.Session.FindElementByName("Html:")
                .SendKeys(Keys.Control + "a");

            _Fixture.Session.FindElementByAccessibilityId("Html(5)")
                .SendKeys("THIS IS A TEST");

            _Fixture.Session.FindElementByAccessibilityId("Html(5)")
                .SendKeys(Keys.Control + Keys.Enter);

            _Fixture.Session.FindElementByName("Text row 0")
                .Text.ShouldBe("THIS IS A TEST");
        }

        [Fact]
        public void Cancel()
        {
            _Fixture.Session.FindElementByName("Text row 0")
                .Text.ShouldBe("BOLD");

            _Fixture.Session.FindElementByName("Text row 0")
                .Click();

            _Fixture.Session.FindElementByName("Text row 0")
                .SendKeys(Keys.F2);

            _Fixture.Session.FindElementByName("Html:")
                .Click();

            _Fixture.Session.FindElementByName("Html:")
                .SendKeys(Keys.Control + "a");

            _Fixture.Session.FindElementByAccessibilityId("Html(5)")
                .SendKeys("THIS IS A TEST");

            _Fixture.Session.FindElementByAccessibilityId("Html(5)")
                .SendKeys(Keys.Escape);

            _Fixture.Session.FindElementByName("Text row 0")
                .Text.ShouldBe("BOLD");
        }
    }
}
