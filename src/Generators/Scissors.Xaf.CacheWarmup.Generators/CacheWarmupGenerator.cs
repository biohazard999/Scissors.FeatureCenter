using System;
using System.IO;
using System.Linq;
using System.Reflection;
using static System.Console;
using Fasterflect;

namespace Scissors.Xaf.CacheWarmup.Generators
{
    public class CacheWarmupGenerator
    {
        private const string GetDcAssemblyFilePath = "GetDcAssemblyFilePath";
        private const string GetModelAssemblyFilePath = "GetModelAssemblyFilePath";
        private const string GetModelCacheFileLocationPath = "GetModelCacheFileLocationPath";
        private const string GetModulesVersionInfoFilePath = "GetModulesVersionInfoFilePath";

        public CacheWarmupGeneratorResponse WarmupCache(Assembly assembly, string xafApplicationTypeName, string xafApplicationFactoryTypeName)
        {
            var applicationType = assembly.GetType(xafApplicationTypeName);

            if (applicationType != null)
            {
                WriteLine($"Found {xafApplicationTypeName} in {assembly.Location}");

                var factory = string.IsNullOrEmpty(xafApplicationFactoryTypeName)
                    ? new Func<System.IDisposable>(() => (System.IDisposable)applicationType.CreateInstance())
                    : new Func<System.IDisposable>(() =>
                    {
                        WriteLine($"Try to find {xafApplicationFactoryTypeName} in {assembly.Location}");
                        var factoryType = assembly.GetType(xafApplicationFactoryTypeName);
                        var f = factoryType.CreateInstance();
                        return (System.IDisposable)f.CallMethod("CreateApplication");
                    });

                WriteLine($"Creating Application");
                using (var xafApplication = factory())
                {
                    WriteLine($"Created Application");
                    WriteLine($"Remove SplashScreen");
                    xafApplication.SetPropertyValue("SplashScreen", null);
                    WriteLine($"Set DatabaseUpdateMode: 'Never'");
                    xafApplication.SetPropertyValue("DatabaseUpdateMode", 0);

                    WriteLine($"Setting up application");
                    WriteLine($"Starting cache warmup");
                    xafApplication.CallMethod("Setup");
                    WriteLine($"Setup application done.");
                    WriteLine($"Wormed up caches.");

                    var dcAssemblyFilePath = (string)xafApplication.CallMethod(GetDcAssemblyFilePath);
                    var modelAssemblyFilePath = (string)xafApplication.CallMethod(GetModelAssemblyFilePath);
                    var modelCacheFileLocationPath = (string)xafApplication.CallMethod(GetModelCacheFileLocationPath);
                    var modulesVersionInfoFilePath = (string)xafApplication.CallMethod(GetModulesVersionInfoFilePath);

                    var cacheResult = new CacheWarmupGeneratorResponse
                    {
                        DcAssemblyFilePath = dcAssemblyFilePath,
                        ModelAssemblyFilePath = modelAssemblyFilePath,
                        ModelCacheFilePath = modelCacheFileLocationPath,
                        ModulesVersionInfoFilePath = modulesVersionInfoFilePath
                    };

                    WriteLine($"{nameof(cacheResult.DcAssemblyFilePath)}: {cacheResult.DcAssemblyFilePath}");
                    WriteLine($"{nameof(cacheResult.ModelAssemblyFilePath)}: {cacheResult.ModelAssemblyFilePath}");
                    WriteLine($"{nameof(cacheResult.ModelCacheFilePath)}: {cacheResult.ModelCacheFilePath}");
                    WriteLine($"{nameof(cacheResult.ModulesVersionInfoFilePath)}: {cacheResult.ModulesVersionInfoFilePath}");

                    return cacheResult;
                }
            }

            return null;
        }

        [Serializable]
        public class CacheWarmupGeneratorRequest
        {
            public string XafApplicationTypeName { get; set; }
            public string XafApplicationFactoryTypeName { get; set; }
            public string AssemblyPath { get; set; }
        }

        [Serializable]
        public class CacheWarmupGeneratorResponse
        {
            public string DcAssemblyFilePath { get; set; }
            public string ModelAssemblyFilePath { get; set; }
            public string ModelCacheFilePath { get; set; }
            public string ModulesVersionInfoFilePath { get; set; }
        }
    }
}
