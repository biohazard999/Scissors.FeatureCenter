using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DevExpress.ExpressApp.Win;

namespace Scissors.FeatureCenter.Win
{
    public partial class FeatureCenterWindowsFormsApplication : WinApplication
    {
#if DEBUG
        protected override string GetDcAssemblyFilePath()
            => null;

        protected override string GetModelAssemblyFilePath()
            => null;

        protected override string GetModelCacheFileLocationPath()
            => null;

        protected override string GetModulesVersionInfoFilePath()
           => null;
#else
        string OutputDirectory => Path.GetDirectoryName(GetType().Assembly.Location);

        protected override string GetDcAssemblyFilePath()
            => Path.Combine(OutputDirectory, DcAssemblyFileName);

        protected override string GetModelAssemblyFilePath()
            => Path.Combine(OutputDirectory, ModelAssemblyFileName);

        protected override string GetModelCacheFileLocationPath()
            => OutputDirectory;

        protected override string GetModulesVersionInfoFilePath()
           => Path.Combine(OutputDirectory, ModulesVersionInfoFileName);
#endif
        protected override void OnCustomGetUserModelDifferencesPath(CustomGetUserModelDifferencesPathEventArgs args)
            => args.Path = Path.Combine(Application.UserAppDataPath, ApplicationName);
    }
}
