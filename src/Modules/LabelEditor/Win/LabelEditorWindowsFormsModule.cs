using System;
using System.Linq;
using DevExpress.ExpressApp.Editors;
using Scissors.ExpressApp.Win;
using Scissors.ExpressApp.LabelEditor.Win.Editors;
using DevExpress.ExpressApp;

namespace Scissors.ExpressApp.LabelEditor.Win
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Scissors.ExpressApp.Win.ScissorsBaseModuleWin" />
    public class LabelEditorWindowsFormsModule : ScissorsBaseModuleWin
    {
        /// <summary>
        /// Adds the base modules from ScissorsBaseModule and
        /// adds the DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule to the collection
        /// </summary>
        /// <returns></returns>
        protected override ModuleTypeList GetRequiredModuleTypesCore() => base.GetRequiredModuleTypesCore()
            .AndModuleTypes(typeof(LabelEditorModule));

        /// <summary>
        /// Registers the editor descriptors.
        /// </summary>
        /// <param name="editorDescriptorsFactory">The editor descriptors factory.</param>
        protected override void RegisterEditorDescriptors(EditorDescriptorsFactory editorDescriptorsFactory)
            => editorDescriptorsFactory.RegisterLabelStringPropertyEditor();
    }
}
