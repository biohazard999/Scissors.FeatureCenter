using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Utils.CodeGeneration;
using DevExpress.ExpressApp.Validation;
using DevExpress.ExpressApp.Validation.Win;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Xpo.DB;
using Scissors.ExpressApp.InlineEditForms.Win;
using Squirrel;

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
            if(Tracing.GetFileLocationFromSettings() == DevExpress.Persistent.Base.FileLocation.CurrentUserApplicationDataFolder)
            {
                Tracing.LocalUserAppDataPath = Application.LocalUserAppDataPath;
            }
            Tracing.Initialize();
            DevExpress.ExpressApp.ModelCacheManager.UseMultithreadedLoading = true;
            DevExpress.ExpressApp.ModelCacheManager.SkipEmptyNodes = true;

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

