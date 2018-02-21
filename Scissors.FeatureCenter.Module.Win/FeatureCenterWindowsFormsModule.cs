using System;
using System.Linq;
using DevExpress.ExpressApp;
using Scissors.ExpressApp;
using Scissors.ExpressApp.Win;
using Scissors.FeatureCenter.Modules.LabelEditorDemos;

namespace Scissors.FeatureCenter.Module.Win
{
    public sealed class FeatureCenterWindowsFormsModule : ScissorsBaseModuleWin
    {
        protected override ModuleTypeList GetRequiredModuleTypesCore()
            => base.GetRequiredModuleTypesCore()
                .AndModuleTypes(
                    typeof(FeatureCenterModule),
                    typeof(LabelEditorDemosFeatureCenterWindowsFormsModule)
                );
    }
}