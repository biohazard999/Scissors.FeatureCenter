using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.SystemModule;

namespace Scissors.ExpressApp.Win
{
    /// <summary>
    /// Abstract base class for a Scissors win-forms Module
    /// </summary>
    public abstract class ScissorsBaseModuleWin : ScissorsBaseModule
    {
        /// <summary>
        /// Adds the base modules from ScissorsBaseModule and 
        /// adds the DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule to the collection
        /// </summary>
        protected override ModuleTypeList GetRequiredModuleTypesCore()
            => base.GetRequiredModuleTypesCore()
                .AndModuleTypes(typeof(SystemWindowsFormsModule));
    }
}
