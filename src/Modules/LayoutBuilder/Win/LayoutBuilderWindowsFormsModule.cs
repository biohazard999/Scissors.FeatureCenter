using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Win.SystemModule;
using Scissors.ExpressApp.Win;

namespace Scissors.ExpressApp.LayoutBuilder.Win
{
    /// <summary>
    /// This module contains the win-forms specific implementations of the LayoutBuilder
    /// </summary>
    public class LayoutBuilderWindowsFormsModule : ScissorsBaseModuleWin
    {
        /// <summary></summary>
        protected override ModuleTypeList GetRequiredModuleTypesCore() => base.GetRequiredModuleTypesCore()
                .AndModuleTypes(typeof(LayoutBuilderModule));
    }
}
