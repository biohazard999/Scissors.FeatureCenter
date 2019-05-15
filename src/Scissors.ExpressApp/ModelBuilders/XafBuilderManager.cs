using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp.DC;

namespace Scissors.ExpressApp.ModelBuilders
{
    public class XafBuilderManager : BuilderManager, ITypesInfoProvider
    {
        public static IEnumerable<IBuilder> EmptyBuilders { get; } = new IBuilder[0];
        public ITypesInfo TypesInfo { get; }
        public IEnumerable<IBuilder> Builders { get; }

        public XafBuilderManager(ITypesInfo typesInfo) : this(typesInfo, EmptyBuilders) { }

        public XafBuilderManager(ITypesInfo typesInfo, IEnumerable<IBuilder> builders)
        {
            TypesInfo = typesInfo;
            Builders = builders;
        }

        public static XafBuilderManager Create(ITypesInfo typesInfo, IEnumerable<IBuilder> builders)
            => new XafBuilderManager(typesInfo, builders);

        protected virtual IEnumerable<IBuilder> GetBuilders() => Builders;

        public override void Build()
        {
            foreach(var builder in GetBuilders())
            {
                AddBuilder(builder);
            }

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
