using AppDomainToolkit;
using Fasterflect;
using System;
using System.IO;
using System.Linq;
using static System.Console;

namespace Scissors.Xaf.CacheWarmup.Generators
{
    public class CacheWarmupGenerator
    {
        private const string GetDcAssemblyFilePath = "GetDcAssemblyFilePath";
        private const string GetModelAssemblyFilePath = "GetModelAssemblyFilePath";
        private const string GetModelCacheFileLocationPath = "GetModelCacheFileLocationPath";
        private const string GetModulesVersionInfoFilePath = "GetModulesVersionInfoFilePath";

        public CacheWarmupGeneratorResponse WarmupCache(string assemblyPath, string xafApplicationTypeName, string xafApplicationFactoryTypeName, Mode mode = Mode.InProcess)
        {
            if (mode == Mode.InProcess)
            {
                using (var context = AppDomainContext.Create(new AppDomainSetup
                {
                    ApplicationBase = Path.GetDirectoryName(typeof(CacheWarmupGenerator).Assembly.Location),
                    PrivateBinPath = Path.GetDirectoryName(assemblyPath),
                    ConfigurationFile = $"{assemblyPath}.config"
                }))
                {
                    context.LoadAssemblyWithReferences(LoadMethod.LoadFile, assemblyPath);
                    return RemoteFunc.Invoke(context.Domain, new CacheWarmupGeneratorRequest
                    {
                        AssemblyPath = assemblyPath,
                        XafApplicationTypeName = xafApplicationTypeName,
                        XafApplicationFactoryTypeName = xafApplicationFactoryTypeName,
                    },
                    (args) =>
                    {
                        WriteLine($"Try to find {args.XafApplicationTypeName} in {args.AssemblyPath}");
                        var assembly = AppDomain.CurrentDomain.GetAssemblies().First(b => b.Location == args.AssemblyPath);
                        var applicationType = assembly.GetType(args.XafApplicationTypeName);

                        if (applicationType != null)
                        {
                            WriteLine($"Found {args.XafApplicationTypeName} in {args.AssemblyPath}");

                            var factory = string.IsNullOrEmpty(args.XafApplicationFactoryTypeName)
                                ? new Func<System.IDisposable>(() => (System.IDisposable)applicationType.CreateInstance())
                                : new Func<System.IDisposable>(() =>
                                {
                                    WriteLine($"Try to find {args.XafApplicationFactoryTypeName} in {args.AssemblyPath}");
                                    var factoryType = assembly.GetType(args.XafApplicationFactoryTypeName);
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
                    });
                }
            }
            else
            {
                var assembly = AppDomain.CurrentDomain.GetAssemblies().First(b => b.Location == assemblyPath);

                var applicationType = assembly.GetType(xafApplicationTypeName);

                if (applicationType != null)
                {
                    WriteLine($"Found {xafApplicationTypeName} in {assemblyPath}");

                    var factory = string.IsNullOrEmpty(xafApplicationFactoryTypeName)
                        ? new Func<System.IDisposable>(() => (System.IDisposable)applicationType.CreateInstance())
                        : new Func<System.IDisposable>(() =>
                        {
                            WriteLine($"Try to find {xafApplicationFactoryTypeName} in {assemblyPath}");
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
