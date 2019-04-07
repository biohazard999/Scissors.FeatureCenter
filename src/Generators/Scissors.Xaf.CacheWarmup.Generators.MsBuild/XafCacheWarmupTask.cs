using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;

namespace Scissors.Xaf.CacheWarmup.Generators.MsBuild
{
    public class XafCacheWarmupTask : Task
    {
        [Required]
        public string ApplicationPath { get; set; }

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
            Console.WriteLine($"ApplicationPath: {ApplicationPath}");

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

            return false;
        }
    }
}
