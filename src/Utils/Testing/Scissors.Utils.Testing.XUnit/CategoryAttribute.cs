using System;
using System.Linq;
using Xunit.Sdk;

namespace Scissors.Utils.Testing.XUnit
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    /// <seealso cref="Xunit.Sdk.ITraitAttribute" />
    [TraitDiscoverer("Scissors.Utils.Testing.XUnit.CategoriesDiscoverer", "Scissors.Utils.Testing.XUnit")]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Assembly)]
    public abstract class CategoryAttribute : Attribute, ITraitAttribute
    {
        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public abstract string Category { get; }
    }
}
