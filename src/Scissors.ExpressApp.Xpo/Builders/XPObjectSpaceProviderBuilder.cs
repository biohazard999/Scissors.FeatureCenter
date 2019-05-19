using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.DC.Xpo;
using DevExpress.ExpressApp.Xpo;
using Scissors.ExpressApp.Builders;
using Scissors.Utils;

namespace Scissors.ExpressApp.Xpo.Builders
{
    /// <summary>
    /// A concrete builder to create an XPObjectSpaceProvider
    /// </summary>
    public class XPObjectSpaceProviderBuilder : XPObjectSpaceProviderBuilder<XPObjectSpaceProvider, XPObjectSpaceProviderBuilder>
    { }

    /// <summary>
    /// An abstract builder to create an XPObjectSpaceProvider
    /// </summary>
    /// <typeparam name="TObjectSpaceProvider">The type of XPObjectSpaceProvider to build</typeparam>
    /// <typeparam name="TBuilder">The type of the builder</typeparam>
    public class XPObjectSpaceProviderBuilder<TObjectSpaceProvider, TBuilder> : ObjectSpaceProviderBuilder<TObjectSpaceProvider, TBuilder>
        where TObjectSpaceProvider : XPObjectSpaceProvider
        where TBuilder : XPObjectSpaceProviderBuilder<TObjectSpaceProvider, TBuilder>
    {
        /// <summary>
        /// Creates an instance of the ObjectSpaceProvider
        /// </summary>
        /// <returns>An instance of the ObjectSpaceProvider</returns>
        protected override TObjectSpaceProvider Create()
        {
            Guard.AssertNotNull(DataStoreProvider, nameof(DataStoreProvider), $"DataStoreProvider is null, call the {nameof(WithDataStoreProvider)}, {nameof(WithConnectionString)} or {nameof(InMemory)} method to create an instance of '{typeof(TObjectSpaceProvider).FullName}'.");

            EnsureTypesInfo();

            return (TObjectSpaceProvider)new XPObjectSpaceProvider(
                DataStoreProvider,
                TypesInfo,
                XpoTypeInfoSource,
                IsThreadSafe ?? false);
        }

        /// <summary>
        /// The DataStoreProvider to create the ObjectSpaceProvider
        /// </summary>
        protected IXpoDataStoreProvider DataStoreProvider { get; set; }
        /// <summary>
        /// The DataStoreProvider to create the ObjectSpaceProvider
        /// </summary>
        /// <param name="dataStoreProvider">The DataStoreProvider to use</param>
        /// <returns></returns>
        public TBuilder WithDataStoreProvider(IXpoDataStoreProvider dataStoreProvider)
        {
            DataStoreProvider = dataStoreProvider;
            return This;
        }

        /// <summary>
        /// Use the MemoryDataStoreProvider as a DataStoreProvider
        /// </summary>
        /// <remarks>
        /// The <see cref="DataStoreProvider"/> will be set to an MemoryDataStoreProvider
        /// </remarks>
        /// <returns></returns>
        public TBuilder InMemory()
            => WithDataStoreProvider(new MemoryDataStoreProvider());

        /// <summary>
        /// The ConnectionString to be used
        /// </summary>
        protected string ConnectionString { get; set; }
        /// <summary>
        /// Specified the ConnectionString
        /// </summary>
        /// <remarks>
        /// The <see cref="DataStoreProvider"/> will be set to an ConnectionStringDataStoreProvider
        /// </remarks>
        /// <param name="connectionString">The ConnectionString to be used</param>
        /// <returns></returns>
        public TBuilder WithConnectionString(string connectionString)
        {
            ConnectionString = connectionString;

            return WithDataStoreProvider(new ConnectionStringDataStoreProvider(ConnectionString));
        }

        /// <summary>
        /// Specifies if an thread safe version of the ObjectSpaceProvider should be used
        /// </summary>
        protected bool? IsThreadSafe { get; set; }
        /// <summary>
        /// Specifies if an thread safe version of the ObjectSpaceProvider should be used
        /// </summary>
        /// <returns></returns>
        public TBuilder ThreadSafe()
            => WithThreadSafety(true);
        /// <summary>
        /// Specifies if an non thread safe version of the ObjectSpaceProvider should be used
        /// </summary>
        /// <returns></returns>
        public TBuilder NotThreadSafe()
            => WithThreadSafety(false);
        /// <summary>
        /// Specifies if the thread safety version of the ObjectSpaceProvider should be used
        /// </summary>
        /// <param name="value">specifies if the thread safety</param>
        /// <returns></returns>
        public TBuilder WithThreadSafety(bool value)
        {
            IsThreadSafe = value;
            return This;
        }

        /// <summary>
        /// The TypesInfo to be used
        /// </summary>
        protected ITypesInfo TypesInfo { get; set; }
        /// <summary>
        /// Specifies the TypesInfo to be used
        /// </summary>
        /// <remarks>
        /// When used for testing, this and the <see cref="WithTypesInfoSource"/> should always be used.
        /// If not side effects can happen.
        /// </remarks>
        /// <param name="typesInfo">The TypesInfo to be used</param>
        /// <returns></returns>
        public TBuilder WithTypesInfo(ITypesInfo typesInfo)
        {
            TypesInfo = typesInfo;
            return This;
        }

        /// <summary>
        /// The XpoTypeInfoSource to be used
        /// </summary>
        protected XpoTypeInfoSource XpoTypeInfoSource { get; set; }
        /// <summary>
        /// Specifies the XpoTypeInfoSource to be used
        /// </summary>
        /// <remarks>
        /// When used for testing, this and the <see cref="WithTypesInfo"/> should always be used.
        /// If not side effects can happen.
        /// </remarks>
        /// <param name="xpoTypeInfoSource">The XpoTypesInfoSource to be used</param>
        /// <returns></returns>
        public TBuilder WithTypesInfoSource(XpoTypeInfoSource xpoTypeInfoSource)
        {
            XpoTypeInfoSource = xpoTypeInfoSource;
            return This;
        }

        /// <summary>
        /// Ensures the <see cref="TypesInfo"/> and <see cref="XpoTypeInfoSource"/> are correct set.
        /// </summary>
        /// <remarks>
        /// When testing make sure to use <see cref="WithTypesInfo(ITypesInfo)"/> and <see cref="WithTypesInfoSource(XpoTypeInfoSource)"/> to avoid side effects.
        /// </remarks>
        protected void EnsureTypesInfo()
        {
            if(TypesInfo == null)
            {
                TypesInfo = XpoTypesInfoHelper.GetTypesInfo();
            }

            if(XpoTypeInfoSource == null)
            {
                XpoTypeInfoSource = XpoTypesInfoHelper.GetXpoTypeInfoSource();
            }
        }
    }
}
