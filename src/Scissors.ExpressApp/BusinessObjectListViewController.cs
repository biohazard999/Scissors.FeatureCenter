using System;
using System.Linq;
using DevExpress.ExpressApp;

namespace Scissors.ExpressApp
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TObjectType">The type of the object type.</typeparam>
    /// <seealso cref="BusinessObjectViewController{ListView, TObjectType}" />
    public class BusinessObjectListViewController<TObjectType> : BusinessObjectViewController<ListView, TObjectType>
        where TObjectType : class
    {
    }
}
