using System;
using System.Runtime.Serialization;

namespace Scissors.Utils.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.ArgumentNullException" />
    [Serializable]
    public class ScissorsArgumentNullException : ArgumentNullException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScissorsArgumentNullException"/> class.
        /// </summary>
        public ScissorsArgumentNullException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScissorsArgumentNullException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">An object that describes the source or destination of the serialized data.</param>
        public ScissorsArgumentNullException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScissorsArgumentNullException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for this exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public ScissorsArgumentNullException(string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScissorsArgumentNullException"/> class.
        /// </summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        public ScissorsArgumentNullException(string paramName)
            : base(paramName) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScissorsArgumentNullException" /> class.
        /// </summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        public ScissorsArgumentNullException(string paramName, string message)
            : base(paramName, message) { }

        /// <summary>
        /// Gets a string representation of the immediate frames on the call stack.
        /// </summary>
        public override string StackTrace
            => StackTraceCleaner.Clean(base.StackTrace);
    }
}
