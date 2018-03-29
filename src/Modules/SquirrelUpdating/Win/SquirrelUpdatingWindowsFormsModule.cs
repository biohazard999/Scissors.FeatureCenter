using System;
using System.Linq;
using DevExpress.ExpressApp;
using Scissors.ExpressApp.Win;

namespace Scissors.ExpressApp.SquirrelUpdating.Win
{
    public class SquirrelUpdatingWindowsFormsModule : ScissorsBaseModuleWin
    {
        public override void Setup(XafApplication application)
        {
            base.Setup(application);
            application.DatabaseVersionMismatch += Application_DatabaseVersionMismatch;
            application.SetupComplete += Application_SetupComplete;
        }

        private void Application_SetupComplete(object sender, EventArgs e)
        {
        }

        private void Application_DatabaseVersionMismatch(object sender, DatabaseVersionMismatchEventArgs e)
        {
        }
    }
}
