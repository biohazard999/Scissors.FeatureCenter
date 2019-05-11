using System;
using System.Collections.Generic;
using System.Linq;

namespace Scissors.ExpressApp.ModelBuilders
{
    public class BuilderManager : IBuilderManager
    {
        protected IList<IBuilder> Builders = new List<IBuilder>();

        public IBuilderManager AddBuilder(IBuilder builder)
        {
            Builders.Add(builder);
            return this;
        }

        public virtual void Build()
        {
            foreach(var builder in Builders)
            {
                BuildBuilder(builder);
            }
        }

        protected virtual void BuildBuilder(IBuilder builder)
            => builder.Build();
    }
}
