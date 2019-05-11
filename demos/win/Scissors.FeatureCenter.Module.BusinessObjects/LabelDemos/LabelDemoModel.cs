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

        private string text;
        [Persistent]
        public string Text
        {
            get => text;
            set => SetPropertyValue(ref text, value);
        }

        private string html;
        public string Html
        {
            get => html;
            set
            {
                if(SetPropertyValue(ref html, value))
                {
                    Text = html;
                }
            }
        }
    }
}