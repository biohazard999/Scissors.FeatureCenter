using System;
using System.Linq;
using DevExpress.Xpo;
using Scissors.Data;
using Scissors.Xpo.Persistent;

namespace Scissors.FeatureCenter.Modules.BusinessObjects.LabelDemos
{
    [Persistent]
    public class LabelDemoModel : ScissorsBaseObjectOid
    {
        public static readonly ExpressionHelper<LabelDemoModel> Field = new ExpressionHelper<LabelDemoModel>();

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