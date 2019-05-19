using System;
using System.Runtime.Serialization;

namespace Scissors.Utils.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.ArgumentException" />
    [Serializable]
    public class ScissorsArgumentException : ArgumentException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScissorsArgumentException"/> class.
        /// </summary>
        public ScissorsArgumentException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScissorsArgumentException" /> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        public ScissorsArgumentException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScissorsArgumentException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public ScissorsArgumentException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScissorsArgumentException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public ScissorsArgumentException(string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScissorsArgumentException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="paramName">The name of the parameter that caused the current exception.</param>
        public ScissorsArgumentException(string message, string paramName)
            : base(message, paramName) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScissorsArgumentException" /> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="paramName">The name of the parameter that caused the current exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public ScissorsArgumentException(string message, string paramName, Exception innerException)
            : base(message, paramName, innerException) { }

        /// <summary>
        /// Gets a string representation of the immediate frames on the call stack.
        /// </summary>
        public override string StackTrace
            => StackTraceCleaner.Clean(base.StackTrace);
    }
}