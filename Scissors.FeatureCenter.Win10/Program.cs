using System;
using System.IO;
using System.Linq;
using DevExpress.Persistent.Base;

namespace Scissors.FeatureCenter.Win
{
    static partial class Program
    {
        static partial void InitializeTracing()
        {
            Tracing.LogName = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, FeatureCenterWindowsFormsApplication.APP_NAME, "logs", "eXpressAppFramework");

            if(!Directory.Exists(Path.GetDirectoryName(Tracing.LogName)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Tracing.LogName));
            }

            Tracing.Initialize();
        }
    }
}