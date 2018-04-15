using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Persistent.Base;

namespace Scissors.FeatureCenter.Win
{
    static partial class Program
    {
        static partial void InitializeTracing()
        {
            Tracing.LogName = Path.Combine(Application.UserAppDataPath, FeatureCenterWindowsFormsApplication.APP_NAME, "logs", "eXpressAppFramework");

            if(!Directory.Exists(Path.GetDirectoryName(Tracing.LogName)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Tracing.LogName));
            }

            Tracing.Initialize();
        }
    }
}