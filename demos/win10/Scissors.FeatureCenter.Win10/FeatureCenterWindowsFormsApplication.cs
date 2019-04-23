using System;
using System.IO;
using System.Linq;
using DevExpress.ExpressApp.Win;

namespace Scissors.FeatureCenter.Win
{
    public class FeatureCenterWindowsFormsApplication10 : FeatureCenterWindowsFormsApplication
    {
        protected override void OnCustomGetUserModelDifferencesPath(CustomGetUserModelDifferencesPathEventArgs args)
            => args.Path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, ApplicationName);
    }
}
