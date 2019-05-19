using System.Reflection;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model.Core;

namespace Scissors.ExpressApp.Model.Core
{
    /// <summary>
    /// A empty storage for model differences. Does not store anything.
    /// </summary>
    /// <seealso cref="DevExpress.ExpressApp.ModelStoreBase" />
    public class NullDiffsStore : ModelStoreBase
    {
        Assembly assembly;

        /// <summary>
        /// Initializes a new instance of the <see cref="NullDiffsStore"/> class.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public NullDiffsStore(Assembly assembly)
            => this.assembly = assembly;

        /// <summary>
        /// Gets the name of the current model difference store.
        /// </summary>
        /// <value>
        /// A string which is the name of the current model difference store.
        /// </value>
        public override string Name => $"{nameof(NullDiffsStore)} of the assembly '{assembly.FullName}'";

        /// <summary>
        /// Loads the model differences.
        /// </summary>
        /// <param name="model">An ModelApplicationBase object that specifies the Application Model.</param>
        public override void Load(ModelApplicationBase model)
        {
        }

        /// <summary>
        /// Indicates whether or not the current model differences store is readonly.
        /// </summary>
        /// <value>
        /// true, when the model differences store is readonly; otherwise, false.
        /// </value>
        public override bool ReadOnly => true;
    }
}
