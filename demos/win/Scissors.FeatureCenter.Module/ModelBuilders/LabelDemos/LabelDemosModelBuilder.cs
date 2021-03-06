using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.DC;
using Scissors.ExpressApp.LabelEditor.Contracts;
using Scissors.ExpressApp.LayoutBuilder.Contracts;
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
                .HasCaption("LabelPropertyEditor")
                .HasNavigationItem("PropertyEditors");

            For(p => p.Text)
                .UsingLabelPropertyEditor();

            For(p => p.Html)
                .ImmediatePostsData();


            WithAttribute(new DetailViewLayoutBuilderAttribute(DefaultDetailView, new Layout
            {
                Main =
                {
                    Items =
                    {
                        new TabGroup("TG1")
                        {
                            Items =
                            {
                                new Tab("T1")
                                {
                                    Items =
                                    {
                                        new ViewItem(Exp.Property(p => p.Html))
                                    }
                                },
                                new Tab("T2")
                                {
                                    Items =
                                    {
                                        new ViewItem(Exp.Property(p => p.Text))
                                    }
                                }
                            }
                        }
                    }
                }
            }));

            base.Build();
        }
    }
}
