using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Xpo;
using System;
using System.IO;

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

        //protected override void OnCustomGetUserModelDifferencesPath(CustomGetUserModelDifferencesPathEventArgs args)
        //    => args.Path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, ApplicationName, "Model.User.xafml");

        protected override string GetModelCacheFileLocationPath()
            => Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, ApplicationName);

        static FeatureCenterWindowsFormsApplication()
        {
            DevExpress.Persistent.Base.PasswordCryptographer.EnableRfc2898 = true;
            DevExpress.Persistent.Base.PasswordCryptographer.SupportLegacySha512 = false;
        }

        private void InitializeDefaults()
        {
            UserModelDifferenceFilePath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, ApplicationName);

            LinkNewObjectToParentImmediately = false;
            OptimizedControllersCreation = true;
            UseLightStyle = true;
            EnableModelCache = true;
        }

        public FeatureCenterWindowsFormsApplication()
        {
            InitializeComponent();
            InitializeDefaults();
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
    }
}
