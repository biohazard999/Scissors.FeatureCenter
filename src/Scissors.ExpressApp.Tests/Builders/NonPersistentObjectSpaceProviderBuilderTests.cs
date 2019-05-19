using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using Scissors.ExpressApp.Builders;
using Shouldly;
using Xunit;

namespace Scissors.ExpressApp.Tests.Builders
{
    public class NonPersistentObjectSpaceProviderBuilderTests
    {
        public class WithoutTypesInfo
        {
            NonPersistentObjectSpaceProvider provider;
            public WithoutTypesInfo()
                => provider = new NonPersistentObjectSpaceProviderBuilder()
                    .Build();
            
            [Fact]
            public void XafTypesInfoShouldBeUsed()
                => provider.TypesInfo.ShouldBe(XafTypesInfo.Instance);
            
            [Fact]
            public void EntityStoreIsOfTypeNonPersistentTypeInfoSource()
                => provider.EntityStore.ShouldBeOfType<NonPersistentTypeInfoSource>();
        }

        public class WithTypesInfo
        {
            NonPersistentObjectSpaceProvider provider;
            TypesInfo typesInfo = new TypesInfo();
            public WithTypesInfo()
                => provider = new NonPersistentObjectSpaceProviderBuilder()
                    .WithTypesInfo(typesInfo)
                    .Build();

            [Fact]
            public void CustomTypesInfoShouldBeUsed()
                => provider.TypesInfo.ShouldBe(typesInfo);

            [Fact]
            public void EntityStoreIsNull()
                => provider.EntityStore.ShouldBeNull();
        }

        public class WithTypesInfoAndNonPersistentTypeInfoSource
        {
            NonPersistentObjectSpaceProvider provider;
            TypesInfo typesInfo = new TypesInfo();
            NonPersistentTypeInfoSource entityStore;

            public WithTypesInfoAndNonPersistentTypeInfoSource()
            {
                entityStore = new NonPersistentTypeInfoSource(typesInfo);
                provider = new NonPersistentObjectSpaceProviderBuilder()
                    .WithTypesInfo(typesInfo)
                    .WithTypeInfoSource(entityStore)
                    .Build();
            }

            [Fact]
            public void CustomTypesInfoShouldBeUsed()
                => provider.TypesInfo.ShouldBe(typesInfo);

            [Fact]
            public void EntityStoreIsNull()
                => provider.EntityStore.ShouldBe(entityStore);
        }
    }
}
