using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Updating;
using Scissors.ExpressApp.Model.Core;

namespace Scissors.ExpressApp
{
    /// <summary>
    /// The base class for a Scissors module
    /// This is an empty module, so nothing will be loaded by reflection.
    /// Everything needs to be registered manually.
    /// </summary>
    /// <seealso cref="ModuleBase" />
    public abstract class ScissorsBaseModule : ModuleBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScissorsBaseModule"/> class.
        /// </summary>
        public ScissorsBaseModule()
        { }
            //=> DiffsStore = new NullDiffsStore(GetType().Assembly);

        /// <summary>
        /// returns empty updaters
        /// </summary>
        /// <param name="objectSpace"></param>
        /// <param name="versionFromDB"></param>
        /// <returns></returns>
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB)
            => ModuleUpdater.EmptyModuleUpdaters;
        
        /// <summary>
        /// returns empty types
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<Type> GetDeclaredControllerTypes()
            => Type.EmptyTypes;
        
        /// <summary>
        /// returns empty types
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<Type> GetDeclaredExportedTypes()
            => Type.EmptyTypes;

        /// <summary>
        /// returns empty types
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<Type> GetRegularTypes()
            => Type.EmptyTypes;

        /// <summary>
        /// Adds the DevExpress.ExpressApp.SystemModule.SystemModule to the collection
        /// </summary>
        /// <returns></returns>
        protected override ModuleTypeList GetRequiredModuleTypesCore()
            => new ModuleTypeList(
                typeof(SystemModule)
            );

        /// <summary>
        /// Registers the editor descriptors.
        /// </summary>
        /// <param name="editorDescriptorsFactory">The editor descriptors factory.</param>
        protected override void RegisterEditorDescriptors(EditorDescriptorsFactory editorDescriptorsFactory)
        {
        }
    }
}
