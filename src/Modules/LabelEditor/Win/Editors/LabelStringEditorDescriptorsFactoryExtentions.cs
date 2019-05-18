using System;
using System.Linq;
using DevExpress.ExpressApp.Editors;
using Scissors.ExpressApp.LabelEditor.Contracts;

namespace Scissors.ExpressApp.LabelEditor.Win.Editors
{
    /// <summary>
    /// 
    /// </summary>
    public static class LabelStringEditorDescriptorsFactoryExtentions
    {
        /// <summary>
        /// Initializes the <see cref="LabelStringEditorDescriptorsFactoryExtentions"/> class.
        /// </summary>
        static LabelStringEditorDescriptorsFactoryExtentions()
            => LabelEditorAliases.Types.LabelStringEditor = typeof(LabelStringPropertyEditor);

        /// <summary>
        /// Registers the label string property editor.
        /// </summary>
        /// <param name="editorDescriptorsFactory">The editor descriptors factory.</param>
        /// <returns></returns>
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
