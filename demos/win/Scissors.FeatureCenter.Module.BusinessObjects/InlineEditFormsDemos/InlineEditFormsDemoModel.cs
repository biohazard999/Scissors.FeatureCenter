using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpo;
using Scissors.Data;
using Scissors.Xpo.Persistent;

namespace Scissors.FeatureCenter.Module.BusinessObjects.InlineEditFormsDemos
{
    [Persistent]
    public class InlineEditFormsDemoModel : ScissorsBaseObjectOid
    {
        public static readonly ExpressionHelper<InlineEditFormsDemoModel> Field = new ExpressionHelper<InlineEditFormsDemoModel>();

        public InlineEditFormsDemoModel(Session session) : base(session) { }
    }
}
