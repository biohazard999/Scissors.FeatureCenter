using System;
using System.Linq;
using DevExpress.ExpressApp;

namespace Scissors.ExpressApp
{
    public class BusinessObjectListViewController<TObjectType> : BusinessObjectViewController<ListView, TObjectType>
        where TObjectType : class
    {
    }
}
