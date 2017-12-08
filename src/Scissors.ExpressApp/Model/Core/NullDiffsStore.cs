using System;
using System.Linq;
using System.Reflection;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model.Core;

namespace Scissors.ExpressApp.Model.Core
{
    public class NullDiffsStore : ModelStoreBase
    {
        Assembly _Assembly;

        public NullDiffsStore(Assembly assembly)
            => _Assembly = assembly;
        
        public override string Name => $"{nameof(NullDiffsStore)} of the assembly '{_Assembly.FullName}'";

        public override void Load(ModelApplicationBase model)
        {
        }

        public override bool ReadOnly => true;
    }
}
