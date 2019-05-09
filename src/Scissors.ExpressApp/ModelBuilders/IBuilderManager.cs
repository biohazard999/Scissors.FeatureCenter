using System;
using System.Linq;

namespace Scissors.ExpressApp.ModelBuilders
{
    public interface IBuilderManager
    {
        IBuilderManager AddBuilder(IBuilder builder);

        void Build();
    }
}
