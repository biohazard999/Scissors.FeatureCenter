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
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Scissors.Utils.Testing.XUnit.CategoryAttribute" />
    public class IntegrationAttribute : CategoryAttribute
    {
        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public override string Category => "Integration";
    }
}
