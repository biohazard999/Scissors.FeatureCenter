using System;
using System.Windows.Forms;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Validation;
using DevExpress.ExpressApp.Validation.Win;
using DevExpress.ExpressApp.Xpo;
using Scissors.ExpressApp.InlineEditForms.Win;

namespace Scissors.FeatureCenter.Win
{
    static partial class Program
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

            InitializeTracing();
            
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

        static partial void InitializeTracing();
    }
}