using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.Editors;
using Scissors.ExpressApp.Console.Editors;

namespace Scissors.ExpressApp.Console.SystemModule
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Scissors.ExpressApp.ScissorsBaseModule" />
    public sealed class SystemConsoleModule : ScissorsBaseModule
    {
        /// <summary>
        /// returns empty types
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<Type> GetDeclaredControllerTypes() => new[]
        {
            typeof(ExitController)
        };

        /// <summary>
        /// Registers the editor descriptors.
        /// </summary>
        /// <param name="editorDescriptorsFactory">The editor descriptors factory.</param>
        protected override void RegisterEditorDescriptors(EditorDescriptorsFactory editorDescriptorsFactory)
            => editorDescriptorsFactory.RegisterListEditor(EditorAliases.GridListEditor, typeof(object), typeof(GridListEditor), true);
    }
}
