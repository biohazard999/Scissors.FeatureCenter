using System;
using System.Linq;
using DevExpress.ExpressApp;

namespace Scissors.ExpressApp
{
    /// <summary>
    /// 
    /// </summary>
    public static class ModuleBaseExtentions
    {
        /// <summary>
        /// Ands the module types.
        /// </summary>
        /// <param name="moduleTypeList">The module type list.</param>
        /// <param name="types">The types.</param>
        /// <returns></returns>
        public static ModuleTypeList AndModuleTypes(this ModuleTypeList moduleTypeList, params Type[] types)
        {
            moduleTypeList.AddRange(types);
            return moduleTypeList;            
        }
    }
}
