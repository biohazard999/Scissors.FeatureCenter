using System;
using System.Linq;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Utils;
using DevExpress.XtraEditors;

namespace Scissors.ExpressApp.LabelEditor.Win.Editors
{
    public class LabelStringPropertyEditor : WinPropertyEditor
    {
        public LabelStringPropertyEditor(Type objectType, IModelMemberViewItem model)
            : base(objectType, model)
                => ControlBindingProperty = nameof(Control.Text);

        protected override object CreateControlCore()
        {
            var control = new LabelControl
            {
                AllowHtmlString = true,
                AutoSizeMode = LabelAutoSizeMode.None,
            };

            control.Appearance.TextOptions.WordWrap = WordWrap.Wrap;

            return control;
        }
        
        public new LabelControl Control => (LabelControl)base.Control;
    }
}
