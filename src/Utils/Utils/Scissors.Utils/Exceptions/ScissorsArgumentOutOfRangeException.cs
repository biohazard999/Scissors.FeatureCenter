using System;
using System.Runtime.Serialization;

namespace Scissors.Utils.Exceptions
{
    public class ScissorsArgumentOutOfRangeException : ArgumentOutOfRangeException
    {
        public ScissorsArgumentOutOfRangeException() { }

        public ScissorsArgumentOutOfRangeException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public ScissorsArgumentOutOfRangeException(string message, Exception innerException)
            : base(message, innerException) { }

        public ScissorsArgumentOutOfRangeException(string paramName)
            : base(paramName) { }

        public ScissorsArgumentOutOfRangeException(string paramName, object actualValue, string message)
            : base(paramName, actualValue, message) { }

        public ScissorsArgumentOutOfRangeException(string paramName, string message)
            : base(paramName, message) { }

        public override string StackTrace
            => StackTraceCleaner.Clean(base.StackTrace);
    }
}