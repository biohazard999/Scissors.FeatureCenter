using System;
using System.Linq;
using DevExpress.ExpressApp;

namespace Scissors.ExpressApp
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TObjectType">The type of the object type.</typeparam>
    /// <seealso cref="BusinessObjectViewController{DetailView, TObjectType}" />
    public class BusinessObjectDetailViewController<TObjectType> : BusinessObjectViewController<DetailView, TObjectType>
        where TObjectType : class
    {
    }
}
