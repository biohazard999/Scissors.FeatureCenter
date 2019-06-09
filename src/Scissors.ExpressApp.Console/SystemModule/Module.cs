using Scissors.ExpressApp.Console.Core;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Templates.ActionControls.Binding;
using DevExpress.ExpressApp.Updating;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
