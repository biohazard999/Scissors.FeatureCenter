using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.SystemModule;
using Scissors.ExpressApp;
using Scissors.FeatureCenter.Module.ModelBuilders;
using Scissors.FeatureCenter.Modules;
using Scissors.FeatureCenter.Modules.BusinessObjects;

namespace Scissors.FeatureCenter.Module
{
    public sealed class FeatureCenterModule : ScissorsBaseModule
    {
        protected override IEnumerable<Type> GetDeclaredExportedTypes()
            => DemosBusinessObjects.Types;

        protected override IEnumerable<Type> GetDeclaredControllerTypes()
            => DemosControllers.Types;

        public override void CustomizeTypesInfo(ITypesInfo typesInfo)
            => new DemosModelBuilderManager(typesInfo).Build();
    }
}
