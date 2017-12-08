using System;
using System.Linq;
using DevExpress.ExpressApp;

namespace Scissors.ExpressApp
{
    public static class ModuleBaseExtentions
    {
        public static ModuleTypeList AndModuleTypes(this ModuleTypeList moduleTypeList, params Type[] types)
        {
            moduleTypeList.AddRange(types);
            return moduleTypeList;            
        }
    }
}
