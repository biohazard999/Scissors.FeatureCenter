namespace Scissors.ExpressApp.Console
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISplash
    {
        /// <summary>
        /// Starts this instance.
        /// </summary>
        void Start();
        /// <summary>
        /// Stops this instance.
        /// </summary>
        void Stop();
        /// <summary>
        /// Gets a value indicating whether this instance is started.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is started; otherwise, <c>false</c>.
        /// </value>
        bool IsStarted { get; }
        /// <summary>
        /// Sets the display text.
        /// </summary>
        /// <param name="displayText">The display text.</param>
        void SetDisplayText(string displayText);
    }
}
