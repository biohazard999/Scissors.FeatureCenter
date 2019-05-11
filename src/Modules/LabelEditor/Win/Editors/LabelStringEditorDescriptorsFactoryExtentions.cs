using System;
using System.Linq;
using DevExpress.ExpressApp.Editors;
using Scissors.ExpressApp.LabelEditor.Contracts;

namespace Scissors.ExpressApp.LabelEditor.Win.Editors
{
    public static class LabelStringEditorDescriptorsFactoryExtentions
    {
        static LabelStringEditorDescriptorsFactoryExtentions()
            => LabelEditorAliases.Types.LabelStringEditor = typeof(LabelStringPropertyEditor);

        public static EditorDescriptorsFactory RegisterLabelStringPropertyEditor(this EditorDescriptorsFactory editorDescriptorsFactory)
        {
            editorDescriptorsFactory.RegisterPropertyEditorAlias(
                LabelEditorAliases.LabelStringEditor,
                typeof(string),
                true);

            editorDescriptorsFactory
                .RegisterPropertyEditor(LabelEditorAliases.LabelStringEditor,
                typeof(string),
                LabelEditorAliases.Types.LabelStringEditor,
                false);

            return editorDescriptorsFactory;
        }
    }
}
