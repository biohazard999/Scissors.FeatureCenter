using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using Scissors.ExpressApp.Contracts;

namespace Scissors.ExpressApp.Builders
{
    /// <summary>
    /// Abstract base implementation of the builder
    /// </summary>
    /// <typeparam name="TApplication">Type of the application</typeparam>
    /// <typeparam name="TBuilder">The concrete type of the builder</typeparam>
    public abstract class XafApplicationBuilder<TApplication, TBuilder>
        : IApplicationFactory
        , IApplicationFactory<TApplication>
        where TApplication : XafApplication
        where TBuilder : XafApplicationBuilder<TApplication, TBuilder>
    {
        /// <summary>
        /// The actual lazy evaluated instance of the application
        /// </summary>
        protected Lazy<TApplication> Application { get; }

        /// <summary>
        /// Instance of the actual builder
        /// </summary>
        protected virtual TBuilder This => (TBuilder)this;

        /// <summary>
        /// Creates an actual instance of the application
        /// </summary>
        /// <returns>The actual application to build</returns>
        protected abstract TApplication Create();

        XafApplication IApplicationFactory.CreateApplication()
            => Application.Value;

        TApplication IApplicationFactory<TApplication>.CreateApplication()
            => Application.Value;

        /// <summary>
        /// Creates an instance of the builder
        /// </summary>
        public XafApplicationBuilder()
            => Application = new Lazy<TApplication>(Build);

        /// <summary>
        /// Builds up the application
        /// </summary>
        /// <returns>The application to build</returns>
        public virtual TApplication Build()
        {
            var application = Create();

            application.ConnectionString =
                string.IsNullOrEmpty(ConnectionString)
                ? application.ConnectionString
                : ConnectionString;

            application.ApplicationName =
                string.IsNullOrEmpty(ApplicationName)
                ? application.ApplicationName
                : ApplicationName;

            application.DatabaseUpdateMode =
                DatabaseUpdateMode
                ?? application.DatabaseUpdateMode;

            application.DelayedViewItemsInitialization =
                DelayedViewItemsInitialization
                ?? application.DelayedViewItemsInitialization;

            application.IsDelayedDetailViewDataLoadingEnabled =
                DelayedDetailViewDataLoadingEnabled
                ?? application.IsDelayedDetailViewDataLoadingEnabled;

            application.DefaultCollectionSourceMode =
                DefaultCollectionSourceMode
                ?? application.DefaultCollectionSourceMode;

            application.MaxLogonAttemptCount =
                MaxLogonAttemptCount
                ?? application.MaxLogonAttemptCount;

            application.EnableModelCache =
                EnableModelCache
                ?? application.EnableModelCache;

            application.CheckCompatibilityType =
                CheckCompatibilityType
                ?? application.CheckCompatibilityType;

            application.Connection =
                Connection
                ?? application.Connection;

            application.LinkNewObjectToParentImmediately =
                LinkNewObjectToParentImmediately
                ?? application.LinkNewObjectToParentImmediately;

            application.OptimizedControllersCreation =
                OptimizedControllersCreation
                ?? application.OptimizedControllersCreation;

            application.Title =
                string.IsNullOrEmpty(Title)
                ? application.Title
                : Title;

            if(ModuleFactories.Count > 0)
            {
                application.Modules.AddRange(ModuleFactories.Select((factory) => factory(application)));
            }

            if(ObjectSpaceProviderFactories.Count > 0)
            {
                application.CreateCustomObjectSpaceProvider += CreateCustomObjectSpaceProvider;
                void CreateCustomObjectSpaceProvider(object _, CreateCustomObjectSpaceProviderEventArgs args)
                {
                    foreach(var objectSpaceProviderFactory in ObjectSpaceProviderFactories)
                    {
                        args.ObjectSpaceProviders.Add(objectSpaceProviderFactory(args, application));
                    }
                    application.CreateCustomObjectSpaceProvider -= CreateCustomObjectSpaceProvider;
                }
            }

            return application;
        }



        /// <summary>
        /// Defines the ConnectionString for the application
        /// </summary>
        protected string ConnectionString { get; set; }
        /// <summary>
        /// Defines the ConnectionString for the application
        /// </summary>
        /// <param name="connectionString">The ConnectionString to use</param>
        /// <returns></returns>
        public TBuilder WithConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
            return This;
        }

        /// <summary>
        /// Defines the ApplicationName for the application
        /// </summary>
        protected string ApplicationName { get; set; }
        /// <summary>
        /// Defines the ApplicationName for the application
        /// </summary>
        /// <param name="applicationName">The ApplicationName to use</param>
        /// <returns></returns>
        public TBuilder WithApplicationName(string applicationName)
        {
            ApplicationName = applicationName;
            return This;
        }

