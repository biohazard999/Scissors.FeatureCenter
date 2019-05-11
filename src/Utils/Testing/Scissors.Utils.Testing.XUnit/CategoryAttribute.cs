using System;
using System.Linq;
using Xunit.Sdk;

namespace Scissors.Utils.Testing.XUnit
{
    [TraitDiscoverer("Scissors.Utils.Testing.XUnit.CategoriesDiscoverer", "Scissors.Utils.Testing.XUnit")]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Assembly)]
    public abstract class CategoryAttribute : Attribute, ITraitAttribute
    {
        public abstract string Category { get; }
    }
}
