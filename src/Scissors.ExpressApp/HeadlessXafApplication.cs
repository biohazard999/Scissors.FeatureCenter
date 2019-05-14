using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model;

namespace Scissors.ExpressApp
{
    /// <summary>
    /// Manages an Headless XAF application.
    /// Useful for testing or service work.
    /// </summary>
    public class HeadlessXafApplication : XafApplication
    {
        /// <summary>
        /// <para>Creates an instance of the <see cref="HeadlessXafApplication"/> class and initializes the <see cref="DevExpress.ExpressApp.XafApplication.TypesInfo"/> property to a specified value.</para>
        /// </summary>
        /// <param name="typesInfo">An <see cref="DevExpress.ExpressApp.DC.ITypesInfo"/> object which supplies metadata on types used in an XAF application.</param>
        public HeadlessXafApplication(ITypesInfo typesInfo) : base(typesInfo) { }

        /// <summary>
        /// <para>Creates an instance of the <see cref="HeadlessXafApplication"/> class.</para>
        /// </summary>
        public HeadlessXafApplication() { }

        protected override LayoutManager CreateLayoutManagerCore(bool simple) => null;
        protected override ListEditor CreateListEditorCore(IModelListView modelListView, CollectionSourceBase collectionSource) => null;

        private bool isDelayedDetailViewDataLoadingEnabled;
        /// <summary>
        /// Specifies if delayed loading is enabled for Detail Views in WinForms applications.
        /// See DevExpress.ExpressApp.Win.WinApplication.IsDelayedDetailViewDataLoadingEnabled.
        /// </summary>
        public override bool IsDelayedDetailViewDataLoadingEnabled
        {
            get => isDelayedDetailViewDataLoadingEnabled;
            set => isDelayedDetailViewDataLoadingEnabled = value;
        }
    }
}
