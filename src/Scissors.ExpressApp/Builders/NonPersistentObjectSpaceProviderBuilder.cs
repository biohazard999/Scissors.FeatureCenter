using System;
using System.Linq;
using DevExpress.ExpressApp;

namespace Scissors.ExpressApp.Builders
{
    public class NonPersistentObjectSpaceProviderBuilder
        : NonPersistentObjectSpaceProviderBuilder<
            NonPersistentObjectSpaceProvider,
            NonPersistentObjectSpaceProviderBuilder>
    { }

    public class NonPersistentObjectSpaceProviderBuilder<TObjectSpaceProvider, TBuilder>
        : ObjectSpaceProviderBuilder<TObjectSpaceProvider, TBuilder>
        where TObjectSpaceProvider : NonPersistentObjectSpaceProvider
        where TBuilder : NonPersistentObjectSpaceProviderBuilder<TObjectSpaceProvider, TBuilder>
    {
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

        protected ITypesInfo TypesInfo { get; set; }
        protected TBuilder WithTypesInfo(ITypesInfo typesInfo)
        {
            TypesInfo = typesInfo;
            return This;
        }

        protected NonPersistentTypeInfoSource TypeInfoSource { get; set; }
        public TBuilder WithTypeInfoSource(NonPersistentTypeInfoSource typeInfoSource)
        {
            TypeInfoSource = typeInfoSource;
            return This;
        }
    }
}
