using System;
using System.Linq;
using DevExpress.ExpressApp;

namespace Scissors.ExpressApp.Contracts
{
    /// <summary>
    /// Declares a Type that can instantiate an XafApplication
    /// </summary>
    public interface IObjectSpaceProviderFactory
    {
        /// <summary>
        /// Creates a new instance of the XafApplication
        /// </summary>
        IObjectSpaceProvider CreateObjectSpaceProvider();
    }

    /// <summary>
    /// Declares a Type that can instantiate an XafApplication
    /// </summary>
    /// <typeparam name="T">The actual application type</typeparam>
    public interface IObjectSpaceProviderFactory<out T> where T : IObjectSpaceProvider
    {
        /// <summary>
        /// Creates a new instance of the XafApplication
        /// </summary>
        /// <returns>The XafApplication to create</returns>
        T CreateObjectSpaceProvider();
    }
}
