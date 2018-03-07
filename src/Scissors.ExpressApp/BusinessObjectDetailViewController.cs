using System;
using System.Linq;
using DevExpress.ExpressApp;

namespace Scissors.ExpressApp
{
    public class BusinessObjectDetailViewController<TObjectType> : BusinessObjectViewController<DetailView, TObjectType>
        where TObjectType : class
    {
    }
}
