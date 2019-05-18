using System;
using System.IO;
using System.Linq;
using System.Reflection;
using static System.Console;
using Fasterflect;

namespace Scissors.Xaf.CacheWarmup.Generators
{
    /// <summary>
    /// 
    /// </summary>
    public class CacheWarmupGenerator
    {
        private const string GetDcAssemblyFilePath = "GetDcAssemblyFilePath";
        private const string GetModelAssemblyFilePath = "GetModelAssemblyFilePath";
        private const string GetModelCacheFileLocationPath = "GetModelCacheFileLocationPath";
        private const string GetModulesVersionInfoFilePath = "GetModulesVersionInfoFilePath";

        /// <summary>
        /// Warmups the cache.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="xafApplicationTypeName">Name of the xaf application type.</param>
        /// <param name="xafApplicationFactoryTypeName">Name of the xaf application factory type.</param>
        /// <returns></returns>
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
                    WriteLine($"Set EnableModelCache: 'true'");
                    xafApplication.SetPropertyValue("EnableModelCache", true);

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
                        ModelCacheFilePath = Path.Combine(modelCacheFileLocationPath, "Model.Cache.xafml"),
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

        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        public class CacheWarmupGeneratorRequest
        {
            /// <summary>
            /// Gets or sets the name of the xaf application type.
            /// </summary>
            /// <value>
            /// The name of the xaf application type.
            /// </value>
            public string XafApplicationTypeName { get; set; }
            /// <summary>
            /// Gets or sets the name of the xaf application factory type.
            /// </summary>
            /// <value>
            /// The name of the xaf application factory type.
            /// </value>
            public string XafApplicationFactoryTypeName { get; set; }
            /// <summary>
            /// Gets or sets the assembly path.
            /// </summary>
            /// <value>
            /// The assembly path.
            /// </value>
            public string AssemblyPath { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        public class CacheWarmupGeneratorResponse
        {
            /// <summary>
            /// Gets or sets the dc assembly file path.
            /// </summary>
            /// <value>
            /// The dc assembly file path.
            /// </value>
            public string DcAssemblyFilePath { get; set; }
            /// <summary>
            /// Gets or sets the model assembly file path.
            /// </summary>
            /// <value>
            /// The model assembly file path.
            /// </value>
            public string ModelAssemblyFilePath { get; set; }
            /// <summary>
            /// Gets or sets the model cache file path.
            /// </summary>
            /// <value>
            /// The model cache file path.
            /// </value>
            public string ModelCacheFilePath { get; set; }
            /// <summary>
            /// Gets or sets the modules version information file path.
            /// </summary>
            /// <value>
            /// The modules version information file path.
            /// </value>
            public string ModulesVersionInfoFilePath { get; set; }
        }
    }
}
