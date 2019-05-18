using System;
using System.Diagnostics;
using System.Linq;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;

namespace Scissors.ExpressApp.LabelEditor.Win.Editors
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DevExpress.ExpressApp.Win.Editors.WinPropertyEditor" />
    /// <seealso cref="DevExpress.ExpressApp.Win.Editors.IInplaceEditSupport" />
    public class LabelStringPropertyEditor : WinPropertyEditor, IInplaceEditSupport
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LabelStringPropertyEditor"/> class.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="model">The model.</param>
        public LabelStringPropertyEditor(Type objectType, IModelMemberViewItem model)
            : base(objectType, model)
                => ControlBindingProperty = nameof(Control.Text);

        /// <summary>
        /// Creates the control core.
        /// </summary>
        /// <returns></returns>
        protected override object CreateControlCore()
        {
            var control = new HyperlinkLabelControl
            {
                AllowHtmlString = true,
                AutoSizeMode = LabelAutoSizeMode.None,
            };

            control.HyperlinkClick += Control_HyperlinkClick;

            control.Appearance.TextOptions.WordWrap = WordWrap.Wrap;

            return control;
        }

        private void Control_HyperlinkClick(object sender, HyperlinkClickEventArgs e)
            => Process.Start(e.Link);

        /// <summary>
        /// Unsubscribes from the control's events and, depending on the parameter, also disposes of the control and removes the link to the control.
        /// </summary>
        /// <param name="unwireEventsOnly">true to only unsubscribe from events, false to also dispose of the control and remove the link to the control.</param>
        public override void BreakLinksToControl(bool unwireEventsOnly)
        {
            if(Control != null)
            {
                Control.HyperlinkClick -= Control_HyperlinkClick;
            }

            base.BreakLinksToControl(unwireEventsOnly);
        }

        /// <summary>
        /// Creates a Repository Item corresponding to the editor control used by the Property Editor.
        /// </summary>
        /// <returns>
        /// A <see cref="T:DevExpress.XtraEditors.Repository.RepositoryItem" /> object corresponding to the editor control used by the Property Editor.
        /// </returns>
        public RepositoryItem CreateRepositoryItem()
            => new RepositoryItemHypertextLabel();

        /// <summary>
        /// Provides access to the control that represents the current Property Editor in a UI.
        /// </summary>
        /// <value>
        /// A <see cref="T:System.Windows.Forms.Control" /> object used to display the current Property Editor in a UI.
        /// </value>
        public new HyperlinkLabelControl Control => (HyperlinkLabelControl)base.Control;
    }
}