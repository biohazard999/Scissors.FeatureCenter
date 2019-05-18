using System;
using System.Linq;
using System.Reflection;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.SystemModule;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Localization;
using DevExpress.XtraGrid.Views.Grid;

namespace Scissors.ExpressApp.InlineEditForms.Win.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DevExpress.XtraGrid.Views.Grid.EditFormUserControl" />
    public class ObjectSpaceEditFormUserControl : EditFormUserControl
    {
        readonly IObjectSpace objectSpace;
        readonly Frame frame;
        readonly DetailView detailView;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectSpaceEditFormUserControl"/> class.
        /// </summary>
        /// <param name="objectSpace">The object space.</param>
        /// <param name="frame">The frame.</param>
        /// <param name="detailView">The detail view.</param>
        public ObjectSpaceEditFormUserControl(IObjectSpace objectSpace, Frame frame, DetailView detailView)
        {
            this.objectSpace = objectSpace;
            this.frame = frame;
            this.detailView = detailView;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.VisibleChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            var controller = frame.GetController<WinFocusDefaultDetailViewItemController>();
            var method = controller.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault(m => m.Name == "FocusDefaultItemControl");
            method?.Invoke(controller, null);

            if(Parent == null)
            {
                return;
            }

            var pnl = Parent?.Parent?.Controls?.OfType<PanelControl>().FirstOrDefault();

            if(pnl == null)
            {
                return;
            }
                    
            var cancelText = GridLocalizer.Active.GetLocalizedString(GridStringId.EditFormCancelButton);
            var okText = GridLocalizer.Active.GetLocalizedString(GridStringId.EditFormUpdateButton);
            var cancelBtn = pnl.Controls.OfType<SimpleButton>().Where(b => b.Text == cancelText).First();
            var okBtn = pnl.Controls.OfType<SimpleButton>().Where(b => b.Text == okText).First();

            okBtn.Click -= OkBtn_Click;
            okBtn.Click += OkBtn_Click;
            cancelBtn.Click -= CancelBtn_Click;
            cancelBtn.Click += CancelBtn_Click;
        }

        private void OkBtn_Click(object sender, EventArgs e)
            => objectSpace.CommitChanges();

        private void CancelBtn_Click(object sender, EventArgs e)
            => objectSpace.Rollback();
    }
}
