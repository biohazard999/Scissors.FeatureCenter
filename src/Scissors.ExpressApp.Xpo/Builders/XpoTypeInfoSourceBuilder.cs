using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.DC.Xpo;
using DevExpress.Xpo.Metadata;
using Scissors.Utils;

namespace Scissors.ExpressApp.Xpo.Builders
{
    /// <summary>
    /// A concrete builder to create an XpoTypeInfoSource
    /// </summary>
    public class XpoTypeInfoSourceBuilder : XpoTypeInfoSourceBuilder<XpoTypeInfoSource, XpoTypeInfoSourceBuilder>
    { }

    /// <summary>
    /// An non concrete builder to create an XpoTypeInfoSource
    /// </summary>
    /// <typeparam name="TXpoTypeInfoSource">The type of the XpoTypeInfoSource</typeparam>
    /// <typeparam name="TBuilder">The type of the builder</typeparam>
    public class XpoTypeInfoSourceBuilder<TXpoTypeInfoSource, TBuilder>
      where TXpoTypeInfoSource : XpoTypeInfoSource
      where TBuilder : XpoTypeInfoSourceBuilder<TXpoTypeInfoSource, TBuilder>
    {
        /// <summary>
        /// Returns an actual instance of the builder
        /// </summary>
        protected TBuilder This => (TBuilder)this;

        /// <summary>
        /// Creates an instance of the XpoTypeInfoSource
        /// </summary>
        /// <returns>The XpoTypeInfoSource created</returns>
        public virtual TXpoTypeInfoSource Build()
        {
            EnsureTypesInfo();

            if(Assemblies.Count > 0 && Dictionary == null)
            {
                Dictionary = new XafReflectionDictionary();
            }

            if(Dictionary != null && Types.Count > 0)
            {
                throw new InvalidOperationException($"Either specify a {nameof(Dictionary)} with {nameof(WithDictionary)}, {nameof(WithAssembly)} and {nameof(WithAssemblies)} methods or {nameof(Types)} with {nameof(WithTypes)} or {nameof(WithType)}");
            }

            if(Dictionary != null)
            {
                if(Assemblies.Count > 0)
                {
                    Dictionary.CollectClassInfos(Assemblies.ToArray());
                }

                var xpoTypeInfoSource = new XpoTypeInfoSource(TypesInfo, Dictionary);

                foreach(XPClassInfo classInfo in Dictionary.Classes)
                {
                    xpoTypeInfoSource.RegisterEntity(classInfo.ClassType);
                }

                return (TXpoTypeInfoSource)xpoTypeInfoSource;
            }

            if(Types.Count > 0)
            {
                return (TXpoTypeInfoSource)new XpoTypeInfoSource(TypesInfo, Types.ToArray());
            }

            return (TXpoTypeInfoSource)new XpoTypeInfoSource(TypesInfo);
        }

        /// <summary>
        /// The XPDictionary to use.
        /// </summary>
        protected XPDictionary Dictionary { get; set; }
        /// <summary>
        /// The XPDictionary to use. (optional)
        /// </summary>
        /// <remarks>Don't mix <see cref="WithDictionary(XPDictionary)"/> and <see cref="WithAssemblies(Assembly[])"/> or <see cref="WithAssembly(Assembly)"/> with <see cref="WithType(Type)"/> and <see cref="WithTypes(Type[])"/></remarks>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public virtual TBuilder WithDictionary(XPDictionary dictionary)
        {
            Dictionary = dictionary;
            return This;
        }

        /// <summary>
        /// The list of assemblies to use
        /// </summary>
        protected List<Assembly> Assemblies { get; } = new List<Assembly>();
        /// <summary>
        /// The assembly to add.
        /// </summary>
        /// <remarks>Don't mix <see cref="WithDictionary(XPDictionary)"/> and <see cref="WithAssemblies(Assembly[])"/> or <see cref="WithAssembly(Assembly)"/> with <see cref="WithType(Type)"/> and <see cref="WithTypes(Type[])"/></remarks>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public TBuilder WithAssembly(Assembly assembly)
        {
            Assemblies.Add(assembly);

            return This;
        }

        /// <summary>
        /// Adds the assemblies to use.
        /// Multiple calls are allowed
        /// </summary>
        /// <remarks>Don't mix <see cref="WithDictionary(XPDictionary)"/> and <see cref="WithAssemblies(Assembly[])"/> or <see cref="WithAssembly(Assembly)"/> with <see cref="WithType(Type)"/> and <see cref="WithTypes(Type[])"/></remarks>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public TBuilder WithAssemblies(params Assembly[] assemblies)
        {
            Assemblies.AddRange(assemblies);

            return This;
        }

        /// <summary>
        /// Must be of instance <see cref="TypesInfo"/>. (optional)
        /// </summary>
        /// <param name="typesInfo">the TypesInfo to use</param>
        /// <returns></returns>
        public TBuilder WithTypesInfo(ITypesInfo typesInfo)
            => WithTypesInfo((TypesInfo)typesInfo);

        /// <summary>
        /// The TypeInfo to use
        /// </summary>
        protected TypesInfo TypesInfo { get; set; }
        /// <summary>
        /// The TypesInfo to use (optional)
        /// </summary>
        /// <param name="typesInfo">the TypesInfo to use</param>
        /// <returns></returns>
        public TBuilder WithTypesInfo(TypesInfo typesInfo)
        {
            TypesInfo = typesInfo;
            return This;
        }

        /// <summary>
        /// Specifies the Types
        /// </summary>
        protected List<Type> Types { get; } = new List<Type>();
        /// <summary>
        /// Specifies a Type to use.
        /// Multiple calls are allowed
        /// </summary>
        /// <remarks>Don't mix <see cref="WithDictionary(XPDictionary)"/> and <see cref="WithAssemblies(Assembly[])"/> or <see cref="WithAssembly(Assembly)"/> with <see cref="WithType(Type)"/> and <see cref="WithTypes(Type[])"/></remarks>
        /// <param name="type">The type to use</param>
        /// <returns></returns>
        public virtual TBuilder WithType(Type type)
        {
            Types.Add(type);
            return This;
        }

        /// <summary>
        /// Specifies the Types to use.
        /// Multiple calls are allowed.
        /// </summary>
        /// <remarks>Don't mix <see cref="WithDictionary(XPDictionary)"/> and <see cref="WithAssemblies(Assembly[])"/> or <see cref="WithAssembly(Assembly)"/> with <see cref="WithType(Type)"/> and <see cref="WithTypes(Type[])"/></remarks>
        /// <param name="types">An array of Types to use</param>
        /// <returns></returns>
        public TBuilder WithTypes(params Type[] types)
        {
            Types.AddRange(types);
            return This;
        }

        void EnsureTypesInfo()
        {
            if(TypesInfo == null)
            {
                TypesInfo = new TypesInfo();
            }
        }
    }
}
