using System;
using System.IO;
using System.Linq;
using DevExpress.ExpressApp.Win;

namespace Scissors.FeatureCenter.Win
{
    public partial class FeatureCenterWindowsFormsApplication : WinApplication
    {
#if DEBUG
        string OutputDirectory => Path.GetDirectoryName(GetType().Assembly.Location);

        protected override string GetDcAssemblyFilePath()
            => Path.Combine(OutputDirectory, DcAssemblyFileName);

        protected override string GetModelAssemblyFilePath()
            => Path.Combine(OutputDirectory, ModelAssemblyFileName);

        protected override string GetModelCacheFileLocationPath()
            => OutputDirectory;

        protected override string GetModulesVersionInfoFilePath()
           => Path.Combine(OutputDirectory, ModulesVersionInfoFileName);

        protected override void OnCustomGetUserModelDifferencesPath(CustomGetUserModelDifferencesPathEventArgs args)
            => args.Path = null;
#else
       protected override void OnCustomGetUserModelDifferencesPath(CustomGetUserModelDifferencesPathEventArgs args)
            => args.Path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, ApplicationName);
#endif
    }
}
