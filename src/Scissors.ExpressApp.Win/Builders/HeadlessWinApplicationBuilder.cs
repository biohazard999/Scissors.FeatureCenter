namespace Scissors.ExpressApp.Win.Builders
{

    /// <summary>
    /// Concrete Headless Winforms application builder.
    /// </summary>
    /// <seealso cref="HeadlessWinApplicationBuilder{HeadlessWinApplication, HeadlessWinApplicationBuilder}" />
    public class HeadlessWinApplicationBuilder : HeadlessWinApplicationBuilder<HeadlessWinApplication, HeadlessWinApplicationBuilder> { }

    /// <summary>Abstract Headless Winforms application builder.</summary>
    /// <typeparam name="TApplication">The type of the application.</typeparam>
    /// <typeparam name="TBuilder">The type of the builder.</typeparam>
    public class HeadlessWinApplicationBuilder<TApplication, TBuilder> : WinApplicationBuilder<TApplication, TBuilder>
        where TApplication : HeadlessWinApplication
        where TBuilder : HeadlessWinApplicationBuilder<TApplication, TBuilder>
    {
        /// <summary>
        /// Creates the HeadlessWinApplication
        /// </summary>
        /// <returns>
        /// An instance of an HeadlessWinApplication
        /// </returns>
        protected override TApplication Create() => (TApplication)new HeadlessWinApplication();
    }
}
