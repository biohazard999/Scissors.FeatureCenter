using DevExpress.ExpressApp.DC;

namespace Scissors.ExpressApp.ModelBuilders
{
    public class XafBuilderManager : BuilderManager, ITypesInfoProvider
    {
        public ITypesInfo TypesInfo { get; }

        public XafBuilderManager(ITypesInfo typesInfo)
            => TypesInfo = typesInfo;

        protected virtual void AddBuilders()
        {

        }

        public override void Build()
        {
            AddBuilders();
            base.Build();
        }

        protected override void BuildBuilder(IBuilder builder)
        {
            base.BuildBuilder(builder);
            if(builder is ITypeInfoProvider)
            {
                TypesInfo.RefreshInfo(((ITypeInfoProvider)builder).TypeInfo);                
            }
        }
    }
}
