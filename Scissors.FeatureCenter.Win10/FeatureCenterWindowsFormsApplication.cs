using System;
using System.IO;
using System.Linq;
using DevExpress.ExpressApp.Win;

namespace Scissors.FeatureCenter.Win
{
    public partial class FeatureCenterWindowsFormsApplication : WinApplication
    {
        string OutputDirectory => Path.GetDirectoryName(GetType().Assembly.Location);

        protected override string GetDcAssemblyFilePath()
            => Path.Combine(OutputDirectory, DcAssemblyFileName);

        protected override string GetModelAssemblyFilePath()
            => Path.Combine(OutputDirectory, ModelAssemblyFileName);
        
        protected override string GetModulesVersionInfoFilePath()
           => Path.Combine(OutputDirectory, ModulesVersionInfoFileName);
#if DEBUG

        protected override string GetModelCacheFileLocationPath()
            => OutputDirectory;

        protected override void OnCustomGetUserModelDifferencesPath(CustomGetUserModelDifferencesPathEventArgs args)
        { 
            args.Path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, ApplicationName, "Debug");
            if(!Directory.Exists(args.Path))
            {
                Directory.CreateDirectory(args.Path);
            }
        }
#else
        protected override string GetModelCacheFileLocationPath()
        {
            var modelCacheFile = Path.Combine(OutputDirectory, "Model.Cache.xafml");
            var destinationFolder = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, ApplicationName);

            if(!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }

            var destinationPath = Path.Combine(destinationFolder, "Model.Cache.xafml");

            if(File.Exists(destinationPath))
            {
                File.Delete(destinationPath);
            }

            if(File.Exists(modelCacheFile))
            {
                File.Copy(modelCacheFile, destinationPath);
            }

            return Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, ApplicationName);
        }         

        protected override void OnCustomGetUserModelDifferencesPath(CustomGetUserModelDifferencesPathEventArgs args)
            => args.Path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, ApplicationName);
#endif
    }
}
