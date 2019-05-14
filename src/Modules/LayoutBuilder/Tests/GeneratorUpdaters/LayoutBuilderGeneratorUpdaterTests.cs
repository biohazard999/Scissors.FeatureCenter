using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.SystemModule;
using Scissors.ExpressApp.Builders;
using Scissors.ExpressApp.LayoutBuilder.GeneratorUpdaters;
using Xunit;

namespace Scissors.ExpressApp.LayoutBuilder.Tests.GeneratorUpdaters
{
    public class LayoutBuilderGeneratorUpdaterTests
    {
        [Fact]
        public void Foo()
        {
            var app = new HeadlessXafApplicationBuilder()
                   .WithTypesInfo(new TypesInfo())
                   .WithObjectSpaceProviderFactory((args, a) => new NonPersistentObjectSpaceProviderBuilder()
                   .WithTypeInfoSource(new NonPersistentTypeInfoSource(a.TypesInfo))
                   .Build())
                   .WithModule<SystemModule>()
                   .WithModule<LayoutBuilderModule>()
                   .Build();

            app.Setup();

            //sut.UpdateNode(node);
        }
    }
}
