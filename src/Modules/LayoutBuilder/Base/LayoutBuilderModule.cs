using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;

namespace Scissors.ExpressApp.LayoutBuilder
{
    public class LayoutBuilderModule : ModuleBase
    {
        protected override ModuleTypeList GetRequiredModuleTypesCore() => new ModuleTypeList(new[]
        {
            typeof(SystemModule)
        });
    }
}
