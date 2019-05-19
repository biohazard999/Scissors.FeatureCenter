using System;
using System.Collections.Generic;
using System.Linq;
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
        /// returns the declared controller types
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<Type> GetDeclaredControllerTypes() => new[]
        {
            typeof(InlineEditFormsGridListViewController)
        };
    }
}
