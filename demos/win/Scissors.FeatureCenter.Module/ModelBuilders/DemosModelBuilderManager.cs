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
            ModelBuilder.Create<LabelDemosModelBuilder, LabelDemoModel>(TypesInfo)
        };
    }
}
