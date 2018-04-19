using System;
using System.Runtime.Serialization;

namespace Scissors.Utils.Exceptions
{
    public class ScissorsArgumentNullException : ArgumentNullException
    {
        public ScissorsArgumentNullException() { }

        public ScissorsArgumentNullException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public ScissorsArgumentNullException(string message, Exception innerException)
            : base(message, innerException) { }

        public ScissorsArgumentNullException(string paramName)
            : base(paramName) { }

        public ScissorsArgumentNullException(string paramName, string message)
            : base(paramName, message) { }

        public override string StackTrace
            => StackTraceCleaner.Clean(base.StackTrace);
    }
}
