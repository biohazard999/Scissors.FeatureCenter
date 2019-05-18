using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win;

namespace Scissors.ExpressApp.Win
{
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

        /// <summary>
        /// Creates the layout manager core.
        /// </summary>
        /// <param name="simple">if set to <c>true</c> [simple].</param>
        /// <returns></returns>
        protected override LayoutManager CreateLayoutManagerCore(bool simple) => null;

        /// <summary>
        /// Creates the list editor core.
        /// </summary>
        /// <param name="modelListView">The model ListView.</param>
        /// <param name="collectionSource">The collection source.</param>
        /// <returns></returns>
        protected override ListEditor CreateListEditorCore(IModelListView modelListView, CollectionSourceBase collectionSource) => null;
    }
}
