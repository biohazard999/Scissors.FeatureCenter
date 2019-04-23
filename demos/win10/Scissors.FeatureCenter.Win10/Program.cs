using System;
using System.IO;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;

namespace Scissors.FeatureCenter.Win
{
    public class ProgramWin10 : Program
    {
        protected override void InitializeTracing()
        {
            Tracing.LogName = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, FeatureCenterWindowsFormsApplication.APP_NAME, "logs", "eXpressAppFramework");

            if (!Directory.Exists(Path.GetDirectoryName(Tracing.LogName)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Tracing.LogName));
            }

            Tracing.Initialize();
        }

        public override FeatureCenterWindowsFormsApplication CreateApplication()
        {
            var winApplication = new FeatureCenterWindowsFormsApplication10();
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
            var program = new ProgramWin10();
            program.Run();
        }
    }
}