using System;
using System.Linq;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;

namespace Scissors.ExpressApp.LabelEditor.Win.Editors
{
    public class LabelStringPropertyEditor : WinPropertyEditor, IInplaceEditSupport
    {
        public LabelStringPropertyEditor(Type objectType, IModelMemberViewItem model)
            : base(objectType, model)
                => ControlBindingProperty = nameof(Control.Text);

        protected override object CreateControlCore()
        {
            var control = new HyperlinkLabelControl
            {
                AllowHtmlString = true,
                AutoSizeMode = LabelAutoSizeMode.None,
            };

            control.Appearance.TextOptions.WordWrap = WordWrap.Wrap;

            return control;
        }

        public RepositoryItem CreateRepositoryItem()
            => new RepositoryItemHypertextLabel();
        
        public new HyperlinkLabelControl Control => (HyperlinkLabelControl)base.Control;
    }
}
