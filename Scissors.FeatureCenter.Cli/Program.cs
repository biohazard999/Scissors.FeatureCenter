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
                        Directory.Delete(winApplication.PreCompileOutputDirectory, true);
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
                    var color = Console.ForegroundColor;
                    try
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error:");
                        Console.WriteLine(new string('=', Console.BufferWidth));
                        Console.WriteLine(e.ToString());
                        return 1;
                    }
                    finally
                    {
                        Console.ForegroundColor = color;
                    }
                }
                Console.WriteLine($"Caches created at '{winApplication.PreCompileOutputDirectory}'");
                Console.WriteLine("Caches completed");
                return 0;
            }
        }
    }
}