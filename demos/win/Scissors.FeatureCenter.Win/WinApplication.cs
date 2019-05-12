using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Validation;
using DevExpress.ExpressApp.Validation.Win;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Xpo;
using Scissors.ExpressApp.InlineEditForms.Win;
using System;
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
            EnableModelCache = false;
            ExecuteStartupLogicBeforeClosingLogonWindow = true;
        }
        
        static FeatureCenterWindowsFormsApplication()
        {
            DevExpress.ExpressApp.ModelCacheManager.UseMultithreadedLoading = true;
            DevExpress.ExpressApp.ModelCacheManager.SkipEmptyNodes = true;
            DevExpress.ExpressApp.ModelCacheManager.UseCacheWhenDebuggerIsAttached = false;
            DevExpress.Persistent.Base.PasswordCryptographer.EnableRfc2898 = true;
            DevExpress.Persistent.Base.PasswordCryptographer.SupportLegacySha512 = false;
            DevExpress.ExpressApp.Utils.ImageLoader.Instance.UseSvgImages = true;
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

        protected override void LoadUserDifferences()
        {
            if(Debugger.IsAttached)
            {
                return;
            }

            base.LoadUserDifferences();
        }
    }
}
