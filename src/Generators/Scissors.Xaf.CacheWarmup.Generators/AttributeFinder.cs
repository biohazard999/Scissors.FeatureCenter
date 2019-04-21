using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AppDomainToolkit;
using Scissors.Xaf.CacheWarmup.Attributes;
using static System.Console;

namespace Scissors.Xaf.CacheWarmup.Generators
{
    public enum Mode
    {
        InProcess,
        OutOfProcess
    }

    public class AttributeFinder
    {
        public AttributeFinderResponse FindAttribute(string assemblyPath, Mode mode = Mode.InProcess)
        {
            if(mode == Mode.InProcess)
            {
                using (var context = AppDomainContext.Create(new AppDomainSetup
                {
                    ApplicationBase = Path.GetDirectoryName(typeof(AttributeFinder).Assembly.Location),
                    PrivateBinPath = Path.GetDirectoryName(assemblyPath),
                }))
                {
                    context.LoadAssemblyWithReferences(LoadMethod.LoadFile, assemblyPath);
                    return RemoteFunc.Invoke(context.Domain, new AttributeFinderRequest { AssemblyPath = assemblyPath, AttributeType = typeof(XafCacheWarmupAttribute) }, (args) =>
                    {
                        WriteLine($"Try to find {nameof(XafCacheWarmupAttribute)} in {args.AssemblyPath}");
                        var assembly = AppDomain.CurrentDomain.GetAssemblies().First(b => b.Location == args.AssemblyPath);

                        var attribute = assembly.GetCustomAttributes(false).OfType<XafCacheWarmupAttribute>().FirstOrDefault();
                        if (attribute != null)
                        {
                            WriteLine($"Found {nameof(XafCacheWarmupAttribute)} with '{attribute.XafApplicationType.FullName}'");

                            return new AttributeFinderResponse
                            {
                                ApplicationType = attribute.XafApplicationType.FullName,
                                FactoryType = attribute.XafApplicationFactoryType?.FullName
                            };
                        }
                        return null;
                    });
                }
            }
            else
            {
                var assembly = Assembly.LoadFile(assemblyPath);

                var attribute = assembly.GetCustomAttributes(false).OfType<XafCacheWarmupAttribute>().FirstOrDefault();
                if (attribute != null)
                {
                    WriteLine($"Found {nameof(XafCacheWarmupAttribute)} with '{attribute.XafApplicationType.FullName}'");

                    return new AttributeFinderResponse
                    {
                        ApplicationType = attribute.XafApplicationType.FullName,
                        FactoryType = attribute.XafApplicationFactoryType?.FullName
                    };
                }
            }
            return null;
        }

        [Serializable]
        public class AttributeFinderRequest
        {
            public string AssemblyPath { get; set; }
            public Type AttributeType { get; set; }
        }

        [Serializable]
        public class AttributeFinderResponse
        {
            public string ApplicationType { get; set; }
            public string FactoryType { get; set; }
        }
    }
}
