using System;
using System.Linq;
using DevExpress.ExpressApp.Model.NodeGenerators;

namespace Scissors.FeatureCenter.Modules.LabelEditorDemos.Contracts
{
    public static class ViewIds
    {
        public static class LabelDemoModel
        {
            public static readonly string DetailView = ModelNodeIdHelper.GetDetailViewId(typeof(BusinessObjects.LabelDemoModel));
            public static readonly string ListView = ModelNodeIdHelper.GetListViewId(typeof(BusinessObjects.LabelDemoModel));
            public static readonly string LookupListView = ModelNodeIdHelper.GetDetailViewId(typeof(BusinessObjects.LabelDemoModel));
        }
    }
}
