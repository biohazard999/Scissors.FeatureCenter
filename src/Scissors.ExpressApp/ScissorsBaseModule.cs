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
    /// <inheritdoc />
    /// <summary>
    /// The base class for a Scissors module
    /// This is an empty module, so nothing will be loaded by reflection.
    /// Everything needs to be registered manually.
    /// <seealso cref="ModuleBase" />
    /// </summary>
    public abstract class ScissorsBaseModule : ModuleBase
    {
        /// <inheritdoc />
        /// <summary></summary>
        public ScissorsBaseModule()
            => DiffsStore = new NullDiffsStore(GetType().Assembly);

        /// <inheritdoc />
        /// <summary>
        /// returns empty updaters
        /// </summary>
        /// <param name="objectSpace"></param>
        /// <param name="versionFromDB"></param>
        /// <returns></returns>
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB)
            => ModuleUpdater.EmptyModuleUpdaters;
        
        /// <inheritdoc />
        /// <summary>
        /// returns empty types
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<Type> GetDeclaredControllerTypes()
            => Type.EmptyTypes;
        
        /// <inheritdoc />
        /// <summary>
        /// returns empty types
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<Type> GetDeclaredExportedTypes()
            => Type.EmptyTypes;

        /// <inheritdoc />
        /// <summary>
        /// returns empty types
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<Type> GetRegularTypes()
            => Type.EmptyTypes;

        /// <inheritdoc />
        /// <summary>
        /// Adds the DevExpress.ExpressApp.SystemModule.SystemModule to the collection
        /// </summary>
        /// <returns></returns>
        protected override ModuleTypeList GetRequiredModuleTypesCore()
            => new ModuleTypeList(
                typeof(SystemModule)
            );

        /// <inheritdoc />
        protected override void RegisterEditorDescriptors(EditorDescriptorsFactory editorDescriptorsFactory)
        {
        }
    }
}
