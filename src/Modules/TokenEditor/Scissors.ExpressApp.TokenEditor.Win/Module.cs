using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Win.SystemModule;

namespace Scissors.TokenEditor.Win
{
    public class TokenEditorWindowsFormsModule : ModuleBase
    {
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB)
            => ModuleUpdater.EmptyModuleUpdaters;

        protected override IEnumerable<Type> GetDeclaredControllerTypes()
            => Type.EmptyTypes;

        protected override IEnumerable<Type> GetDeclaredExportedTypes()
            => Type.EmptyTypes;

        protected override IEnumerable<Type> GetRegularTypes()
            => Type.EmptyTypes;

        protected override ModuleTypeList GetRequiredModuleTypesCore()
            => new ModuleTypeList(
                typeof(SystemModule),
                typeof(SystemWindowsFormsModule)
            );
        
        protected override void RegisterEditorDescriptors(EditorDescriptorsFactory editorDescriptorsFactory)
        {
            base.RegisterEditorDescriptors(editorDescriptorsFactory);
        }
    }
}
