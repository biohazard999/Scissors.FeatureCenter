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
    /// <summary>
    /// 
    /// </summary>
    public class AttributeFinder
    {
        /// <summary>
        /// Finds the attribute.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        public class AttributeFinderRequest
        {
            /// <summary>
            /// Gets or sets the assembly path.
            /// </summary>
            /// <value>
            /// The assembly path.
            /// </value>
            public string AssemblyPath { get; set; }
            /// <summary>
            /// Gets or sets the type of the attribute.
            /// </summary>
            /// <value>
            /// The type of the attribute.
            /// </value>
            public Type AttributeType { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        public class AttributeFinderResponse
        {
            /// <summary>
            /// Gets or sets the type of the application.
            /// </summary>
            /// <value>
            /// The type of the application.
            /// </value>
            public string ApplicationType { get; set; }
            /// <summary>
            /// Gets or sets the type of the factory.
            /// </summary>
            /// <value>
            /// The type of the factory.
            /// </value>
            public string FactoryType { get; set; }
        }
    }
}
