using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using Scissors.ExpressApp.InlineEditForms.Win.Controllers;
using Scissors.ExpressApp.Win;

namespace Scissors.ExpressApp.InlineEditForms.Win
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Scissors.ExpressApp.Win.ScissorsBaseModuleWin" />
    public class InlineEditFormsWindowsFormsModule : ScissorsBaseModuleWin
    {
        /// <summary>
        /// Adds the base modules from ScissorsBaseModule and 
        /// adds the DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule to the collection
        /// </summary>
        protected override ModuleTypeList GetRequiredModuleTypesCore()
            => base.GetRequiredModuleTypesCore()
                .AndModuleTypes(typeof(InlineEditFormsModule));

        /// <summary>
        /// returns the declared controller types
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<Type> GetDeclaredControllerTypes() => new[]
        {
            typeof(InlineEditFormsGridListViewController)
        };
    }
}
