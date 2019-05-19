using System;
using DevExpress.ExpressApp;
using Scissors.ExpressApp.Contracts;

namespace Scissors.ExpressApp.Builders
{
    /// <summary>
    /// An abstract base class to create ObjectSpaceProviders
    /// </summary>
    /// <typeparam name="TObjectSpaceProvider">The type of ObjectSpaceProvider to be build</typeparam>
    /// <typeparam name="TBuilder">The type of the builder</typeparam>
    public abstract class ObjectSpaceProviderBuilder<TObjectSpaceProvider, TBuilder>
        : IObjectSpaceProviderFactory
        , IObjectSpaceProviderFactory<TObjectSpaceProvider>
        where TObjectSpaceProvider : IObjectSpaceProvider
        where TBuilder : ObjectSpaceProviderBuilder<TObjectSpaceProvider, TBuilder>
    {
        /// <summary>
        /// The instance of the actual builder
        /// </summary>
        protected TBuilder This => (TBuilder)this;
        /// <summary>
        /// The actual lazy evaluated instance of the ObjectSpaceProvider
        /// </summary>
        protected Lazy<TObjectSpaceProvider> ObjectSpaceProvider { get; }

        /// <summary>
        /// Creates an instance of the ObjectSpaceProviderBuilder
        /// </summary>
        public ObjectSpaceProviderBuilder()
            => ObjectSpaceProvider = new Lazy<TObjectSpaceProvider>(Build);

        /// <summary>
        /// Factory method to create the builder
        /// </summary>
        /// <returns>The ObjectSpaceProvider that is built</returns>
        protected abstract TObjectSpaceProvider Create();

        /// <summary>
        /// Factory method to create the builder
        /// </summary>
        /// <returns>The ObjectSpaceProvider that is built</returns>
        public TObjectSpaceProvider Build()
        {
            var provider = Create();
            return provider;
        }

        IObjectSpaceProvider IObjectSpaceProviderFactory.CreateObjectSpaceProvider()
            => ObjectSpaceProvider.Value;

        TObjectSpaceProvider IObjectSpaceProviderFactory<TObjectSpaceProvider>.CreateObjectSpaceProvider()
            => ObjectSpaceProvider.Value;
    }
}
