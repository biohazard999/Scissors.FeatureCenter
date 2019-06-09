using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acme.Module
{
    [Persistent]
    [DefaultClassOptions]
    public class MyFirstTestObjectToShow  : XPBaseObject
    {
        public MyFirstTestObjectToShow(Session session) : base(session) { }

        [Persistent("Oid"), Key(AutoGenerate = true)]
        int _Oid;
        [PersistentAlias(nameof(_Oid))]
        public int Oid { get => _Oid; }

        private string _Name;
        [Persistent("Name")]
        public string Name { get => _Name; set => SetPropertyValue(nameof(Name), ref _Name, value); }
    }
}
