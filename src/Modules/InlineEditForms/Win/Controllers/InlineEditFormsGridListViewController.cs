using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraGrid.Views.Grid;

namespace Scissors.ExpressApp.InlineEditForms.Win.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ViewController{ListView}" />
    public class InlineEditFormsGridListViewController : ViewController<ListView>
    {
        List<Action> deactivation = new List<Action>();

        /// <summary>
        /// Called when [activated].
        /// </summary>
        protected override void OnActivated()
        {
            base.OnActivated();

            View.ControlsCreated += ControlsCreated;

            deactivation.Add(() => View.ControlsCreated -= ControlsCreated);

            void ControlsCreated(object sender, EventArgs args)
            {
                if(View.Editor is GridListEditor gridListEditor)
                {
                    View.ControlsCreated -= ControlsCreated;

                    gridListEditor.GridView.OptionsBehavior.Editable = true;
                    gridListEditor.GridView.OptionsBehavior.EditingMode = GridEditingMode.EditFormInplace;

                    var dv = Application.CreateDetailView(ObjectSpace, Application.FindDetailViewId(View.ObjectTypeInfo.Type), false, null);
                    var frame = Application.CreateNestedFrame(null, TemplateContext.NestedFrame, dv);
                    frame.CreateTemplate();

                    frame.SetView(dv, true, Frame);
                    dv.CreateControls();

                    ((System.Windows.Forms.Control)dv.Control).Dock = System.Windows.Forms.DockStyle.Fill;
                    ((System.Windows.Forms.Control)dv.Control).Size = new System.Drawing.Size(100, 50);

                    var userControl = new ObjectSpaceEditFormUserControl(ObjectSpace, frame, dv)
                    {
                        Width = ((System.Windows.Forms.Control)dv.Control).PreferredSize.Width,
                        Height = ((System.Windows.Forms.Control)dv.Control).PreferredSize.Height,
                        Controls =
                        {
                            (System.Windows.Forms.Control)dv.Control
                        }
                    };

                    gridListEditor.GridView.OptionsEditForm.CustomEditFormLayout = userControl;
                    gridListEditor.GridView.OptionsEditForm.ActionOnModifiedRowChange = EditFormModifiedAction.Save;
                    gridListEditor.GridView.OptionsEditForm.BindingMode = EditFormBindingMode.Direct;

                    gridListEditor.GridView.EditFormPrepared += EditFormPrepared;

                    deactivation.Add(() => gridListEditor.GridView.EditFormPrepared += EditFormPrepared);

                    void EditFormPrepared(object sender1, EditFormPreparedEventArgs e)
                    {
                        var obj = gridListEditor.GridView.GetRow(e.RowHandle);
                        dv.CurrentObject = obj;
                    }
                }
            }
        }

        /// <summary>
        /// Called when [deactivated].
        /// </summary>
        protected override void OnDeactivated()
        {
            foreach(var action in deactivation)
            {
                action();
            }

            base.OnDeactivated();
        }
    }
}
