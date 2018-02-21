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

        string _Html;
        public string Html
        {
            get => _Html;
            set
            {
                if(SetPropertyValue(ref _Html, value))
                {
                    Text = _Html;
                }
            }
        }
    }
}