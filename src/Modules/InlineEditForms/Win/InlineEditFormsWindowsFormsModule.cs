using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp.Editors;
using Scissors.ExpressApp.InlineEditForms.Win.Controllers;
using Scissors.ExpressApp.Win;

namespace Scissors.ExpressApp.InlineEditForms.Win
{
    public class InlineEditFormsWindowsFormsModule : ScissorsBaseModuleWin
    {
        protected override IEnumerable<Type> GetDeclaredControllerTypes()
            => new[]
            {
                typeof(InlineEditFormsGridListViewController)
            };
    }
}
