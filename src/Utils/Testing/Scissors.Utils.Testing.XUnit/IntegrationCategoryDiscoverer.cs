using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Scissors.Utils.Testing.XUnit
{
    public class CategoriesDiscoverer : ITraitDiscoverer
    {
        private const string key = "Category";
        public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
        {
            var attributeInfo = traitAttribute as ReflectionAttributeInfo;
            var category = attributeInfo?.Attribute as CategoryAttribute;
            var value = category?.Category ?? string.Empty;
            yield return new KeyValuePair<string, string>(key, value);
        }
    }
}
