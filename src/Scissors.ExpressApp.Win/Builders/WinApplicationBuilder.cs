using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model;
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

        public override TApplication Build()
        {
            var application = base.Build();

            application.SplashScreen =
                SplashScreen
                ?? application.SplashScreen;

            return application;
        }

        protected ISplash SplashScreen { get; set; }
        public TBuilder WithSplashScreen(ISplash splashScreen)
        {
            SplashScreen = splashScreen;
            return This;
        }
    }

    /// <summary>
    /// Manages an Headless Winforms XAF application.
    /// Useful for testing or service work.
    /// </summary>
    public class HeadlessWinApplication : WinApplication
    {
        /// <summary>
        /// <para>Creates an instance of the <see cref="HeadlessXafApplication"/> class.</para>
        /// </summary>
        public HeadlessWinApplication() { }

        protected override LayoutManager CreateLayoutManagerCore(bool simple) => null;
        protected override ListEditor CreateListEditorCore(IModelListView modelListView, CollectionSourceBase collectionSource) => null;
    }

    public class HeadlessWinApplicationBuilder : HeadlessWinApplicationBuilder<HeadlessWinApplication, HeadlessWinApplicationBuilder> { }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TApplication"></typeparam>
    /// <typeparam name="TBuilder"></typeparam>
    public class HeadlessWinApplicationBuilder<TApplication, TBuilder> : WinApplicationBuilder<TApplication, TBuilder>
        where TApplication : HeadlessWinApplication
        where TBuilder : HeadlessWinApplicationBuilder<TApplication, TBuilder>
    {
        protected override TApplication Create() => (TApplication)new HeadlessWinApplication();
    }
}
