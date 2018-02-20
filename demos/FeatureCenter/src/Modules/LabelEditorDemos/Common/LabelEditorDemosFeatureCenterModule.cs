using System;
using System.Collections.Generic;
using Scissors.ExpressApp;
using Scissors.FeatureCenter.Modules.LabelEditorDemos.BusinessObjects;

namespace Scissors.FeatureCenter.Modules.LabelEditorDemos
{
    public sealed class LabelEditorDemosFeatureCenterModule : ScissorsBaseModule
    {
        protected override IEnumerable<Type> GetDeclaredExportedTypes()
            => LabelEditorDemosBusinessObjects.Types;
    }
}