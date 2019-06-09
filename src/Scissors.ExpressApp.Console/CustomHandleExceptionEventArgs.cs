using System;
using System.ComponentModel;

namespace Scissors.ExpressApp.Console
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.ComponentModel.HandledEventArgs" />
    public class CustomHandleExceptionEventArgs : HandledEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomHandleExceptionEventArgs"/> class.
        /// </summary>
        /// <param name="e">The e.</param>
        public CustomHandleExceptionEventArgs(Exception e)
            => Exception = e;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomHandleExceptionEventArgs"/> class.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <param name="originalException">The original exception.</param>
        public CustomHandleExceptionEventArgs(Exception e, Exception originalException)
            : this(e) => OriginalException = originalException;
        
        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public Exception Exception { get; }
        
        /// <summary>
        /// Gets the original exception.
        /// </summary>
        /// <value>
        /// The original exception.
        /// </value>
        public Exception OriginalException { get; }
    }
}
