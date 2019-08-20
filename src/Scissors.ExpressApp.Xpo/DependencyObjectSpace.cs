using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.DC.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using DevExpress.Xpo.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Scissors.ExpressApp.Xpo
{
    /// <summary>
    /// This is a special <see cref="XPObjectSpace"/> that provides DependencyInjection capabilities via <see cref="Microsoft.Extensions.DependencyInjection"/>.
    /// </summary>
    /// <seealso cref="DevExpress.ExpressApp.Xpo.XPObjectSpace" />
    public class DependencyXPObjectSpace : XPObjectSpace
    {
        /// <summary>
        /// The dependency session wide data store key
        /// </summary>
        public static string DependencySessionWideDataStoreKey = nameof(serviceCollection);
        
        private readonly IServiceCollection serviceCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyXPObjectSpace"/> class.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <param name="typesInfo">The types information.</param>
        /// <param name="xpoTypeInfoSource">The xpo type information source.</param>
        /// <param name="createUnitOfWorkDelegate">The create unit of work delegate.</param>
        public DependencyXPObjectSpace(
            IServiceCollection serviceCollection,
            ITypesInfo typesInfo,
            XpoTypeInfoSource xpoTypeInfoSource,
            CreateUnitOfWorkHandler createUnitOfWorkDelegate
            ) : base(typesInfo, xpoTypeInfoSource, createUnitOfWorkDelegate)
                => this.serviceCollection = serviceCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyXPObjectSpace"/> class.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <param name="typesInfo">The types information.</param>
        /// <param name="xpoTypeInfoSource">The xpo type information source.</param>
        /// <param name="hostParametersMaxNumber">The host parameters maximum number.</param>
        /// <param name="createUnitOfWorkDelegate">The create unit of work delegate.</param>
        public DependencyXPObjectSpace(
            IServiceCollection serviceCollection,
            ITypesInfo typesInfo,
            XpoTypeInfoSource xpoTypeInfoSource,
            ushort hostParametersMaxNumber,
            CreateUnitOfWorkHandler createUnitOfWorkDelegate
            ) : base(typesInfo, xpoTypeInfoSource, hostParametersMaxNumber, createUnitOfWorkDelegate)
                => this.serviceCollection = serviceCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyXPObjectSpace" /> class.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <param name="typesInfo">The types information.</param>
        /// <param name="xpoTypeInfoSource">The xpo type information source.</param>
        /// <param name="hostParametersMaxNumber">The host parameters maximum number.</param>
        /// <param name="session">The session.</param>
        public DependencyXPObjectSpace(
            IServiceCollection serviceCollection,
            ITypesInfo typesInfo,
            XpoTypeInfoSource xpoTypeInfoSource,
            ushort hostParametersMaxNumber,
            UnitOfWork session
            ) : base(typesInfo, xpoTypeInfoSource, hostParametersMaxNumber, session)
                => this.serviceCollection = serviceCollection;

        /// <summary>
        /// Creates a nested Object Space.
        /// </summary>
        /// <returns>
        /// An <see cref="T:DevExpress.ExpressApp.IObjectSpace" /> object that is a created nested Object Space.
        /// </returns>
        public override IObjectSpace CreateNestedObjectSpace()
            => this;


        /// <summary>
        /// Sets the session.
        /// </summary>
        /// <param name="newSession">The new session.</param>
        protected override void SetSession(UnitOfWork newSession)
        {
            UpdateSession(newSession);
            base.SetSession(newSession);
        }

        private void UpdateSession(UnitOfWork newSession)
        {
            if(newSession == null)
            {
                return;
            }
            
            serviceCollection.AddScoped<Session>(_ => newSession);
            serviceCollection.AddScoped(_ => newSession);

            ((IWideDataStorage)newSession)?.SetWideDataItem(DependencySessionWideDataStoreKey, serviceCollection);
        }

        /// <summary>
        /// <para>Releases all resources used by an <see cref="DevExpress.ExpressApp.Xpo.XPObjectSpace"/> object.</para>
        /// </summary>
        public override void Dispose()
        {
            ((IWideDataStorage)Session)?.SetWideDataItem(DependencySessionWideDataStoreKey, null);

            //serviceCollection.Dispose();
            base.Dispose();
        }
    }
}
