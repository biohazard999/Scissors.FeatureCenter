using System;
using System.Collections;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Localization;
using DevExpress.ExpressApp.Model;
using Control = Terminal.Gui.View;

namespace Scissors.ExpressApp.Console.Layout
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DevExpress.ExpressApp.Layout.LayoutManager" />
    public class ConsoleLayoutManager : LayoutManager
    {
        private IModelSplitLayout layoutInfo;
        private Control singleControl;
        private Control sidePanel1;
        private Control sidePanel2;
        private Control mainPanel;
        private Hashtable idToPanelMap = new Hashtable();

        /// <summary>
        /// Layouts the controls.
        /// </summary>
        /// <param name="layoutInfo">The layout information.</param>
        /// <param name="repositoryControls">The repository controls.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">
        /// The layoutInfo isn't IModelSplitLayout
        /// or
        /// repositoryControls
        /// </exception>
        public override object LayoutControls(IModelNode layoutInfo, ViewItemsCollection repositoryControls)
        {
            if(!(layoutInfo is IModelSplitLayout))
            {
                throw new ArgumentException("The layoutInfo isn't IModelSplitLayout");
            }
            if(repositoryControls.Count == 2)
            {
                LayoutControls((IModelSplitLayout)layoutInfo, repositoryControls[0], repositoryControls[1]);
            }
            else if(repositoryControls.Count == 1)
            {
                LayoutControls((IModelSplitLayout)layoutInfo, repositoryControls[0], null);
            }
            else
            {
                throw new ArgumentException(SystemExceptionLocalizer.GetExceptionMessage(ExceptionId.LayoutControlInvalidCount, repositoryControls.Count), "repositoryControls");
            }
            return Container;
        }

        /// <summary>
        /// Layouts the controls.
        /// </summary>
        /// <param name="layoutInfo">The layout information.</param>
        /// <param name="firstItem">The first item.</param>
        /// <param name="secondItem">The second item.</param>
        /// <exception cref="ArgumentNullException">firstItem</exception>
        public void LayoutControls(IModelSplitLayout layoutInfo, ViewItem firstItem, ViewItem secondItem)
        {
            if(firstItem == null)
            {
                throw new ArgumentNullException("firstItem");
            }

            if(secondItem != null)
            {
                this.layoutInfo = layoutInfo;
                singleControl = null;
                InitPanels((Control)firstItem.Control, (Control)secondItem.Control);
                FillMap(firstItem, secondItem);
                ApplyModel();
            }
            else
            {
                singleControl = (Control)firstItem.Control;
                //singleControl.Dock = DockStyle.Fill;
            }
        }

        private void InitPanels(Control firstControlToPlace, Control secondControlToPlace)
        {
            mainPanel = new Control();
            sidePanel1 = new Control();
            sidePanel2 = new Control();
            //mainPanel.SuspendLayout();
            //sidePanel1.SuspendLayout();
            //sidePanel2.SuspendLayout();
            //mainPanel.Dock = DockStyle.Fill;
            //mainPanel.MinimumSize = new Size(DefaultMinWidth, DefaultMinWidth);
            //mainPanel.Visible = true;
            //mainPanel.Orientation = IsHorizontal ? Orientation.Horizontal : Orientation.Vertical;
            mainPanel.Add(sidePanel2, sidePanel1);
            //mainPanel.FixedPanel = sidePanel1;
            //mainPanel.FillPanel = sidePanel2;
            //firstControlToPlace.Dock = DockStyle.Fill;
            //secondControlToPlace.Dock = DockStyle.Fill;
            sidePanel1.Add(firstControlToPlace);
            //sidePanel1.Controls.SetChildIndex(firstControlToPlace, 0);
            //sidePanel1.OverlayResizeZoneThickness = 5;
            //sidePanel1.MinimumSize = GetMinSize(new Size(DefaultMinWidth, DefaultMinHeight), firstControlToPlace.MinimumSize);
            sidePanel2.Add(secondControlToPlace);
            //sidePanel2.Controls.SetChildIndex(secondControlToPlace, 0);
            //sidePanel2.OverlayResizeZoneThickness = 5;
            //sidePanel2.MinimumSize = GetMinSize(new Size(DefaultMinWidth, DefaultMinHeight), secondControlToPlace.MinimumSize);
            //Size innerControlsSize = Size.Add(sidePanel1.MinimumSize, sidePanel2.MinimumSize);
            //mainPanel.MinimumSize = GetMinSize(mainPanel.MinimumSize, innerControlsSize);
            //sidePanel1.ResumeLayout();
            //sidePanel2.ResumeLayout();
            //mainPanel.ResumeLayout();
        }
        private void FillMap(ViewItem firstItem, ViewItem secondItem)
        {
            idToPanelMap.Clear();
            idToPanelMap.Add(firstItem.Id, sidePanel1);
            idToPanelMap.Add(secondItem.Id, sidePanel2);
        }

        private void ApplyModel()
        {
            if(layoutInfo != null)
            {
                //mainPanel.FixedPanelWidth = layoutInfo.SplitterPosition;
            }
        }

        /// <summary>
        /// Gets the container core.
        /// </summary>
        /// <returns></returns>
        protected override object GetContainerCore()
        {
            if(singleControl != null)
            {
                return singleControl;
            }
            else
            {
                return mainPanel;
            }
        }
    }
}
