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
    public abstract class ScissorsBaseObjectString : ScissorsBaseObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScissorsBaseObjectString"/> class.
        /// Uses the <see cref="CryptoRandom.CreateUniqueId(int, CryptoRandom.OutputFormat)"/> method to generate the <see cref="Oid"/>
        /// </summary>
        /// <param name="session">The session.</param>
        protected ScissorsBaseObjectString(Session session) : base(session) { }

        [Key(AutoGenerate = true)]
        [Persistent(nameof(Oid))]
        private string oid = CryptoRandom.CreateUniqueId();
        /// <summary>
        /// Gets the oid.
        /// </summary>
        /// <value>
        /// The oid.
        /// </value>
        [PersistentAlias(nameof(oid))]
        public string Oid => oid;
    }
}
