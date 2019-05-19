using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.NodeGenerators;
using Scissors.ExpressApp.LayoutBuilder.Contracts;

namespace Scissors.ExpressApp.LayoutBuilder.GeneratorUpdaters
{
    /// <summary>
    /// 
    /// </summary>
    public class LayoutBuilderGeneratorUpdater : ModelNodesGeneratorUpdater<ModelViewsNodesGenerator>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public override void UpdateNode(ModelNode node)
        {
            foreach(var detailViewNode in ((IModelViews)node).OfType<IModelDetailView>())
            {
                var attribute =detailViewNode.ModelClass.TypeInfo.FindAttributes<DetailViewLayoutBuilderAttribute>()
                    .FirstOrDefault(attr => attr.DetailViewId == detailViewNode.Id);

                if(attribute == null) { continue; }

                UpdateDetailViewLayout(detailViewNode, attribute, detailViewNode.Layout);
            }
        }

        private void UpdateDetailViewLayout(IModelDetailView modelDetailView, DetailViewLayoutBuilderAttribute attribute, IModelViewLayout layoutNode)
        {
            //layoutAttribute.Options(modelDetailView);

            layoutNode.ClearNodes();

            BuildLayout(attribute.Layout, layoutNode, modelDetailView);
        }

        private void BuildLayout(IEnumerable<LayoutItem> items, IModelNode parentNode, IModelDetailView modelDetailView)
        {
            foreach(var item in items)
            {
                var newNode = FactorNode(item, parentNode, modelDetailView);

                BuildLayout(item, newNode, modelDetailView);
            }
        }

        private IModelViewLayoutElement FactorNode(LayoutItem item, IModelNode parentNode, IModelDetailView modelDetailView)
        {
            if(item is VerticalGroup)
            {
                var vGroupNode = parentNode.AddNode<IModelLayoutGroup>(item.Id);
                vGroupNode.Direction = FlowDirection.Vertical;
                return vGroupNode;
            }

            if(item is HorizontalGroup)
            {
                var hGroupNode = parentNode.AddNode<IModelLayoutGroup>(item.Id);
                hGroupNode.Direction = FlowDirection.Horizontal;
                return hGroupNode;
            }

            if(item is TabGroup)
            {
                var tabGroupNode = parentNode.AddNode<IModelTabbedGroup>(item.Id);
                tabGroupNode.Direction = FlowDirection.Horizontal;
                return tabGroupNode;
            }

            if(item is Tab)
            {
                var tabGroupNode = parentNode.AddNode<IModelLayoutGroup>(item.Id);
                tabGroupNode.Direction = FlowDirection.Horizontal;
                return tabGroupNode;
            }

            //if(item is Contracts.Layout.Splitter)
            //{
            //    var splitterNode = parentNode.AddNode<IModelSplitter>(item.Id);

            //    (item as Contracts.Layout.Splitter).Options(splitterNode);

            //    node = splitterNode;
            //}

            //if(item is Contracts.Layout.Seperator)
            //{
            //    var seperatorNode = parentNode.AddNode<IModelSeparator>(item.Id);

            //    (item as Contracts.Layout.Seperator).Options(seperatorNode);

            //    node = seperatorNode;
            //}

            //if(item is Contracts.Layout.LabelItem)
            //{
            //    var labelNode = parentNode.AddNode<IModelLabel>(item.Id);

            //    (item as Contracts.Layout.LabelItem).Options(labelNode);
            //    labelNode.Text = (item as Contracts.Layout.LabelItem).Text;

            //    node = labelNode;
            //}

            if(item is ViewItem)
            {
                var viewItemNode = parentNode.AddNode<IModelLayoutViewItem>(item.Id);
                var editor = item as ViewItem;

                viewItemNode.ViewItem = modelDetailView.Items.FirstOrDefault(m => m.Id == editor.ViewItemId);

                return viewItemNode;
            }

            if(item is EmptySpaceItem)
            {
                var emptySpaceNode = parentNode.AddNode<IModelLayoutViewItem>(item.Id);
                return emptySpaceNode;
            }

            if(item is LayoutGroup)
            {
                var groupNode = parentNode.AddNode<IModelLayoutGroup>(item.Id);
                return groupNode;
            }

            return null;
        }
    }
}
