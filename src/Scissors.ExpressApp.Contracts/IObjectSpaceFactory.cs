using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp;

namespace Scissors.ExpressApp.Contracts
{
    /// <summary>
    /// Declares a Type that can instantiate an IObjectSpace
    /// </summary>
    public interface IObjectSpaceFactory
    {
        /// <summary>
        /// Creates a new instance of an IObjectSpace
        /// </summary>
        /// <param name="objectType">The target type to create</param>
        /// <returns>An actual IObjectSpace instance</returns>
        IObjectSpace CreateObjectSpace(Type objectType);
    }

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
