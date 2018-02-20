using System;
using System.Linq;
using DevExpress.Xpo;
using Scissors.Xpo.Persistent;

namespace Scissors.FeatureCenter.Modules.LabelEditorDemos.BusinessObjects
{
    [Persistent]
    public class LabelDemoModel : ScissorsBaseObjectOid
    {
        public LabelDemoModel(Session session) : base(session) { }

        string _Text;
        [Persistent]
        public string Text
        {
            get => _Text;
            set => SetPropertyValue(ref _Text, value);
        }
    }
}