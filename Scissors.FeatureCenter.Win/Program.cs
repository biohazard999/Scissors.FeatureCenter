using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Validation;
using DevExpress.ExpressApp.Validation.Win;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using Scissors.ExpressApp.InlineEditForms.Win;

namespace Scissors.FeatureCenter.Win
{
    public class Program
    {
        protected virtual void InitializeTracing()
        {
            Tracing.LogName = Path.Combine(Application.UserAppDataPath, FeatureCenterWindowsFormsApplication.APP_NAME, "logs", "eXpressAppFramework");

            if (!Directory.Exists(Path.GetDirectoryName(Tracing.LogName)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Tracing.LogName));
            }

            Tracing.Initialize();
        }

        public virtual FeatureCenterWindowsFormsApplication CreateApplication()
        {
            var winApplication = new FeatureCenterWindowsFormsApplication();
            InMemoryDataStoreProvider.Register();
            winApplication.ConnectionString = InMemoryDataStoreProvider.ConnectionString;
            winApplication.DatabaseUpdateMode = DatabaseUpdateMode.Never;
            return winApplication;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var program = new Program();
            program.Run();
        }

        public void Run()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

#if EASYTEST
            DevExpress.ExpressApp.Win.EasyTest.EasyTestRemotingRegistration.Register();
#endif

            EditModelPermission.AlwaysGranted = System.Diagnostics.Debugger.IsAttached;

            InitializeTracing();

            var winApplication = CreateApplication();

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached && winApplication.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema)
            {
                winApplication.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }
#endif
            try
            {
                winApplication.Setup();
                winApplication.Start();
            }
            catch (Exception e)
            {
                winApplication.HandleException(e);
            }
        }
    }

}