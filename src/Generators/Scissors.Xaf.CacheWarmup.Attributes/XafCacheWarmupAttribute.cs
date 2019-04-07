using System;

namespace Scissors.Xaf.CacheWarmup.Attributes
{
    /// <summary>
    /// Marks an xaf application type to be caches to be warmed up.
    /// Use on assembly level.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public class XafCacheWarmupAttribute : Attribute
    {
        /// <summary>
        /// The type of the application to warmup those caches
        /// </summary>
        public Type XafApplicationType { get; }

        /// <summary>
        /// The factory type. This factory must have an parameterless constructor and a method CreateApplication() that returns an instance of the application.
        /// </summary>
        public Type XafApplicationFactoryType { get; }

        /// <summary>
        /// Marks an xaf application type to be caches to be warmed up.
        /// For more complex scenarios use the xafApplicationFactoryType parameter to create an instance yourself.
        /// </summary>
        /// <param name="xafApplicationType">The type of the application to warmup those caches</param>
        /// <param name="xafApplicationFactoryType">The factory type. This factory must have an parameterless constructor and a method CreateApplication() that returns an instance of the application.</param>
        public XafCacheWarmupAttribute(Type xafApplicationType, Type xafApplicationFactoryType = null)
        {
            XafApplicationType = xafApplicationType;
            XafApplicationFactoryType = xafApplicationFactoryType;
        }
    }
}
