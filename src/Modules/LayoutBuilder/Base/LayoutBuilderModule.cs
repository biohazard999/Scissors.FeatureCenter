using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.SystemModule;
using Scissors.ExpressApp.LayoutBuilder.GeneratorUpdaters;

namespace Scissors.ExpressApp.LayoutBuilder
{
    /// <summary>
    /// 
    /// </summary>
    public class LayoutBuilderModule : ScissorsBaseModule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="updaters"></param>
        public override void AddGeneratorUpdaters(ModelNodesGeneratorUpdaters updaters)
        {
            base.AddGeneratorUpdaters(updaters);
            updaters.Add(new LayoutBuilderGeneratorUpdater());
        }
    }
}
