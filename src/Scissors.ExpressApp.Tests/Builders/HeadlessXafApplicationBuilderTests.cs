using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.SystemModule;
using FakeItEasy;
using Scissors.ExpressApp.Builders;
using Scissors.Utils.Testing.XUnit;
using Shouldly;
using Xunit;

namespace Scissors.ExpressApp.Tests.Builders
{
    public class HeadlessXafApplicationBuilderTests
    {
        protected HeadlessXafApplicationBuilder CreateBuilder()
            => new HeadlessXafApplicationBuilder();

        public class WithConnectionString : HeadlessXafApplicationBuilderTests
        {
            [Theory]
            [InlineData("ABC")]
            public void ShouldBe(string connectionString)
                => CreateBuilder()
                    .WithConnectionString(connectionString)
                    .Build()
                        .ConnectionString.ShouldBe(connectionString);
        }

        public class WithApplicationName : HeadlessXafApplicationBuilderTests
        {
            [Theory]
            [InlineData("My awesome App")]
            public void ShouldBe(string applicationName)
                => CreateBuilder()
                    .WithApplicationName(applicationName)
                    .Build()
                        .ApplicationName.ShouldBe(applicationName);
        }

        public class WithDatabaseUpdateMode : HeadlessXafApplicationBuilderTests
        {
            [Theory]
            [InlineData(DatabaseUpdateMode.Never)]
            [InlineData(DatabaseUpdateMode.UpdateDatabaseAlways)]
            [InlineData(DatabaseUpdateMode.UpdateOldDatabase)]
            public void ShouldBe(DatabaseUpdateMode databaseUpdateMode)
                => CreateBuilder()
                    .WithDatabaseUpdateMode(databaseUpdateMode)
                    .Build()
                        .DatabaseUpdateMode.ShouldBe(databaseUpdateMode);
        }

        public class WithDelayedViewItemsInitialization : HeadlessXafApplicationBuilderTests
        {
            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void ShouldBe(bool delayedViewItemsInitialization)
                => CreateBuilder()
                    .WithDelayedViewItemsInitialization(delayedViewItemsInitialization)
                    .Build()
                        .DelayedViewItemsInitialization.ShouldBe(delayedViewItemsInitialization);
        }

        public class WithDelayedDetailViewDataLoadingEnabled : HeadlessXafApplicationBuilderTests
        {
            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void ShouldBe(bool isDelayedDetailViewDataLoadingEnabled)
                => CreateBuilder()
                    .WithDelayedDetailViewDataLoadingEnabled(isDelayedDetailViewDataLoadingEnabled)
                    .Build()
                        .IsDelayedDetailViewDataLoadingEnabled.ShouldBe(isDelayedDetailViewDataLoadingEnabled);
        }

        public class WithDefaultCollectionSourceMode : HeadlessXafApplicationBuilderTests
        {
            [Theory]
            [InlineData(CollectionSourceMode.Normal)]
            [InlineData(CollectionSourceMode.Proxy)]
            public void ShouldBe(CollectionSourceMode collectionSourceMode)
                => CreateBuilder()
                    .WithDefaultCollectionSourceMode(collectionSourceMode)
                    .Build()
                        .DefaultCollectionSourceMode.ShouldBe(collectionSourceMode);
        }

        public class WithMaxLogonAttemptCount : HeadlessXafApplicationBuilderTests
        {
            [Theory]
            [InlineData(0)]
            [InlineData(5)]
            [InlineData(1000)]
            public void ShouldBe(int maxLogonAttemptCount)
                => CreateBuilder()
                    .WithMaxLogonAttemptCount(maxLogonAttemptCount)
                    .Build()
                        .MaxLogonAttemptCount.ShouldBe(maxLogonAttemptCount);
        }

        public class WithEnableModelCache : HeadlessXafApplicationBuilderTests
        {
            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void ShouldBe(bool enableModelCache)
                => CreateBuilder()
                    .WithEnableModelCache(enableModelCache)
                    .Build()
                        .EnableModelCache.ShouldBe(enableModelCache);
        }

        public class WithCheckCompatibilityType : HeadlessXafApplicationBuilderTests
        {
            [Theory]
            [InlineData(CheckCompatibilityType.DatabaseSchema)]
            [InlineData(CheckCompatibilityType.ModuleInfo)]
            public void ShouldBe(CheckCompatibilityType checkCompatibilityType)
                => CreateBuilder()
                    .WithCheckCompatibilityType(checkCompatibilityType)
                    .Build()
                        .CheckCompatibilityType.ShouldBe(checkCompatibilityType);
        }

        public class WithConnection : HeadlessXafApplicationBuilderTests
        {
            [Fact]
            public void ShouldBe()
            {
                var connection = A.Fake<IDbConnection>();
                CreateBuilder()
                    .WithConnection(connection)
                    .Build()
                        .Connection.ShouldBe(connection);
            }
        }

