using System;
using System.Linq;

namespace Scissors.ExpressApp.ModelBuilders
{
    public interface IBuilderManager : IBuilder
    {
        IBuilderManager AddBuilder(IBuilder builder);
    }
}
