using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.SystemModule;

namespace Scissors.ExpressApp.Win
{
    public abstract class ScissorsBaseModuleWin : ScissorsBaseModule
    {
        protected override ModuleTypeList GetRequiredModuleTypesCore()
            => base.GetRequiredModuleTypesCore()
                .AndModuleTypes(typeof(SystemWindowsFormsModule));
    }
}
