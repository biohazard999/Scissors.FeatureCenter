using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.Validation;
using DevExpress.ExpressApp.Validation.Win;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo.DB;
using Scissors.ExpressApp.InlineEditForms.Win;

namespace Scissors.FeatureCenter.Win
{
    class CliProgram
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Generating caches");
            using(var winApplication = new FeatureCenterWindowsFormsApplication())
            {
                InMemoryDataStoreProvider.Register();
                winApplication.ConnectionString = InMemoryDataStoreProvider.ConnectionString;
                winApplication.SplashScreen = null;
                try
                {
                    winApplication.Modules.Add(new ValidationModule());
                    winApplication.Modules.Add(new ValidationWindowsFormsModule());
                    winApplication.Modules.Add(new InlineEditFormsWindowsFormsModule());
                    winApplication.Setup();
                }
                catch(Exception e)
                {
                    winApplication.HandleException(e);
                }
                Console.WriteLine("Caches completed");
            }
        }
    }
}
