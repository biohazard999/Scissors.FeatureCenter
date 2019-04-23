using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
        public AttributeFinderResponse FindAttribute(Assembly assembly)
        {
            foreach(var attr in assembly.GetCustomAttributes())
            {
                Console.WriteLine($"Attribute in Assembly {assembly.Location}: {attr.GetType()}");
            }

            var attribute = assembly.GetCustomAttributes().Where(a => a.GetType() == typeof(XafCacheWarmupAttribute)).OfType<XafCacheWarmupAttribute>().FirstOrDefault();
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
