using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.DC;
using Scissors.ExpressApp.ModelBuilders;
using Scissors.FeatureCenter.Module.BusinessObjects.InlineEditFormsDemos;

namespace Scissors.FeatureCenter.Module.ModelBuilders.InlineEditFormsDemos
{
    public class InlineEditFormsDemoModelBuilder : ModelBuilder<InlineEditFormsDemoModel>
    {
        public InlineEditFormsDemoModelBuilder(ITypeInfo typeInfo) : base(typeInfo) { }
        public override void Build()
        {
            base.Build();

            this
                .HasCaption("InlineEditForms")
                .HasNavigationItem("InlineEditForms");
        }
    }
}
