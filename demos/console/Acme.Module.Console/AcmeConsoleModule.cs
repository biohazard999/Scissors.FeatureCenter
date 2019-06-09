using DevExpress.ExpressApp;
using Scissors.ExpressApp.Console.SystemModule;
using DevExpress.ExpressApp.SystemModule;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acme.Module.ConsoleLib
{
    public class AcmeConsoleModule : ModuleBase
    {
        protected override ModuleTypeList GetRequiredModuleTypesCore()
            => new ModuleTypeList(typeof(SystemModule), typeof(SystemConsoleModule));
    }
}
