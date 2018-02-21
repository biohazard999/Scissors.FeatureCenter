using System;
using System.Linq;
using DevExpress.ExpressApp.Editors;
using Scissors.ExpressApp.LabelEditor.Contracts;

namespace Scissors.ExpressApp.LabelEditor.Win.Editors
{
    public static class LabelStringEditorDescriptorsFactoryExtentions
    {
        static LabelStringEditorDescriptorsFactoryExtentions()
            => EditorAliasesLabelEditor.Types.LabelStringEditor = typeof(LabelStringPropertyEditor);

        public static EditorDescriptorsFactory RegisterLabelStringPropertyEditor(this EditorDescriptorsFactory editorDescriptorsFactory)
        {
            editorDescriptorsFactory.RegisterPropertyEditorAlias(
                EditorAliasesLabelEditor.LabelStringEditor,
                typeof(string),
                true);

            editorDescriptorsFactory
                .RegisterPropertyEditor(EditorAliasesLabelEditor.LabelStringEditor,
                typeof(string),
                EditorAliasesLabelEditor.Types.LabelStringEditor,
                false);

            return editorDescriptorsFactory;
        }
    }
}
