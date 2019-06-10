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
#pragma warning disable CS0649 //Field will be filled by reflection
        int oid;
#pragma warning restore CS0649
        [PersistentAlias(nameof(oid))]
        public int Oid => oid;

        private string name;
        [Persistent("Name")]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }
    }
}
