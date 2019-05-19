using System;
using System.Linq;

namespace Scissors.ExpressApp.ModelBuilders
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="IBuilder" />
    public interface IBuilderManager : IBuilder
    {
        /// <summary>
        /// Adds the builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        IBuilderManager AddBuilder(IBuilder builder);
    }
}
