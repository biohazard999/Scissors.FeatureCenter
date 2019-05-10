using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.DC;
using Scissors.ExpressApp.ModelBuilders;
using Scissors.FeatureCenter.Module.ModelBuilders.LabelDemos;
using Scissors.FeatureCenter.Modules.BusinessObjects.LabelDemos;

namespace Scissors.FeatureCenter.Module.ModelBuilders
{
    public class DemosModelBuilderManager : XafBuilderManager
    {
        public DemosModelBuilderManager(ITypesInfo typesInfo) : base(typesInfo) { }

        protected override IEnumerable<IBuilder> CreateBuilders() => new[]
        {
            Create<LabelDemosModelBuilder, LabelDemoModel>(TypesInfo)
        };

        public static TBuilder Create<TBuilder, T>(ITypesInfo typesInfo)
           where TBuilder : IModelBuilder<T>
                => (TBuilder)Activator.CreateInstance(typeof(TBuilder), typesInfo.FindTypeInfo<T>());
    }
}
