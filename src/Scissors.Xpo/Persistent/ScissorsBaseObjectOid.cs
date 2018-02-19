using System;
using System.Linq;
using DevExpress.Xpo;

namespace Scissors.Xpo.Persistent
{
    [NonPersistent]
    public abstract class ScissorsBaseObjectOid : ScissorsBaseObject
    {
        protected ScissorsBaseObjectOid(Session session) : base(session) { }

        [Key(AutoGenerate = true)]
        [Persistent(nameof(Oid))]
        private int _Oid = -1;
        [PersistentAlias(nameof(_Oid))]
        public int Oid => _Oid;
    }
}
