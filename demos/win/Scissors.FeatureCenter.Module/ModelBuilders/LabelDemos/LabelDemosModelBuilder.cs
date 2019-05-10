using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.DC;
using Scissors.ExpressApp.ModelBuilders;
using Scissors.FeatureCenter.Modules.BusinessObjects.LabelDemos;

namespace Scissors.FeatureCenter.Module.ModelBuilders.LabelDemos
{
    public class LabelDemosModelBuilder : ModelBuilder<LabelDemoModel>
    {
        public LabelDemosModelBuilder(ITypeInfo typeInfo) : base(typeInfo) { }

        public override void Build()
        {
            this
                .HasCaption("Label-Property-Editor-Demos")
                .HasNavigationItem("PropertyEditors");

            base.Build();
        }
    }
}
