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
    public class ObjectSpaceEditFormUserControl : EditFormUserControl
    {
        readonly IObjectSpace _ObjectSpace;
        readonly Frame _Frame;
        readonly DetailView _DetailView;

        public ObjectSpaceEditFormUserControl(IObjectSpace objectSpace, Frame frame, DetailView detailView)
        {
            _ObjectSpace = objectSpace;
            _Frame = frame;
            _DetailView = detailView;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            var controller = _Frame.GetController<WinFocusDefaultDetailViewItemController>();
            var method = controller.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault(m => m.Name == "FocusDefaultItemControl");
            method?.Invoke(controller, null);

            var pnl = Parent.Parent.Controls.OfType<PanelControl>().First();
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
            => _ObjectSpace.CommitChanges();

        private void CancelBtn_Click(object sender, EventArgs e)
            => _ObjectSpace.Rollback();
    }
}
