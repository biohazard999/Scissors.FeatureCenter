using System;
using System.Linq;
using DevExpress.ExpressApp;

namespace Scissors.ExpressApp.Contracts
{
    /// <summary>
    /// Declares a Type that can instantiate an XafApplication
    /// </summary>
    /// <typeparam name="T">The actual application type</typeparam>
    public interface IApplicationFactory<out T> where T : XafApplication
    {
        /// <summary>
        /// Creates a new instance of the XafApplication
        /// </summary>
        /// <returns>The XafApplication to create</returns>
        T CreateApplication();
    }

    /// <summary>
    /// Declares a Type that can instantiate an XafApplication
    /// </summary>
    public interface IApplicationFactory
    {
        /// <summary>
        /// Creates a new instance of the XafApplication
        /// </summary>
        XafApplication CreateApplication();
    }
}
