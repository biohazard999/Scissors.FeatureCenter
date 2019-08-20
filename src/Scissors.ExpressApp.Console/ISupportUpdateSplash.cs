namespace Scissors.ExpressApp.Console
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISupportUpdateSplash
    {
        /// <summary>
        /// Updates the splash.
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <param name="description">The description.</param>
        /// <param name="additionalParams">The additional parameters.</param>
        void UpdateSplash(string caption, string description, params object[] additionalParams);
    }
}
