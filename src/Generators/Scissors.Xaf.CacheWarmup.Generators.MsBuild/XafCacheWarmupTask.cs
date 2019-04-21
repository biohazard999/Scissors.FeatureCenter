using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Diagnostics;
using System.IO;

namespace Scissors.Xaf.CacheWarmup.Generators.MsBuild
{
    public class XafCacheWarmupTask : Task
    {
        [Required]
        public string ApplicationPath { get; set; }

        public string CliPath { get; set; }

        public string Mode { get; set; } = "InProcess";

        [Output]
        public string DcAssembly { get; set; }
        [Output]
        public string ModelAssembly { get; set; }
        [Output]
        public string ModelCache { get; set; }
        [Output]
        public string ModulesVersionInfo { get; set; }

        public override bool Execute()
        {
            Log.LogMessage($"ApplicationPath: {ApplicationPath}");
            Log.LogMessage($"Mode: {Mode}");

            if (Mode == "InProcess")
            {
                var finder = new AttributeFinder();
                var assemblyPath = ApplicationPath;
                var foundType = finder.FindAttribute(assemblyPath);

                Console.WriteLine(foundType);

                if (foundType != null)
                {
                    var cacheGenerator = new CacheWarmupGenerator();

                    var cacheResult = cacheGenerator.WarmupCache(assemblyPath, foundType.ApplicationType, foundType.FactoryType);
                    if (cacheResult != null)
                    {
                        DcAssembly = cacheResult.DcAssemblyFilePath;
                        ModelAssembly = cacheResult.ModelAssemblyFilePath;
                        ModelCache = cacheResult.ModelCacheFilePath;
                        ModulesVersionInfo = cacheResult.ModulesVersionInfoFilePath;

                        Console.WriteLine("Done");
                        return true;
                    }
                }
            }

            if (Mode == "OutOfProcess")
            {
                if (string.IsNullOrEmpty(CliPath))
                {
                    CliPath = Path.Combine(Path.GetDirectoryName(ApplicationPath), "Scissors.Xaf.CacheWarmup.Generators.Cli");
                }

                using (var process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo(CliPath, ApplicationPath)
                    {
                        RedirectStandardOutput = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        WorkingDirectory = Path.GetDirectoryName(ApplicationPath)
                    };
                    process.OutputDataReceived += (s, e) => Log.LogMessage(e.Data);
                    if (process.Start())
                    {
                        process.WaitForExit(10000);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
