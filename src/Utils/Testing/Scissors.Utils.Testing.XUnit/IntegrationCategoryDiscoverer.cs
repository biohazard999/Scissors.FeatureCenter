using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Scissors.Utils.Testing.XUnit
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Xunit.Sdk.ITraitDiscoverer" />
    public class CategoriesDiscoverer : ITraitDiscoverer
    {
        private const string key = "Category";
        /// <summary>
        /// Gets the trait values from the trait attribute.
        /// </summary>
        /// <param name="traitAttribute">The trait attribute containing the trait values.</param>
        /// <returns>
        /// The trait values.
        /// </returns>
        public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
        {
            var attributeInfo = traitAttribute as ReflectionAttributeInfo;
            var category = attributeInfo?.Attribute as CategoryAttribute;
            var value = category?.Category ?? string.Empty;
            yield return new KeyValuePair<string, string>(key, value);
        }
    }
}
