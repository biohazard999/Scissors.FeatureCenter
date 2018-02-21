using System;
using System.Linq;
using DevExpress.ExpressApp.Editors;
using Scissors.ExpressApp.Win;
using Scissors.ExpressApp.LabelEditor.Win.Editors;

namespace Scissors.ExpressApp.LabelEditor.Win
{
    public class LabelEditorWindowsFormsModule : ScissorsBaseModuleWin
    {
        protected override void RegisterEditorDescriptors(EditorDescriptorsFactory editorDescriptorsFactory)
            => editorDescriptorsFactory.RegisterLabelStringPropertyEditor();
    }
}
