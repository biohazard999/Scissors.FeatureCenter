using System;
using System.Collections.Generic;
using System.Linq;

namespace Scissors.ExpressApp.ModelBuilders
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="IBuilderManager" />
    public class BuilderManager : IBuilderManager
    {
        private List<IBuilder> builders = new List<IBuilder>();

        /// <summary>
        /// Adds an builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public virtual IBuilderManager AddBuilder(IBuilder builder)
        {
            builders.Add(builder);
            return this;
        }

        /// <summary>
        /// Adds the builders.
        /// </summary>
        /// <param name="builders">The builders.</param>
        /// <returns></returns>
        public virtual IBuilderManager AddBuilders(IEnumerable<IBuilder> builders)
        {
            this.builders.AddRange(builders);
            return this;
        }

        /// <summary>
        /// Gets the builders.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<IBuilder> GetBuilders() => builders;

        /// <summary>
        /// Builds this instance.
        /// </summary>
        public virtual void Build()
        {
            foreach(var builder in GetBuilders())
            {
                BuildBuilder(builder);
            }
        }

        /// <summary>
        /// Builds the builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected virtual void BuildBuilder(IBuilder builder)
            => builder.Build();
    }
}
