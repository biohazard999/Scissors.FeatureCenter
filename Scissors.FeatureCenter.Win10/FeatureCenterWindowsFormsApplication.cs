using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.Win;

namespace Scissors.FeatureCenter.Win
{
    public partial class FeatureCenterWindowsFormsApplication : WinApplication
    {
        protected override string GetDcAssemblyFilePath()
            => Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, ApplicationName, DcAssemblyFileName);

        protected override string GetModelAssemblyFilePath()
            => Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, ApplicationName, ModelAssemblyFileName);

        protected override string GetModulesVersionInfoFilePath()
           => Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, ApplicationName, ModulesVersionInfoFileName);

        protected override string GetModelCacheFileLocationPath()
            => Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, ApplicationName);

        partial void InitUserModelDifferences()
            => UserModelDifferenceFilePath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, ApplicationName);

    }
}
