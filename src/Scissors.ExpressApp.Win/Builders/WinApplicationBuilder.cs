using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Win;
using Scissors.ExpressApp.Builders;

namespace Scissors.ExpressApp.Win.Builders
{
    /// <summary>
    ///
    /// </summary>
    public class WinApplicationBuilder : WinApplicationBuilder<WinApplication, WinApplicationBuilder> { }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TApplication"></typeparam>
    /// <typeparam name="TBuilder"></typeparam>
    public class WinApplicationBuilder<TApplication, TBuilder> : XafApplicationBuilder<TApplication, TBuilder>
        where TApplication : WinApplication
        where TBuilder : WinApplicationBuilder<TApplication, TBuilder>
    {
        /// <summary>
        /// Creates the WinApplication
        /// </summary>
        /// <returns>An instace of an WinApplication</returns>
        protected override TApplication Create() => (TApplication)new WinApplication();

        /// <summary>
        /// Builds up the application
        /// </summary>
        /// <returns>
        /// The application to build
        /// </returns>
        public override TApplication Build()
        {
            var application = base.Build();

            application.SplashScreen =
                SplashScreen
                ?? application.SplashScreen;

            return application;
        }

        /// <summary>
        /// Gets or sets the splash screen.
        /// </summary>
        /// <value>
        /// The splash screen.
        /// </value>
        protected ISplash SplashScreen { get; set; }
        /// <summary>
        /// Withes the splash screen.
        /// </summary>
        /// <param name="splashScreen">The splash screen.</param>
        /// <returns></returns>
        public TBuilder WithSplashScreen(ISplash splashScreen)
        {
            SplashScreen = splashScreen;
            return This;
        }
    }


}
