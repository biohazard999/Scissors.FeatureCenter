using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using Scissors.ExpressApp.Contracts;

namespace Scissors.ExpressApp.Builders
{
    public abstract class ObjectSpaceProviderBuilder<TObjectSpaceProvider, TBuilder>
        : IObjectSpaceProviderFactory
        , IObjectSpaceProviderFactory<TObjectSpaceProvider>
        where TObjectSpaceProvider : IObjectSpaceProvider
        where TBuilder : ObjectSpaceProviderBuilder<TObjectSpaceProvider, TBuilder>
    {
        protected TBuilder This => (TBuilder)this;
        /// <summary>
        /// The actual lazy evaluated instance of the ObjectSpaceProvider
        /// </summary>
        protected Lazy<TObjectSpaceProvider> ObjectSpaceProvider { get; }

        public ObjectSpaceProviderBuilder()
            => ObjectSpaceProvider = new Lazy<TObjectSpaceProvider>(Build);

        protected abstract TObjectSpaceProvider Create();
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
