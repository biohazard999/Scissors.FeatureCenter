using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp;
using Scissors.ExpressApp.Contracts;

namespace Scissors.ExpressApp.Builders
{
    /// <summary>
    /// Abstract base implementation of the builder
    /// </summary>
    /// <typeparam name="TApplication">Type of the application</typeparam>
    /// <typeparam name="TBuilder">The concrete type of the builder</typeparam>
    public abstract class XafApplicationBuilder<TApplication, TBuilder> : IApplicationFactory, IApplicationFactory<TApplication>
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
            => Application = new Lazy<TApplication>(() => Build());

        /// <summary>
        /// Builds up the application
        /// </summary>
        /// <returns>The application to build</returns>
        protected virtual TApplication Build()
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
    }
}
