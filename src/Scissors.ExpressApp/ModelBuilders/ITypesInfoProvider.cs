using DevExpress.ExpressApp.DC;

namespace Scissors.ExpressApp.ModelBuilders
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITypesInfoProvider
    {
        /// <summary>
        /// Gets the types information.
        /// </summary>
        /// <value>
        /// The types information.
        /// </value>
        ITypesInfo TypesInfo { get; }
    }
}
