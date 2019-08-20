using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.DC.Xpo;
using DevExpress.ExpressApp.Xpo;
using Microsoft.Extensions.DependencyInjection;

namespace Scissors.ExpressApp.Xpo
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DevExpress.ExpressApp.Xpo.XPObjectSpaceProvider" />
    public class DependencyXPObjectSpaceProvider : DevExpress.ExpressApp.Xpo.XPObjectSpaceProvider
    {
        /// <summary>
        /// 
        /// </summary>
        public IServiceScopeFactory ServiceScopeFactory { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceScopeFactory"></param>
        /// <param name="dataStoreProvider"></param>
        /// <param name="threadSafe"></param>
        public DependencyXPObjectSpaceProvider(IServiceScopeFactory serviceScopeFactory, IXpoDataStoreProvider dataStoreProvider, bool threadSafe) : base(dataStoreProvider, threadSafe)
            => ServiceScopeFactory = serviceScopeFactory;

        /// <summary>
        /// <para></para>
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="connection"></param>
        /// <param name="threadSafe"></param>
        public DependencyXPObjectSpaceProvider(string connectionString, IDbConnection connection, bool threadSafe) : base(connectionString, connection, threadSafe)
        {

        }

        /// <summary>
        /// <para></para>
        /// </summary>
        /// <param name="dataStoreProvider"></param>
        /// <param name="typesInfo"></param>
        /// <param name="xpoTypeInfoSource"></param>
        /// <param name="threadSafe"></param>
        public DependencyXPObjectSpaceProvider(IXpoDataStoreProvider dataStoreProvider, ITypesInfo typesInfo, XpoTypeInfoSource xpoTypeInfoSource, bool threadSafe) : base(dataStoreProvider, typesInfo, xpoTypeInfoSource, threadSafe)
        {

        }

        /// <summary>
        /// <para></para>
        /// </summary>
        /// <param name="dataStoreProvider"></param>
        public DependencyXPObjectSpaceProvider(IXpoDataStoreProvider dataStoreProvider) : base(dataStoreProvider)
        {

        }

        /// <summary>
        /// <para></para>
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="connection"></param>
        public DependencyXPObjectSpaceProvider(string connectionString, IDbConnection connection) : base(connectionString, connection)
        {

        }

        /// <summary>
        /// <para></para>
        /// </summary>
        /// <param name="connection"></param>
        public DependencyXPObjectSpaceProvider(IDbConnection connection) : base(connection)
        {

        }

        /// <summary>
        /// <para></para>
        /// </summary>
        /// <param name="connectionString"></param>
        public DependencyXPObjectSpaceProvider(string connectionString) : base(connectionString)
        {

        }

        /// <summary>
        /// <para></para>
        /// </summary>
        /// <param name="dataStoreProvider"></param>
        /// <param name="typesInfo"></param>
        /// <param name="xpoTypeInfoSource"></param>
        public DependencyXPObjectSpaceProvider(IXpoDataStoreProvider dataStoreProvider, ITypesInfo typesInfo, XpoTypeInfoSource xpoTypeInfoSource) : base(dataStoreProvider, typesInfo, xpoTypeInfoSource)
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override IObjectSpace CreateObjectSpaceCore()
        {
            var scope = ServiceScopeFactory.CreateScope();

            var os = new DependencyXPObjectSpace(scope.ServiceProvider.GetService<IServiceCollection>(), TypesInfo, XpoTypeInfoSource, () => CreateUnitOfWork(DataLayer));

            scope.ServiceProvider.GetRequiredService<IServiceCollection>().AddScoped<IObjectSpace>((_) => os);

            return os;
        }

        //IObjectSpace IObjectSpaceProvider.CreateUpdatingObjectSpace(bool allowUpdateSchema)
        //{
        //    var scope = _dependencyService.BeginScope();

        //    var nestedService = scope.GetService<IDependencyService>();

        //    var os = new DependencyObjectSpace(nestedService, TypesInfo, XpoTypeInfoSource, () => CreateUpdatingUnitOfWork(allowUpdateSchema));

        //    nestedService.RegisterAsExternalControlled<IObjectSpace>(os);

        //    return os;
        //}
    }
}
