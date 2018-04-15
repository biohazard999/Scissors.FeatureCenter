using System;
using System.IO;
using System.Linq;
using DevExpress.ExpressApp.Win;

namespace Scissors.FeatureCenter.Win
{
    public partial class FeatureCenterWindowsFormsApplication : WinApplication
    {
        protected override string GetDcAssemblyFilePath()
            => Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, ApplicationName, DcAssemblyFileName);

        protected override string GetModelAssemblyFilePath()
            => Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, ApplicationName, ModelAssemblyFileName);

        protected override string GetModelCacheFileLocationPath()
            => Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, ApplicationName);

        protected override string GetModulesVersionInfoFilePath()
           => Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, ApplicationName, ModulesVersionInfoFileName);

        protected override void OnCustomGetUserModelDifferencesPath(CustomGetUserModelDifferencesPathEventArgs args)
            => args.Path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, ApplicationName);
    }
}
