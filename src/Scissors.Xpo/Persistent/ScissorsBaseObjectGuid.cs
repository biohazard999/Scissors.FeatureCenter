using System;
using System.Linq;
using DevExpress.Xpo;

namespace Scissors.Xpo.Persistent
{
    [NonPersistent]
    public abstract class ScissorsBaseObjectGuid : ScissorsBaseObject
    {
        protected ScissorsBaseObjectGuid(Session session) : base(session) { }

        [Key(AutoGenerate = true)]
        [Persistent(nameof(Oid))]
        private Guid _Oid;
        [PersistentAlias(nameof(_Oid))]
        public Guid Oid => _Oid;
    }
}
