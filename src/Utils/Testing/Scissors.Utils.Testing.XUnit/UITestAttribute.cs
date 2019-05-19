using System;
using System.Linq;

namespace Scissors.Utils.Testing.XUnit
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Scissors.Utils.Testing.XUnit.CategoryAttribute" />
    public class UITestAttribute : CategoryAttribute
    {
        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public override string Category => "UITest";
    }
}
