using System;
using System.Linq;
using DevExpress.Xpo;

namespace Scissors.Xpo.Persistent
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Scissors.Xpo.Persistent.ScissorsBaseObject" />
    [NonPersistent]
    public abstract class ScissorsBaseObjectGuid : ScissorsBaseObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScissorsBaseObjectGuid"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        protected ScissorsBaseObjectGuid(Session session) : base(session) { }

        [Key(AutoGenerate = true)]
        [Persistent(nameof(Oid))]
        private Guid oid = default;
        /// <summary>
        /// Gets the oid.
        /// </summary>
        /// <value>
        /// The oid.
        /// </value>
        [PersistentAlias(nameof(oid))]
        public Guid Oid => oid;
    }
}
