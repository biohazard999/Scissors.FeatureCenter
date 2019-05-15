using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.SystemModule;
using Scissors.ExpressApp.Builders;
using Scissors.ExpressApp.LayoutBuilder.Contracts;
using Scissors.ExpressApp.LayoutBuilder.GeneratorUpdaters;
using Scissors.ExpressApp.ModelBuilders;
using Shouldly;
using Xunit;

namespace Scissors.ExpressApp.LayoutBuilder.Tests.GeneratorUpdaters
{ 
  

    public class LayoutBuilderGeneratorUpdaterTests
    {
        public class TestModule : ScissorsBaseModule
        {
            protected override ModuleTypeList GetRequiredModuleTypesCore() => base.GetRequiredModuleTypesCore()
                .AndModuleTypes(typeof(LayoutBuilderModule));

            protected override IEnumerable<Type> GetDeclaredExportedTypes() => new[]
            {
                typeof(TestClass),
            };
            public override void CustomizeTypesInfo(ITypesInfo typesInfo)
            {
                base.CustomizeTypesInfo(typesInfo);
                var builder = ModelBuilder.Create<TestClass>(typesInfo);
                builder.WithAttribute(new DetailViewLayoutBuilderAttribute(builder.DefaultDetailView, new Layout()
                {

                }));

                XafBuilderManager.Create(typesInfo, new[] { builder }).Build();
            }
            public class TestClass
            {
                public string StringProperty { get; set; }
                public int IntProperty { get; set; }
            }

        }

        [Fact]
        public void ClearsLayoutWithAttribute()
        {
            var app = new HeadlessXafApplicationBuilder()
                   .WithTypesInfo(new TypesInfo())
                   .WithObjectSpaceProviderFactory((args, a) => new NonPersistentObjectSpaceProviderBuilder()
                   .WithTypeInfoSource(new NonPersistentTypeInfoSource(a.TypesInfo))
                   .Build())
                   .WithModule<SystemModule>()
                   .WithModule<LayoutBuilderModule>()
                   .WithModule<TestModule>()
                   .Build();

            app.Setup();

            var detailView = app.FindModelView(app.GetDetailViewId(typeof(TestModule.TestClass)));

            detailView.ShouldSatisfyAllConditions
            (
                () => detailView.ShouldBeAssignableTo<IModelDetailView>(),
                () => ((IModelDetailView)detailView).Layout.Count.ShouldBe(1),
                () => ((IModelDetailView)detailView).Layout.First().ShouldBeAssignableTo<IModelLayoutGroup>(),
                () => ((IModelDetailView)detailView).Layout.OfType<IModelLayoutGroup>().First().Id.ShouldBe("Main"),
                () => ((IModelDetailView)detailView).Layout.OfType<IModelLayoutGroup>().First().Count.ShouldBe(0)
            );
        }
    }
}