        public class WithLinkNewObjectToParentImmediately : HeadlessXafApplicationBuilderTests
        {
            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void ShouldBe(bool linkNewObjectToParentImmediately)
                => CreateBuilder()
                    .WithLinkNewObjectToParentImmediately(linkNewObjectToParentImmediately)
                    .Build()
                        .LinkNewObjectToParentImmediately.ShouldBe(linkNewObjectToParentImmediately);
        }

        public class WithOptimizedControllersCreation : HeadlessXafApplicationBuilderTests
        {
            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void ShouldBe(bool optimizedControllersCreation)
                => CreateBuilder()
                    .WithOptimizedControllersCreation(optimizedControllersCreation)
                    .Build()
                        .OptimizedControllersCreation.ShouldBe(optimizedControllersCreation);
        }

        public class WithTitle : HeadlessXafApplicationBuilderTests
        {
            [Theory]
            [InlineData("My awesome Title")]
            public void ShouldBe(string title)
                => CreateBuilder()
                    .WithTitle(title)
                    .Build()
                        .Title.ShouldBe(title);
        }

        public class WithTypesInfo : HeadlessXafApplicationBuilderTests
        {
            [Fact]
            public void ShouldUse()
            {
                var typesInfo = A.Dummy<ITypesInfo>();
                CreateBuilder()
                    .WithTypesInfo(typesInfo)
                    .Build()
                        .TypesInfo.ShouldBe(typesInfo);
            }

            [Fact]
            public void ShouldUseXafTypesInfoInstance()
                => CreateBuilder()
                    .Build()
                        .TypesInfo.ShouldBe(XafTypesInfo.Instance);

        }

        public class WithModule : HeadlessXafApplicationBuilderTests
        {
            public class TestModule1 : ModuleBase { }

            public class TestModule2 : ModuleBase { }

            private TestModule1 module1 = new TestModule1();

            [Fact]
            public void ShouldSelfCreatedModule()
                => CreateBuilder()
                    .WithModule(module1)
                    .Build()
                        .Modules.ShouldContain(module1);

            [Fact]
            public void ShouldBuilderCreatedModule()
                => CreateBuilder()
                    .WithModule<TestModule1>()
                    .Build()
                        .Modules.ShouldContain(m => m.GetType() == typeof(TestModule1));

            [Fact]
            public void ShouldContainModulesInOrder()
            {
                var application = CreateBuilder()
                    .WithModule<TestModule1>()
                    .WithModule<TestModule2>()
                    .Build();

                application.ShouldSatisfyAllConditions(
                    () => application.Modules[0].ShouldBeOfType<SystemModule>(),
                    () => application.Modules[1].ShouldBeOfType<TestModule1>(),
                    () => application.Modules[2].ShouldBeOfType<TestModule2>()
                );
            }
        }

        public class WithModuleFactory : HeadlessXafApplicationBuilderTests
        {
            public class TestModule1 : ModuleBase { }

            public class TestModule2 : ModuleBase { }

            private TestModule1 module1 = new TestModule1();
            private TestModule2 module2 = new TestModule2();

            [Fact]
            public void ShouldSelfCreatedModule()
                => CreateBuilder()
                    .WithModuleFactory((_) => module1)
                    .Build()
                        .Modules.ShouldContain(module1);

            [Fact]
            public void ShouldContainModulesInOrder()
            {
                var application = CreateBuilder()
                    .WithModuleFactory((_) => module1)
                    .WithModuleFactory((_) => module2)
                    .Build();

                application.ShouldSatisfyAllConditions(
                    () => application.Modules[0].ShouldBeOfType<SystemModule>(),
                    () => application.Modules[1].ShouldBe(module1),
                    () => application.Modules[2].ShouldBe(module2)
                );
            }

            [Fact]
            public void ShouldPassThroughAnApplication()
               => CreateBuilder()
                   .WithModuleFactory((app) =>
                   {
                       app.ShouldBeOfType<HeadlessXafApplication>();
                       return module1;
                   })
                   .Build();
        }

        public class WithObjectSpaceProviderBuilder : HeadlessXafApplicationBuilderTests
        {
            [Fact]
            [Integration]
            public void ShouldAddProvider()
            {
                var application = CreateBuilder()
                    .WithTypesInfo(new TypesInfo())
                    .WithObjectSpaceProviderFactory((args, app) => new NonPersistentObjectSpaceProviderBuilder()
                    .WithTypesInfo(app.TypesInfo)
                    .WithTypeInfoSource(new NonPersistentTypeInfoSource(app.TypesInfo))
                    .Build())
                 .Build();
                application.Setup();

                application.ObjectSpaceProviders.ShouldSatisfyAllConditions(
                    () => application.ObjectSpaceProviders.Count.ShouldBe(1),
                    () => application.ObjectSpaceProviders.First().ShouldBeOfType<NonPersistentObjectSpaceProvider>()
                );
            }
        }
    }
}
