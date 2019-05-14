using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scissors.ExpressApp.Builders
{
    /// <summary>
    /// Concrete Headless application builder.
    /// Useful for testing or service work.
    /// </summary>
    public class HeadlessXafApplicationBuilder : HeadlessXafApplicationBuilder<HeadlessXafApplication, HeadlessXafApplicationBuilder> {  }

    /// <summary>
    /// Abstract Headless application builder.
    /// Useful for testing or service work.
    /// </summary>
    public class HeadlessXafApplicationBuilder<TApplication, TBuilder> : XafApplicationBuilder<TApplication, TBuilder>
        where TApplication : HeadlessXafApplication
        where TBuilder : HeadlessXafApplicationBuilder<TApplication, TBuilder>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override TApplication Create() => (TApplication)
            (TypesInfo == null
            ? new HeadlessXafApplication()
            : new HeadlessXafApplication(TypesInfo));
    }
}
