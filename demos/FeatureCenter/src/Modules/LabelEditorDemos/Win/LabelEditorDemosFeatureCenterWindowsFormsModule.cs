using System;
using DevExpress.ExpressApp;
using Scissors.ExpressApp;
using Scissors.ExpressApp.LabelEditor.Win;
using Scissors.ExpressApp.Win;

namespace Scissors.FeatureCenter.Modules.LabelEditorDemos
{
    public sealed class LabelEditorDemosFeatureCenterWindowsFormsModule : ScissorsBaseModuleWin
    {
        protected override ModuleTypeList GetRequiredModuleTypesCore()
            => base.GetRequiredModuleTypesCore()
                .AndModuleTypes(
                    typeof(LabelEditorDemosFeatureCenterModule),
                    typeof(LabelEditorWindowsFormsModule));
    }
}