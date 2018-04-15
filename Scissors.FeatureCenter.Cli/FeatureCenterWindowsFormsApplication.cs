using System;
using System.IO;
using System.Linq;
using DevExpress.ExpressApp.Win;

namespace Scissors.FeatureCenter.Win
{
    public partial class FeatureCenterWindowsFormsApplication : WinApplication
    {
        public string PreCompileOutputDirectory => Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), "PreCompile");

        protected override string GetDcAssemblyFilePath()
            => Path.Combine(PreCompileOutputDirectory, DcAssemblyFileName);

        protected override string GetModelAssemblyFilePath()
            => Path.Combine(PreCompileOutputDirectory, ModelAssemblyFileName);

        protected override string GetModelCacheFileLocationPath()
            => PreCompileOutputDirectory;

        protected override string GetModulesVersionInfoFilePath()
           => Path.Combine(PreCompileOutputDirectory, ModulesVersionInfoFileName);
    }
}