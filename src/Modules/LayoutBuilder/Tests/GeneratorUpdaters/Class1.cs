using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.Model.Core;
using Scissors.ExpressApp.LayoutBuilder.GeneratorUpdaters;
using Xunit;

namespace Scissors.ExpressApp.LayoutBuilder.Tests.GeneratorUpdaters
{
    public class LayoutBuilderGeneratorUpdaterTests
    {
        [Fact]
        public void Foo()
        {
            var sut = new LayoutBuilderGeneratorUpdater();

            sut.UpdateNode(node);
        }
    }
}
