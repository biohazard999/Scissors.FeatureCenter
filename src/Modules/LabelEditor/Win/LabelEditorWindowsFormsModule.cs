using System;
using System.Linq;
using DevExpress.ExpressApp.Editors;
using Scissors.ExpressApp.Win;
using Scissors.ExpressApp.LabelEditor.Win.Editors;
using DevExpress.ExpressApp;

namespace Scissors.ExpressApp.LabelEditor.Win
{
    public class LabelEditorWindowsFormsModule : ScissorsBaseModuleWin
    {
        protected override ModuleTypeList GetRequiredModuleTypesCore() => base.GetRequiredModuleTypesCore()
            .AndModuleTypes(typeof(LabelEditorModule));
        
        protected override void RegisterEditorDescriptors(EditorDescriptorsFactory editorDescriptorsFactory)
            => editorDescriptorsFactory.RegisterLabelStringPropertyEditor();
    }
}
