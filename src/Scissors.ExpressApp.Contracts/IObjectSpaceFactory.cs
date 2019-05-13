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
}
