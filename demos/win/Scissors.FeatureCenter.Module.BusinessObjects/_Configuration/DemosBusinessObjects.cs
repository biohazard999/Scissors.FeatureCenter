using System;
using System.Linq;
using Scissors.FeatureCenter.Module.BusinessObjects.InlineEditFormsDemos;
using Scissors.FeatureCenter.Modules.BusinessObjects.LabelDemos;

namespace Scissors.FeatureCenter.Modules.BusinessObjects
{
    public static class DemosBusinessObjects
    {
        public static readonly Type[] Types = new[]
        {
            typeof(LabelDemoModel),
            typeof(InlineEditFormsDemoModel)
        };
    }
}