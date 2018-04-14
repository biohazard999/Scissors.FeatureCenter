using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Validation;
using DevExpress.ExpressApp.Validation.Win;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using Scissors.ExpressApp.InlineEditForms.Win;
using System;
using System.IO;
using System.Windows.Forms;

namespace Scissors.FeatureCenter.Win
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

#if EASYTEST
            DevExpress.ExpressApp.Win.EasyTest.EasyTestRemotingRegistration.Register();
#endif

            EditModelPermission.AlwaysGranted = System.Diagnostics.Debugger.IsAttached;
#if WIN10
            Tracing.LocalUserAppDataPath = Path.Combine(Application.UserAppDataPath, FeatureCenterWindowsFormsApplication.APP_NAME);
#endif

            Tracing.Initialize();

            var winApplication = new FeatureCenterWindowsFormsApplication();

            InMemoryDataStoreProvider.Register();
            winApplication.ConnectionString = InMemoryDataStoreProvider.ConnectionString;

#if DEBUG
            if(System.Diagnostics.Debugger.IsAttached && winApplication.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema)
            {
                winApplication.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }
#endif
            try
            {
                winApplication.Modules.Add(new ValidationModule());
                winApplication.Modules.Add(new ValidationWindowsFormsModule());
                winApplication.Modules.Add(new InlineEditFormsWindowsFormsModule());
                winApplication.Setup();
                winApplication.Start();
            }
            catch(Exception e)
            {
                winApplication.HandleException(e);
            }
        }
    }
}

