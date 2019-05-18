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
    public abstract class ScissorsBaseObjectOid : ScissorsBaseObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScissorsBaseObjectOid"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        protected ScissorsBaseObjectOid(Session session) : base(session) { }

        /// <summary>
        /// The oid
        /// </summary>
        [Key(AutoGenerate = true)]
        [Persistent(nameof(Oid))]
        private int oid = -1;
        /// <summary>
        /// Gets the oid.
        /// </summary>
        /// <value>
        /// The oid.
        /// </value>
        [PersistentAlias(nameof(oid))]
        public int Oid => oid;
    }
}
