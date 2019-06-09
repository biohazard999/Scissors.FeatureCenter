using Acme.Module;
using Acme.Module.ConsoleLib;
using Scissors.ExpressApp.Console;
using DevExpress.ExpressApp.Xpo;
using System;
using Terminal.Gui;

namespace Acme.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Acme Inc.";
            Application.Init();

            var top = Application.Top;
            top.Height = Dim.Fill();
            top.Width = Dim.Fill();
            top.X = 0; 
            top.Y = 0;

            Application.Begin(top);

            try
            {
                var application = new ConsoleApplication(top)
                {
                    ApplicationName = Console.Title,
                    Title = Console.Title,
                };

                application.StartSplash();

                application.CreateCustomObjectSpaceProvider += (s, e) =>
                {
                    e.ObjectSpaceProvider = new XPObjectSpaceProvider(e.ConnectionString, null);
                };

                application.DatabaseVersionMismatch += (s, e) =>
                {
                    e.Updater.Update();
                    e.Handled = true;
                };

                application.Modules.Add(new AcmeModule());
                application.Modules.Add(new AcmeConsoleModule());

                InMemoryDataStoreProvider.Register();
                application.ConnectionString = InMemoryDataStoreProvider.ConnectionString;


                application.Setup();

                application.Start();
            }
            finally
            {
                Application.RequestStop();
            }
        }
    }
}
