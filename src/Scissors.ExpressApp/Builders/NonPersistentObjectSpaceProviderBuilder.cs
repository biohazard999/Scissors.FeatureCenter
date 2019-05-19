using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;

namespace Scissors.ExpressApp.Builders
{
    /// <summary>
    /// A concrete builder for an NonPersistentObjectSpaceProvider
    /// </summary>
    public class NonPersistentObjectSpaceProviderBuilder
        : NonPersistentObjectSpaceProviderBuilder<NonPersistentObjectSpaceProvider, NonPersistentObjectSpaceProviderBuilder>
    { }

    /// <summary>
    /// A non concrete builder for an NonPersistentObjectSpaceProvider
    /// </summary>
    /// <typeparam name="TObjectSpaceProvider">Specifies the concrete ObjectSpaceProviderType</typeparam>
    /// <typeparam name="TBuilder">Specifies the builder type</typeparam>
    public class NonPersistentObjectSpaceProviderBuilder<TObjectSpaceProvider, TBuilder>
        : ObjectSpaceProviderBuilder<TObjectSpaceProvider, TBuilder>
        where TObjectSpaceProvider : NonPersistentObjectSpaceProvider
        where TBuilder : NonPersistentObjectSpaceProviderBuilder<TObjectSpaceProvider, TBuilder>
    {
        /// <summary>
        /// Creates an instance of the TObjectSpaceProvider
        /// </summary>
        /// <returns>An instance of the ObjectSpaceProvider</returns>
        protected override TObjectSpaceProvider Create()
        {
            if(TypesInfo == null)
            {
                return (TObjectSpaceProvider)new NonPersistentObjectSpaceProvider();
            }

            if(TypesInfo is TypesInfo && TypeInfoSource == null)
            {
                return (TObjectSpaceProvider)new NonPersistentObjectSpaceProvider(TypesInfo, null);
            }

            return (TObjectSpaceProvider)new NonPersistentObjectSpaceProvider(TypesInfo, TypeInfoSource);
        }

        /// <summary>
        /// The TypesInfo to be used. If not specified the XafTypesInfo.Instance is used.
        /// </summary>
        protected ITypesInfo TypesInfo { get; set; }
        /// <summary>
        /// The TypesInfo to be used. If not specified the XafTypesInfo.Instance is used.
        /// </summary>
        /// <remarks>
        /// If used in testing the NonPersistentTypeInfoSource must be used, or an instance of the concrete TypesInfo class.
        /// If not, this will result in side effects.
        /// </remarks>
        /// <param name="typesInfo">The instance of the ITypesInfo to be used</param>
        /// <returns></returns>
        public TBuilder WithTypesInfo(ITypesInfo typesInfo)
        {
            TypesInfo = typesInfo;
            return This;
        }

        /// <summary>
        /// The NonPersistentTypeInfoSource
        /// </summary>
        protected NonPersistentTypeInfoSource TypeInfoSource { get; set; }
        /// <summary>
        /// The NonPersistentTypeInfoSource to be used.
        /// </summary>
        /// <remarks>
        /// If used in testing the NonPersistentTypeInfoSource must be used, or an instance of the concrete TypesInfo class.
        /// If not, this will result in side effects.
        /// </remarks>
        /// <param name="typeInfoSource">The NonPersistentTypeInfoSource to be used</param>
        /// <returns></returns>
        public TBuilder WithTypeInfoSource(NonPersistentTypeInfoSource typeInfoSource)
        {
            TypeInfoSource = typeInfoSource;
            return This;
        }
    }
}
