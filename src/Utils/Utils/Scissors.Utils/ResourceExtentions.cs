using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Scissors.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class ResourceExtentions
    {
        /// <summary>
        /// Gets the resource stream.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static Stream GetResourceStream(this object obj, string path)
        {
            Guard.AssertNotNull(obj, nameof(obj));

            return GetResourceStream(obj.GetType(), path);
        }

        /// <summary>
        /// Gets the resource stream.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        /// <exception cref="Scissors.Utils.ResourceNotFoundException"></exception>
        public static Stream GetResourceStream(this Type type, string path)
        {
            Guard.AssertNotNull(type, nameof(type));
            Guard.AssertNotEmpty(path, nameof(path));

            var assembly = type.Assembly;
            var name = type.Assembly.GetName().Name;

            path = path.Replace("/", ".").Replace("\\", ".");

            var fullPath = $"{name}.{path}";
            var stream = assembly.GetManifestResourceStream(fullPath);

            if(stream == null)
            {
                throw new ResourceNotFoundException(assembly, path);
            }

            return stream;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class ResourceNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceNotFoundException"/> class.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="resourceName">Name of the resource.</param>
        public ResourceNotFoundException(Assembly assembly, string resourceName) : base($"Resource '{resourceName}' was not found in Assembly '{assembly.GetName().Name}'")
        {
            Assembly = assembly;
            ResourceName = resourceName;
        }

        /// <summary>
        /// Gets the assembly.
        /// </summary>
        /// <value>
        /// The assembly.
        /// </value>
        public Assembly Assembly { get; }

        /// <summary>
        /// Gets the name of the resource.
        /// </summary>
        /// <value>
        /// The name of the resource.
        /// </value>
        public string ResourceName { get; }

        /// <summary>
        /// Gets the resource path.
        /// </summary>
        /// <value>
        /// The resource path.
        /// </value>
        public string ResourcePath => $"{Assembly.GetName().Name}.{ResourceName}";
    }
}
