using System;
using System.IO;
using System.Linq;
using DevExpress.ExpressApp.Validation;
using DevExpress.ExpressApp.Validation.Win;
using DevExpress.ExpressApp.Xpo;
using Scissors.ExpressApp.InlineEditForms.Win;

namespace Scissors.FeatureCenter.Win
{
    static class CliProgram
    {
        static int Main(string[] args)
        {
            Console.WriteLine("Generating caches");

            using(var winApplication = new FeatureCenterWindowsFormsApplication())
            {
                try
                {
                    if(Directory.Exists(winApplication.PreCompileOutputDirectory))
                    {
                        try
                        {
                            Directory.Delete(winApplication.PreCompileOutputDirectory, true);
                        }
                        catch(Exception ex)
                        {
                            LogException(ex);
                        }
                    }

                    Directory.CreateDirectory(winApplication.PreCompileOutputDirectory);

                    InMemoryDataStoreProvider.Register();
                    winApplication.ConnectionString = InMemoryDataStoreProvider.ConnectionString;
                    winApplication.SplashScreen = null;

                    winApplication.Modules.Add(new ValidationModule());
                    winApplication.Modules.Add(new ValidationWindowsFormsModule());
                    winApplication.Modules.Add(new InlineEditFormsWindowsFormsModule());
                    winApplication.Setup();

                }
                catch(Exception e)
                {
                    LogException(e);
                    return 1;
                }
                Console.WriteLine($"Caches created at '{winApplication.PreCompileOutputDirectory}'");
                Console.WriteLine("Caches completed");
                return 0;
            }
        }

        private static void LogException(Exception e)
        {
            var color = Console.ForegroundColor;
            try
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error:");
                Console.WriteLine(new string('=', 10));
                Console.WriteLine(e.ToString());
            }
            finally
            {
                Console.ForegroundColor = color;
            }
        }
    }
}