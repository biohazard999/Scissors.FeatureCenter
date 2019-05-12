using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Validation;
using DevExpress.ExpressApp.Validation.Win;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Xpo;
using Scissors.ExpressApp.InlineEditForms.Win;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Scissors.FeatureCenter.Win
{
    public partial class FeatureCenterWindowsFormsApplication : WinApplication
    {
        public const string APP_NAME = "Scissors.FeatureCenter";
        private void InitializeDefaults()
        {
            ApplicationName = APP_NAME;
            LinkNewObjectToParentImmediately = false;
            OptimizedControllersCreation = true;
            UseLightStyle = true;
            EnableModelCache = true;
            ExecuteStartupLogicBeforeClosingLogonWindow = true;
        }

        static FeatureCenterWindowsFormsApplication()
        {
            DevExpress.ExpressApp.ModelCacheManager.UseMultithreadedLoading = true;
            DevExpress.ExpressApp.ModelCacheManager.SkipEmptyNodes = true;
            DevExpress.ExpressApp.ModelCacheManager.UseCacheWhenDebuggerIsAttached = true;
            DevExpress.Persistent.Base.PasswordCryptographer.EnableRfc2898 = true;
            DevExpress.Persistent.Base.PasswordCryptographer.SupportLegacySha512 = false;
            DevExpress.ExpressApp.Utils.ImageLoader.Instance.UseSvgImages = true;
        }

        public FeatureCenterWindowsFormsApplication()
        {
            InitializeComponent();
            InitializeDefaults();
            CreateCustomModelCacheManager += FeatureCenterWindowsFormsApplication_CreateCustomModelCacheManager;
        }

        private void FeatureCenterWindowsFormsApplication_CreateCustomModelCacheManager(object sender, CreateCustomModelCacheManagerEventArgs e)
        {
            var p = GetModelCacheFileLocationPath();
            var cacheFile = Path.Combine(p, ModelStoreBase.ModelCacheDefaultName + ModelStoreBase.ModelFileExtension);

            if(File.Exists(cacheFile))
            {
                e.ModelCacheManager = new CustomModelCacheManager(                   
                    File.Open(cacheFile, FileMode.Open, FileAccess.Read, FileShare.Read),
                    p
                );
            }
        }


        protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args)
        {
            args.ObjectSpaceProviders.Add(new XPObjectSpaceProvider(XPObjectSpaceProvider.GetDataStoreProvider(args.ConnectionString, args.Connection, true), false));
            args.ObjectSpaceProviders.Add(new NonPersistentObjectSpaceProvider(TypesInfo, null));
        }

        private void FeatureCenterWindowsFormsApplication_CustomizeLanguagesList(object sender, CustomizeLanguagesListEventArgs e)
        {
            var userLanguageName = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
            if(userLanguageName != "en-US" && e.Languages.IndexOf(userLanguageName) == -1)
            {
                e.Languages.Add(userLanguageName);
            }
        }

        private void FeatureCenterWindowsFormsApplication_DatabaseVersionMismatch(object sender, DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs e)
        {
            e.Updater.Update();
            e.Handled = true;
        }

        protected override void LoadUserDifferences()
        {
            if(Debugger.IsAttached)
            {
                return;
            }

            base.LoadUserDifferences();
        }

        protected override string GetDcAssemblyFilePath()
            => GetFilePath2("DcAssembly.dll");

        protected override string GetModelAssemblyFilePath()
            => GetFilePath2("ModelAssembly.dll");

        protected override string GetModulesVersionInfoFilePath()
            => GetFilePath2("ModulesVersionInfo");

        protected string GetFilePath2(string fileName)
        {
            if(!string.IsNullOrEmpty(UserModelDifferenceFilePath))
            {
                return Path.Combine(UserModelDifferenceFilePath, fileName);
            }

            var modelDifferencesPath = GetUserModelDifferencesPath2();
            if(!string.IsNullOrEmpty(modelDifferencesPath))
            {
                return Path.Combine(modelDifferencesPath, fileName);
            }

            return null;
        }

        private string GetUserModelDifferencesPath2()
        {
            var args = new CustomGetUserModelDifferencesPathEventArgs("")
            {
                Path = GetFileLocationPath("UserModelDiffsLocation")
            };
            OnCustomGetUserModelDifferencesPath(args);
            return args.Path;
        }

    }

    public class CustomModelCacheManager : ModelCacheManager
    {
        public CustomModelCacheManager(Stream stream, string modelCacheFileLocationPath) : base(stream, modelCacheFileLocationPath)
        {
        }

        protected override void Save(ModelApplicationBase model)
        {
            //base.Save(model);
        }

        protected override void SaveCore(Dictionary<string, string> serializedModel)
        {
            //base.SaveCore(serializedModel);
        }
    }
}
