using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Scissors.Utils.Testing.XUnit
{
    [TraitDiscoverer("Scissors.Utils.Testing.XUnit.CategoriesDiscoverer", "Scissors.Utils.Testing.XUnit")]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Assembly)]
    public abstract class CategoryAttribute : Attribute, ITraitAttribute
    {
        public abstract string Category { get; }
    }
    public class IntegrationAttribute : CategoryAttribute
    {
        public override string Category => "Integration";
    }
    public class UITestAttribute : CategoryAttribute
    {
        public override string Category => "UITest";
    }
}