        /// <summary>
        /// Defines the DatabaseUpdateMode for the application
        /// </summary>
        public DatabaseUpdateMode? DatabaseUpdateMode { get; set; }
        /// <summary>
        /// Defines the DatabaseUpdateMode for the application
        /// </summary>
        /// <param name="databaseUpdateMode">The DatabaseUpdateMode to use</param>
        /// <returns></returns>
        public TBuilder WithDatabaseUpdateMode(DatabaseUpdateMode databaseUpdateMode)
        {
            DatabaseUpdateMode = databaseUpdateMode;
            return This;
        }

        /// <summary>
        /// Defines the DelayedViewItemsInitialization for the application
        /// </summary>
        public bool? DelayedViewItemsInitialization { get; set; }
        /// <summary>
        /// Defines the DelayedViewItemsInitialization for the application
        /// </summary>
        /// <param name="delayedViewItemsInitialization">The DelayedViewItemsInitialization to use</param>
        /// <returns></returns>
        public TBuilder WithDelayedViewItemsInitialization(bool delayedViewItemsInitialization)
        {
            DelayedViewItemsInitialization = delayedViewItemsInitialization;
            return This;
        }

        /// <summary>
        /// Defines the IsDelayedDetailViewDataLoadingEnabled for the application
        /// </summary>
        public bool? DelayedDetailViewDataLoadingEnabled { get; set; }
        /// <summary>
        /// Defines the IsDelayedDetailViewDataLoadingEnabled for the application
        /// </summary>
        /// <param name="delayedDetailViewDataLoadingEnabled">The IsDelayedDetailViewDataLoadingEnabled to use</param>
        /// <returns></returns>
        public TBuilder WithDelayedDetailViewDataLoadingEnabled(bool delayedDetailViewDataLoadingEnabled)
        {
            DelayedDetailViewDataLoadingEnabled = delayedDetailViewDataLoadingEnabled;
            return This;
        }

        /// <summary>
        /// Defines the DefaultCollectionSourceMode for the application
        /// </summary>
        public CollectionSourceMode? DefaultCollectionSourceMode { get; set; }
        /// <summary>
        /// Defines the DefaultCollectionSourceMode for the application
        /// </summary>
        /// <param name="defaultCollectionSourceMode">the DefaultCollectionSourceMode to use</param>
        /// <returns></returns>
        public TBuilder WithDefaultCollectionSourceMode(CollectionSourceMode defaultCollectionSourceMode)
        {
            DefaultCollectionSourceMode = defaultCollectionSourceMode;
            return This;
        }

        /// <summary>
        /// Defines the MaxLogonAttemptCount for the application
        /// </summary>
        public int? MaxLogonAttemptCount { get; set; }
        /// <summary>
        /// Defines the MaxLogonAttemptCount for the application
        /// </summary>
        /// <param name="maxLogonAttemptCount">The MaxLogonAttemptCount to use</param>
        /// <returns></returns>
        public TBuilder WithMaxLogonAttemptCount(int maxLogonAttemptCount)
        {
            MaxLogonAttemptCount = maxLogonAttemptCount;
            return This;
        }

        /// <summary>
        /// Defines the UseModelCache for the application
        /// </summary>
        protected bool? EnableModelCache { get; set; }
        /// <summary>
        /// Defines the EnableModelCache for the application
        /// </summary>
        /// <param name="enableModelCache">The EnableModelCache to use</param>
        /// <returns></returns>
        public TBuilder WithEnableModelCache(bool enableModelCache)
        {
            EnableModelCache = enableModelCache;
            return This;
        }

        /// <summary>
        /// Defines the CheckCompatibilityType of the application
        /// </summary>
        protected CheckCompatibilityType? CheckCompatibilityType { get; set; }
        /// <summary>
        /// Defines the CheckCompatibilityType of the application
        /// </summary>
        /// <param name="checkCompatibilityType">The CheckCompatibilityType to use</param>
        /// <returns></returns>
        public TBuilder WithCheckCompatibilityType(CheckCompatibilityType checkCompatibilityType)
        {
            CheckCompatibilityType = checkCompatibilityType;
            return This;
        }

        /// <summary>
        /// Defines the Connection of the application
        /// </summary>
        protected IDbConnection Connection { get; set; }
        /// <summary>
        /// Defines the Connection of the application
        /// </summary>
        /// <param name="connection">The Connection to use</param>
        /// <returns></returns>
        public TBuilder WithConnection(IDbConnection connection)
        {
            Connection = connection;
            return This;
        }

