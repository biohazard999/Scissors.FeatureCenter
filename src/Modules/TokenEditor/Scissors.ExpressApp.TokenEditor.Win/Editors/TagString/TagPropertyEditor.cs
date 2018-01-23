using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Editors;

namespace Scissors.ExpressApp.TokenEditor.Win
{
    public class TagStringPropertyEditor : DXPropertyEditor
    {
        protected TagStringPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model)
        {
        }

        protected override object CreateControlCore()
        {
            throw new NotImplementedException();
        }
    }
}
