using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.Editors;
using Scissors.ExpressApp.LabelEditor.Contracts;
using Scissors.ExpressApp.TokenEditor.Win;

namespace Scissors.ExpressApp.LabelEditor.Win
{
    public static class LabelStringEditorDescriptorsFactoryExtentions
    {
        static LabelStringEditorDescriptorsFactoryExtentions()
            => EditorAliasesLabelEditor.Types.LabelStringEditor = typeof(LabelStringPropertyEditor);

        public static EditorDescriptorsFactory RegisterLabelStringPropertyEditor(this EditorDescriptorsFactory editorDescriptorsFactory)
        {
            editorDescriptorsFactory
                .RegisterPropertyEditor(EditorAliasesLabelEditor.LabelStringEditor,
                typeof(string),
                EditorAliasesLabelEditor.Types.LabelStringEditor,
                false);

            return editorDescriptorsFactory;
        }
    }
}