        /// <summary>
        /// Defines the LinkNewObjectToParentImmediately of the application
        /// </summary>
        public bool? LinkNewObjectToParentImmediately { get; set; }
        /// <summary>
        /// Defines the LinkNewObjectToParentImmediately of the application
        /// </summary>
        /// <param name="linkNewObjectToParentImmediately">The LinkNewObjectToParentImmediately to use</param>
        /// <returns></returns>
        public TBuilder WithLinkNewObjectToParentImmediately(bool linkNewObjectToParentImmediately)
        {
            LinkNewObjectToParentImmediately = linkNewObjectToParentImmediately;
            return This;
        }

        /// <summary>
        /// Defines the OptimizedControllersCreation  of the application
        /// </summary>
        public bool? OptimizedControllersCreation { get; set; }
        /// <summary>
        /// Defines the OptimizedControllersCreation  of the application
        /// </summary>
        /// <param name="optimizedControllersCreation">The OptimizedControllersCreation to use</param>
        /// <returns></returns>
        public TBuilder WithOptimizedControllersCreation(bool optimizedControllersCreation)
        {
            OptimizedControllersCreation = optimizedControllersCreation;
            return This;
        }

        /// <summary>
        /// Defines the Title  of the application
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Defines the Title  of the application
        /// </summary>
        /// <param name="title">The Title to use</param>
        /// <returns></returns>
        public TBuilder WithTitle(string title)
        {
            Title = title;
            return This;
        }

        /// <summary>
        /// Defines a list of ObjectSpaceProviderFactories.
        /// Used in the CustomCreateObjectSpaceEvent to populate the ObjectSpaceProviders
        /// </summary>
        protected List<
            Func<CreateCustomObjectSpaceProviderEventArgs, TApplication, IObjectSpaceProvider>>
            ObjectSpaceProviderFactories
        { get; } = new List<
                Func<CreateCustomObjectSpaceProviderEventArgs, TApplication, IObjectSpaceProvider>>();

        /// <summary>
        /// Adds an ObjectSpaceProviderFactory to the application instance.
        /// Multiple calls are allowed.
        /// </summary>
        /// <param name="factory">A function that accepts the CreateCustomObjectSpaceProviderEventArgs, TApplication parameters and returns an IObjectSpaceProvider</param>
        /// <returns></returns>
        public TBuilder WithObjectSpaceProviderFactory(Func<CreateCustomObjectSpaceProviderEventArgs, TApplication, IObjectSpaceProvider> factory)
        {
            ObjectSpaceProviderFactories.Add(factory);
            return This;
        }

        /// <summary>
        /// The TypesInfo to be used
        /// </summary>
        /// <remarks>For internal use only</remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected ITypesInfo TypesInfo { get; set; }
        /// <summary>
        /// Specifies the TypesInfo to be used
        /// </summary>
        /// <param name="typesInfo">The TypesInfo to be used</param>
        /// <returns></returns>
        /// <remarks>
        /// For internal use only
        /// When used for testing make sure you understand the impacts, this should normally not be used.
        /// If not side effects can happen.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TBuilder WithTypesInfo(ITypesInfo typesInfo)
        {
            TypesInfo = typesInfo;
            return This;
        }

        /// <summary>
        /// Defines a list of ModuleFactories.
        /// Used to populate the Modules collection
        /// </summary>
        /// <value>
        /// The module factories.
        /// </value>
        protected List<Func<TApplication, ModuleBase>> ModuleFactories { get; } = new List<Func<TApplication, ModuleBase>>();

        /// <summary>
        /// Adds an ModuleFactory to the application instance.
        /// Multiple calls are allowed.
        /// </summary>
        /// <param name="factory">A function that accepts the TApplication parameter and returns an instance of the Module</param>
        /// <returns></returns>
        public TBuilder WithModuleFactory(Func<TApplication, ModuleBase> factory)
        {
            ModuleFactories.Add(factory);
            return This;
        }

        /// <summary>
        /// Adds an Module to the application Modules collection.
        /// </summary>
        /// <param name="module">The module to add.</param>
        /// <returns></returns>
        public TBuilder WithModule(ModuleBase module)
            => WithModuleFactory((_) => module);

        /// <summary>
        /// Adds an Module to the application Modules collection.
        /// </summary>
        /// <typeparam name="TModule">Type of the Module</typeparam>
        /// <returns></returns>
        public TBuilder WithModule<TModule>()
            where TModule : ModuleBase, new()
            => WithModuleFactory((_) => Activator.CreateInstance<TModule>());
    }
}
